#include "Application.h"

void static custom_delay_ms(uint16_t milliseconds) {
#ifdef __AVR__
    _delay_ms(milliseconds); // Use _delay_ms() for AVR microcontroller
#else
    usleep(milliseconds * 1000); // Use usleep() for POSIX systems (convert milliseconds to microseconds)
#endif
}

#define HardwareId 1

_Bool UnlockingApproved=false;
char received_message_buffer[128];


int sendTempAndHumidity(int hardwareId){
    uint8_t *data = getTempAndHum(hardwareId); 
    transmitData(data, 16);
    free(data);
    return 1;
}

int sendLight(int hardwareId){
    uint8_t *data = getLightInfo(hardwareId); 
    transmitData(data,16);
    free(data);
    return 1;
}

int sendReadings()
{
    sendTempAndHumidity(HardwareId);
    custom_delay_ms(1000);
    sendLight(HardwareId);

    return 1;
}

int windowAction(uint8_t status,int hardwareId){
    //to indicate that we are moving the window with a servo we are going all the way up, all the way down 
    //then going to the middle and waits for a second and then if open then going up is closed going down
    if (status)
    {
        openWindow(hardwareId);
        return 1;
    }
    else
    {
        closeWindow(hardwareId);
        return 0;
    }
}


_Bool doorApproval(){
    if (UnlockingApproved)
        UnlockingApproved = false;
    return UnlockingApproved;
}

bool doorAction(uint8_t status){
     if (status){
        UnlockingApproved = true;
        openDoor();
    }
    else{
        closeDoor();
        UnlockingApproved=false;
    }
    return UnlockingApproved;
}


char * breakingIn(){
    char * x = Alarm(UnlockingApproved);
    if (strcmp(x,"Hello, Thief! :)")==0)
    {
        transmitData((uint8_t*)x,16);
    }
    return x;
}

int Callback(){
    uint8_t* msg = decryption((uint8_t *)received_message_buffer);
    pc_comm_send_array_blocking((uint8_t*)msg,16);
    
     uint8_t id =msg[0]-'0';
    int x = 0;
    uint8_t value;
    if (msg[1]-'0' == 3)
    {
        value = msg[3] - '0';
        doorAction(value);
        x = 3;
    }

    if (id==HardwareId){
        switch (msg[1]){
        case '1':
            value = msg[3] - '0';
            uint8_t * radiator= setRadiatorLevel(value,HardwareId);
            free(radiator);
            x = 1;
            break;
        case '2':
            value = msg[3] - '0';
            windowAction(value,HardwareId);
            x = 2;
            break;
        case '4':
            value = msg[3] - '0';
            char * light= AdjustLight(value,HardwareId);
            free(light);
            x = 4;
            break;
        default:
            break;
        }
    }
    free(msg);
    return x;
}

int start(){
    pc_comm_init(9600,NULL);
    wifi_init();
    dht11_init();
    light_init();
    display_init();
    leds_init();
    hc_sr04_init();

    encryptionStart();
    
    connect(Callback,received_message_buffer);

    taskSend(sendReadings);
    taskDoor(doorApproval);
    taskSecurity(breakingIn);

    return 1;
}