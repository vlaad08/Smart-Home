#include "Connection.h"

struct AES_ctx my_AES_ctx;
uint8_t key[] = "S3cor3P45Sw0rD@f";

uint8_t* encryptionStart()
{
    AES_init_ctx(&my_AES_ctx,key);
    return (uint8_t*) "Encription ready to encrypt";
}

uint8_t* connect(WIFI_TCP_Callback_t callback_when_message_received, char *received_message_buffer){
        
    //wifi_command_join_AP("Filip's Galaxy S21 FE 5G","jgeb6522");
   // wifi_command_join_AP("KBENCELT 3517","p31A05)1");
    wifi_command_join_AP("002","zabijemsazalentilku");
    wifi_command_create_TCP_connection("192.168.236.1",6868,callback_when_message_received,received_message_buffer);
    return (uint8_t*)"Connected";
}

uint8_t* transmitData(uint8_t * data,uint16_t length){
    AES_ECB_encrypt(&my_AES_ctx,(uint8_t*)data);
    wifi_command_TCP_transmit((uint8_t*)data,length);

    AES_ECB_decrypt(&my_AES_ctx,(uint8_t*)data);
    pc_comm_send_array_blocking((uint8_t*)data,length);

    return data;
}

uint8_t* decryption(uint8_t* data){
   // pc_comm_send_array_blocking((uint8_t*)data,128);
    char * msg= malloc(16*sizeof(char));
    memcpy(msg,data,16);
    AES_ECB_decrypt(&my_AES_ctx,(uint8_t *)msg);
    return (uint8_t*) msg;

/*uint8_t* decrytpion(uint8_t* data){
    AES_ECB_decrypt(&my_AES_ctx,(uint8_t*)data);
    return data;*/
}