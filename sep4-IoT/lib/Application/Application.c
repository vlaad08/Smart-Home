#include "Application.h"

Enc enc;
uint8_t iv[16];
struct AES_ctx my_AES_ctx;
bool IsPKAcquired=false;
bool UnlockingApproved=false;
char received_message_buffer[128];

void transmitData(uint8_t * data,uint16_t length){
    //AES_ECB_encrypt(&my_AES_ctx,(uint8_t*)data);
    wifi_command_TCP_transmit((uint8_t*)data,length);

    //AES_ECB_decrypt(&my_AES_ctx,(uint8_t*)data);
    pc_comm_send_array_blocking((uint8_t*)data,length);

    free(data);
}

void sendTempAndHumidity(){
    uint8_t *data = getTempAndHum(); 
    transmitData(data, 16);
}

void sendLight(){
    uint8_t *data = getLightInfo(); 
    transmitData(data,16);
}

void sendReadings()
{
    sendTempAndHumidity();
    _delay_ms(1000);
    sendLight();
}

void windowAction(uint8_t status){
    //to indicate that we are moving the window with a servo we are going all the way up, all the way down 
    //then going to the middle and waits for a second and then if open then going up is closed going down
    if (status)
        openWindow();
    else
        closeWindow();
}


void doorApproval(){
    if (UnlockingApproved)
        UnlockingApproved = false;
}

void doorAction(uint8_t status){
     if (status){
        UnlockingApproved = true;
        openDoor();
    }
    else{
        closeDoor();
        UnlockingApproved=false;
    } 
}


void breakingIn(){
    char * x = alarm(UnlockingApproved);
    if (strcmp(x,"Hello, Thief! :)")==0)
    {
        transmitData((uint8_t*)x,16);
    }
}

void Callback(){
    if (!IsPKAcquired)
    {
        char token[65];
        strncpy(token, received_message_buffer, 64);
        token[64] = '\0';
        createSharedKey(&enc,token);
        uint8_t * sharedkey=getSharedKey(&enc);
        AES_init_ctx_iv(&my_AES_ctx,sharedkey,iv);
        wifi_command_TCP_transmit((uint8_t*)"Shared Key Created", 19);
        IsPKAcquired=true;
    }
    else{
        uint8_t value;
        switch (received_message_buffer[1])
        {
        case '1':
            value = received_message_buffer[3] - '0';
            uint8_t * radiator= setRadiatorLevel(value);
            free(radiator);
            break;
        case '2':
            value = received_message_buffer[3] - '0';
            windowAction(value);
            break;
        case '3':
            value = received_message_buffer[3] - '0';
            doorAction(value);
            break;
        case '4':
            value = received_message_buffer[3] - '0';
            char * light= AdjustLight(value);
            free(light);
            break;
        default:
            break;
        }
    }
}

void start(){
    pc_comm_init(9600,NULL);
    wifi_init();
    dht11_init();
    light_init();
    display_init();
    leds_init();
    hc_sr04_init();

    createIOTKeys(&enc);
    generate_iv(iv,16);
    
    //wifi_command_join_AP("Filip's Galaxy S21 FE 5G","jgeb6522");
    wifi_command_join_AP("KBENCELT 3517","p31A05)1");
    //wifi_command_join_AP("002","zabijemsazalentilku");
    wifi_command_create_TCP_connection("192.168.137.1",6868,Callback,received_message_buffer);

    char* public_key_hex = print_hex(getIOTPublicKey(&enc), 64);
    char* connection = (char*)malloc((sizeof("Connected:") + strlen(public_key_hex) + 1) * sizeof(char));
    sprintf(connection, "Connected:%s", public_key_hex);
    wifi_command_TCP_transmit((uint8_t*)connection,strlen(connection));
    free(connection);

    _delay_ms(1000);

    periodic_task_init_a(sendReadings,30000);
    periodic_task_init_b(doorApproval,30000);
    periodic_task_init_c(breakingIn,1000);
}