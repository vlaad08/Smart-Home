#include "Application.h"

void static custom_delay_ms(uint16_t milliseconds) {
#ifdef __AVR__
    _delay_ms(milliseconds); // Use _delay_ms() for AVR microcontroller
#else
    usleep(milliseconds * 1000); // Use usleep() for POSIX systems (convert milliseconds to microseconds)
#endif
}

#define HardwareId 1

struct AES_ctx my_AES_ctx;
bool UnlockingApproved=false;
char received_message_buffer[128];
uint8_t key[] = "S3cor3P45Sw0rD@f";
    
void transmitData(uint8_t * data,uint16_t length){
    AES_ECB_encrypt(&my_AES_ctx,(uint8_t*)data);
    
    wifi_command_TCP_transmit((uint8_t*)data,length);

    AES_ECB_decrypt(&my_AES_ctx,(uint8_t*)data);
    
    pc_comm_send_array_blocking((uint8_t*)data,length);

    free(data);
}

int sendTempAndHumidity(int hardwareId){
    uint8_t *data = getTempAndHum(hardwareId); 
    transmitData(data, 16);

    return 1;
}

int sendLight(int hardwareId){
    uint8_t *data = getLightInfo(hardwareId); 
    transmitData(data,16);

    return 1;
}

int sendReadings(int hardwareId)
{
    sendTempAndHumidity(hardwareId);
    custom_delay_ms(1000);
    sendLight(hardwareId);

    return 1;
}

void windowAction(uint8_t status,int hardwareId){
    //to indicate that we are moving the window with a servo we are going all the way up, all the way down 
    //then going to the middle and waits for a second and then if open then going up is closed going down
    if (status)
        openWindow(hardwareId);
    else
        closeWindow(hardwareId);
}


bool doorApproval(){
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
    char * x = alarm(UnlockingApproved);
    if (strcmp(x,"Hello, Thief! :)")==0)
    {
        transmitData((uint8_t*)x,16);
    }

    return x;
}

void Callback(){
    uint8_t id=received_message_buffer[0]-'0';
    if (id==HardwareId){
        uint8_t value;
        switch (received_message_buffer[1]){
        case '1':
            value = received_message_buffer[3] - '0';
            uint8_t * radiator= setRadiatorLevel(value,HardwareId);
            free(radiator);
            break;
        case '2':
            value = received_message_buffer[3] - '0';
            windowAction(value,HardwareId);
            break;
        case '3':
            value = received_message_buffer[3] - '0';
            doorAction(value);
            break;
        case '4':
            value = received_message_buffer[3] - '0';
            char * light= AdjustLight(value,HardwareId);
            free(light);
            break;
        default:
            break;
        }
    }
}

void sendReadingsWrapper(){
    (void)sendReadings(HardwareId);
}

void doorApprovalWrapper(){
    (void) doorApproval();
}

void breakingInWrapper(){
    (void) breakingIn();
}

int start(){
    pc_comm_init(9600,NULL);
    wifi_init();
    dht11_init();
    light_init();
    display_init();
    leds_init();
    hc_sr04_init();
    AES_init_ctx(&my_AES_ctx,key);

    
    //wifi_command_join_AP("Filip's Galaxy S21 FE 5G","jgeb6522");
    wifi_command_join_AP("KBENCELT 3517","p31A05)1");
    //wifi_command_join_AP("002","zabijemsazalentilku");
    wifi_command_create_TCP_connection("192.168.137.1",6868,Callback,received_message_buffer);

    

    periodic_task_init_a(sendReadingsWrapper,15000);
    periodic_task_init_b(doorApprovalWrapper,30000);
    periodic_task_init_c(breakingInWrapper,1000);

    return 1;
}

