#include "Application.h"

void static custom_delay_ms(uint16_t milliseconds) {
#ifdef __AVR__
    _delay_ms(milliseconds); // Use _delay_ms() for AVR microcontroller
#else
    usleep(milliseconds * 1000); // Use usleep() for POSIX systems (convert milliseconds to microseconds)
#endif
}

Enc enc;
uint8_t iv[16];
struct AES_ctx my_AES_ctx;
bool IsPKAcquired=true;
bool UnlockingApproved=false;
char received_message_buffer[128];

void transmitData(uint8_t * data,uint16_t length){
    AES_ECB_encrypt(&my_AES_ctx,(uint8_t*)data);
    wifi_command_TCP_transmit((uint8_t*)data,length);

    AES_ECB_decrypt(&my_AES_ctx,(uint8_t*)data);
    pc_comm_send_array_blocking((uint8_t*)data,length);

    free(data);
}

int sendTempAndHumidity(){
    uint8_t *data = getTempAndHum(); 
    transmitData(data, 16);

    return 1;
}

int sendLight(){
    uint8_t *data = getLightInfo(); 
    transmitData(data,16);

    return 1;
}

int sendReadings()
{
    sendTempAndHumidity();
    custom_delay_ms(1000);
    sendLight();

    return 1;
}

void windowAction(uint8_t status){
    //to indicate that we are moving the window with a servo we are going all the way up, all the way down 
    //then going to the middle and waits for a second and then if open then going up is closed going down
    if (status)
        openWindow();
    else
        closeWindow();
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
    if (!IsPKAcquired)
    {
        char token[65];
        strncpy(token, received_message_buffer, 64);
        token[64] = '\0';
        //createSharedKey(&enc,token);
        //uint8_t * sharedkey=getSharedKey(&enc);
        // uint8_t * sharedkey=(uint8_t *) calloc(33,sizeof(uint8_t));
        // snprintf((char *)sharedkey, 33, "RaT‰ëòçÇRQqBèºQ|{ŽnÎA");
        // AES_init_ctx_iv(&my_AES_ctx,sharedkey,iv);
        //wifi_command_TCP_transmit((uint8_t*)sharedkey, 32);
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

int start(){
    pc_comm_init(9600,NULL);
    wifi_init();
    dht11_init();
    light_init();
    display_init();
    leds_init();
    hc_sr04_init();

    //createIOTKeys(&enc);
    //generate_iv(iv,16);
    
    //wifi_command_join_AP("Filip's Galaxy S21 FE 5G","jgeb6522");
    wifi_command_join_AP("KBENCELT 3517","p31A05)1");
    //wifi_command_join_AP("002","zabijemsazalentilku");
    wifi_command_create_TCP_connection("192.168.137.14",6868,Callback,received_message_buffer);

    //uint8_t * PK=getIOTPublicKey(&enc);
    //char* public_key_hex = print_hex(PK, 64);
    char* connection = (char*)malloc((sizeof("Connected:") /*+ strlen(public_key_hex)*/ + 1) * sizeof(char)); 
    sprintf(connection, "Connected:");
 
    wifi_command_TCP_transmit((uint8_t*)connection,strlen(connection));
    
    free(connection);
    //free(public_key_hex);

    const char * encoded_data = "qKBL+IAOLbn+jLnFJEYp8KAmlAe4iVQVfa2K4d9huA4=";
    unsigned char *decoded_data;
    size_t decoded_len;

    // Calculate the length of the decoded data
    decoded_len = b64_decoded_size((const char *)encoded_data);
    decoded_data = malloc(decoded_len);

    if (!b64_decode(encoded_data, decoded_data, decoded_len)) {
        free(decoded_data);
        transmitData("-2",2);
    }

    if (decoded_len > sizeof(enc.SharedKey)) {     
        free(decoded_data);
        transmitData("-2",2);
    }
    memcpy(enc.SharedKey, decoded_data, decoded_len);

    free(decoded_data);
    
    // const char * encoded_IV = "cRooWgwV4QTvQxZkqOZRHw==";
    // unsigned char *decoded_IV;
    // size_t decoded_IV_len;
// 
    //Calculate the length of the decoded data
    // decoded_len = b64_decoded_size((const char *)encoded_IV);
    // decoded_data = malloc(decoded_IV_len);
// 
    // if (!b64_decode(encoded_IV, decoded_IV, decoded_IV_len)) {
        // free(decoded_IV);
        // return -2;
    // }
// 
    // if (decoded_IV_len > sizeof(iv)) {     
        // free(decoded_IV);
        // return -2;
    // }
    // memcpy(iv, decoded_IV, decoded_IV_len);
// 
    // free(decoded_IV);



void sendReadingsWrapper(void) {
    (void)sendReadings(); // Call sendReadings and ignore the return value
}

    AES_init_ctx_iv(&my_AES_ctx,enc.SharedKey,iv);

    custom_delay_ms(1000);

    periodic_task_init_a(sendReadingsWrapper,15000);
    //periodic_task_init_b(doorApproval,30000);
    //periodic_task_init_c(breakingIn,1000);

    return 1;
}