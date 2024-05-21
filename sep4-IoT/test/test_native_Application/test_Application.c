#include "unity.h"
#include "../fff.h"

#include "Application.h"


DEFINE_FFF_GLOBALS

FAKE_VOID_FUNC(pc_comm_init,uint32_t,pc_comm_callback_t);
FAKE_VOID_FUNC(wifi_init);
FAKE_VOID_FUNC(dht11_init);
FAKE_VOID_FUNC(light_init);
FAKE_VOID_FUNC(display_init);
FAKE_VOID_FUNC(leds_init);
FAKE_VOID_FUNC(hc_sr04_init);
FAKE_VOID_FUNC(createIOTKeys,Enc *);
FAKE_VOID_FUNC(generate_iv, uint8_t *,size_t);

FAKE_VALUE_FUNC(WIFI_ERROR_MESSAGE_t,wifi_command_join_AP,char *,char *);
FAKE_VALUE_FUNC(WIFI_ERROR_MESSAGE_t,wifi_command_create_TCP_connection,char *,uint16_t,WIFI_TCP_Callback_t, char *);
FAKE_VALUE_FUNC(WIFI_ERROR_MESSAGE_t,wifi_command_TCP_transmit,uint8_t *,uint16_t);

FAKE_VALUE_FUNC(char *,print_hex,uint8_t *,size_t);
FAKE_VALUE_FUNC(uint8_t *,getIOTPublicKey,Enc *);





FAKE_VALUE_FUNC(char *,alarm,bool);
FAKE_VALUE_FUNC(uint8_t *,getTempAndHum);
FAKE_VALUE_FUNC(uint8_t *,getLightInfo);
FAKE_VALUE_FUNC(int,openWindow);
FAKE_VALUE_FUNC(int,closeWindow);
FAKE_VALUE_FUNC(int,openDoor);
FAKE_VALUE_FUNC(int,closeDoor);
FAKE_VOID_FUNC(pc_comm_send_array_blocking,uint8_t *,uint16_t );

FAKE_VOID_FUNC(createSharedKey,Enc *,char *);
FAKE_VALUE_FUNC(uint8_t *, getSharedKey,Enc *);
FAKE_VOID_FUNC(AES_init_ctx_iv,struct AES_ctx *,const uint8_t *,const uint8_t *);
FAKE_VOID_FUNC(AES_ECB_encrypt,const struct AES_ctx *,uint8_t *);
FAKE_VOID_FUNC(AES_ECB_decrypt,const struct AES_ctx *,uint8_t *);

FAKE_VOID_FUNC(leds_turnOff,uint8_t);
FAKE_VOID_FUNC(leds_turnOn,uint8_t);
FAKE_VOID_FUNC(display_setValues,uint8_t,uint8_t,uint8_t,uint8_t);
FAKE_VOID_FUNC(servo,uint8_t);




void setUp(void) {
    RESET_FAKE(AES_init_ctx_iv);
    RESET_FAKE(pc_comm_init);
    RESET_FAKE(wifi_init);
    RESET_FAKE(alarm);
    // Reset other fakes as needed
    FFF_RESET_HISTORY();
    UnlockingApproved=false;
}
void tearDown(void) {}

void test_start(void){
    getIOTPublicKey_fake.return_val=(uint8_t *)"0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF";
    print_hex_fake.return_val="30313233343536373839414243444546303132333435363738394142434445463031323334353637383941424344454630313233343536373839414243444546";
    wifi_command_TCP_transmit_fake.return_val=WIFI_OK;
    int result=start();

    TEST_ASSERT_EQUAL_INT(1,result);
}

void test_doorApproval(void){
    bool result=doorApproval();

    TEST_ASSERT_FALSE(result);
    
}

void test_doorActionOpen(void){
    bool result=doorAction(1);

    TEST_ASSERT_TRUE(result);
}

void test_doorActionClose(void){
    bool result=doorAction(0);

    TEST_ASSERT_FALSE(result);
}

void test_breakingInTrue(void){
    UnlockingApproved=false;
    alarm_fake.return_val = "Hello, Thief!";
    char * result=breakingIn();

    TEST_ASSERT_EQUAL_CHAR_ARRAY("Hello, Thief!",result,12);
    //TEST_ASSERT_EQUAL_STRING("Hello, Thief! :)", result);
}

void test_breakingInFalse(void){
    UnlockingApproved=true;
    alarm_fake.return_val = "Approved doors! ";

    char * result=breakingIn();

    //TEST_ASSERT_EQUAL_CHAR_ARRAY("Approved doors! ",result,17);
    TEST_ASSERT_EQUAL_STRING("Approved doors! ", result);
}

void test_sendTempAndHumidity(){

    int result=sendTempAndHumidity();

    TEST_ASSERT_EQUAL_INT(1,result);
}

void test_sendLight(){
    int result=sendLight();

    TEST_ASSERT_EQUAL_INT(1,result);
}

void test_sendReadings(){
    int result=sendReadings();

    TEST_ASSERT_EQUAL_INT(1,result);
}

int main(){
    UNITY_BEGIN();

    RUN_TEST(test_start);

    RUN_TEST(test_doorActionClose);
    RUN_TEST(test_doorActionOpen);
    RUN_TEST(test_doorApproval);

    RUN_TEST(test_breakingInFalse);
    RUN_TEST(test_breakingInTrue);

    RUN_TEST(test_sendLight);
    RUN_TEST(test_sendReadings);
    RUN_TEST(test_sendTempAndHumidity);

    return UNITY_END();
}