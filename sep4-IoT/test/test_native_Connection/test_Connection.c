#include "unity.h"
#include "../fff.h"

#include "Connection.h"


DEFINE_FFF_GLOBALS

FAKE_VALUE_FUNC(WIFI_ERROR_MESSAGE_t,wifi_command_join_AP,char *,char *);//2
FAKE_VALUE_FUNC(WIFI_ERROR_MESSAGE_t,wifi_command_create_TCP_connection,char *,uint16_t,WIFI_TCP_Callback_t, char *);//3
FAKE_VALUE_FUNC(WIFI_ERROR_MESSAGE_t,wifi_command_TCP_transmit,uint8_t *,uint16_t);//5

FAKE_VOID_FUNC(pc_comm_send_array_blocking,uint8_t *,uint16_t );//7

FAKE_VOID_FUNC(AES_init_ctx,struct AES_ctx *,const uint8_t *);//1
FAKE_VOID_FUNC(AES_ECB_encrypt,const struct AES_ctx *,uint8_t *);//4
FAKE_VOID_FUNC(AES_ECB_decrypt,const struct AES_ctx *,uint8_t *);//6

void setUp(void) {}
void tearDown(void) {}

void test_encryptionStart(){
    uint8_t* result = encryptionStart();

    TEST_ASSERT_EQUAL_STRING("Encryption ready to encrypt",result);
}

int testCallback()
{
    return 1;
}

void test_connect(){
    char* x;
    uint8_t* result = connect(testCallback,x);

    TEST_ASSERT_EQUAL_STRING("Connected",result);
}

void test_transmitData(){
    uint8_t* result = transmitData("test",4);

    TEST_ASSERT_EQUAL_STRING("test",result);
}

void test_decryption(){
    uint8_t* result = decryption("test",4);

    TEST_ASSERT_EQUAL_STRING("test",result);
}

int main(void){
    UNITY_BEGIN();

    RUN_TEST(test_encryptionStart);
    RUN_TEST(test_connect);
    RUN_TEST(test_transmitData);
    RUN_TEST(test_decryption);

    return UNITY_END();
}