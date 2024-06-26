#include "Connection.h"

struct AES_ctx my_AES_ctx;
uint8_t key[] = "S3cor3P45Sw0rD@f";

uint8_t* encryptionStart()
{
    AES_init_ctx(&my_AES_ctx,key);
    uint8_t * msg=malloc(30*sizeof(uint8_t));
    sprintf((char *)msg,"Encryption ready to encrypt");
    return msg;
}

uint8_t* connect(WIFI_TCP_Callback_t callback_when_message_received, char *received_message_buffer){    
    //wifi_command_join_AP("Filip's Galaxy S21 FE 5G","jgeb6522");
    wifi_command_join_AP("KBENCELT 3517","p31A05)1");
    //wifi_command_join_AP("002","zabijemsazalentilku");
    wifi_command_create_TCP_connection("172.214.63.209",6868,callback_when_message_received,received_message_buffer);
    uint8_t * msg=malloc(9*sizeof(uint8_t));
    sprintf((char *)msg,"Connected");
    return msg;
}

uint8_t* transmitData(uint8_t * data,uint16_t length){
    AES_ECB_encrypt(&my_AES_ctx,(uint8_t*)data);
    wifi_command_TCP_transmit((uint8_t*)data,length);

    AES_ECB_decrypt(&my_AES_ctx,(uint8_t*)data);
    pc_comm_send_array_blocking((uint8_t*)data,length);

    return data;
}

uint8_t* decryption(uint8_t* data){
    char * msg= malloc(16*sizeof(char));
    memcpy(msg,data,16);
    AES_ECB_decrypt(&my_AES_ctx,(uint8_t *)msg);
    return (uint8_t*) msg;
}