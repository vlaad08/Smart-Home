#include "unity.h"
#include "../fff.h"

#include "Application.h"


DEFINE_FFF_GLOBALS

//start
FAKE_VOID_FUNC(pc_comm_init,uint32_t,pc_comm_callback_t);
FAKE_VOID_FUNC(wifi_init);
FAKE_VOID_FUNC(dht11_init);
FAKE_VOID_FUNC(light_init);
FAKE_VOID_FUNC(display_init);
FAKE_VOID_FUNC(leds_init);
FAKE_VOID_FUNC(hc_sr04_init);

//FAKE_VALUE_FUNC(uint8_t*, encriptionStart);
//FAKE_VALUE_FUNC(uint8_t*, connect,WIFI_TCP_Callback_t,char*);

typedef int (*send_t)(void);
FAKE_VALUE_FUNC(int, taskSend,send_t);
typedef _Bool (*door_t)(void);
FAKE_VALUE_FUNC(int, taskDoor,door_t);
typedef char* (*security_t)(void);
FAKE_VALUE_FUNC(int, taskSecurity,security_t);

//Callback
FAKE_VALUE_FUNC(uint8_t*, setRadiatorLevel, uint8_t, int);
FAKE_VALUE_FUNC(char*, AdjustLight,uint8_t,int)

//breakingIn
FAKE_VALUE_FUNC(char *,alarm,bool);
//FAKE_VALUE_FUNC(uint8_t*, transmitData,uint8_t*, uint16_t);

//doorAction
FAKE_VALUE_FUNC(int,openDoor);
FAKE_VALUE_FUNC(int,closeDoor);

//windowAction
FAKE_VALUE_FUNC(int,openWindow,int);
FAKE_VALUE_FUNC(int,closeWindow,int);

//sendLight
FAKE_VALUE_FUNC(uint8_t *,getLightInfo,uint8_t);

//sendTempAndHumidity
FAKE_VALUE_FUNC(uint8_t *,getTempAndHum,uint8_t);

//For adjust light idk why
FAKE_VOID_FUNC(leds_turnOff,uint8_t);
FAKE_VOID_FUNC(leds_turnOn,uint8_t);
FAKE_VOID_FUNC(display_setValues,uint8_t,uint8_t,uint8_t,uint8_t);
FAKE_VOID_FUNC(servo,uint8_t);

FAKE_VALUE_FUNC(WIFI_ERROR_MESSAGE_t,wifi_command_join_AP,char *,char *);//2
FAKE_VALUE_FUNC(WIFI_ERROR_MESSAGE_t,wifi_command_create_TCP_connection,char *,uint16_t,WIFI_TCP_Callback_t, char *);//3
FAKE_VALUE_FUNC(WIFI_ERROR_MESSAGE_t,wifi_command_TCP_transmit,uint8_t *,uint16_t);//5

FAKE_VOID_FUNC(pc_comm_send_array_blocking,uint8_t *,uint16_t );//7

FAKE_VOID_FUNC(AES_init_ctx,struct AES_ctx *,const uint8_t *);//1
FAKE_VOID_FUNC(AES_ECB_encrypt,const struct AES_ctx *,uint8_t *);//4
FAKE_VOID_FUNC(AES_ECB_decrypt,const struct AES_ctx *,uint8_t *);//6


void setUp(void) {
    RESET_FAKE(pc_comm_init);
    RESET_FAKE(wifi_init);
    RESET_FAKE(alarm);
    // Reset other fakes as needed
    FFF_RESET_HISTORY();
    UnlockingApproved=false;
    sprintf(received_message_buffer,"0000");
}
void tearDown(void) {}

void test_start(void){
    //connect_fake.return_val = WIFI_OK;
    int result=start();

    TEST_ASSERT_EQUAL_INT(1,result);
}

void test_CallbackDoor(void){
    sprintf(received_message_buffer,"0301");
    int result = Callback();

    TEST_ASSERT_EQUAL_INT(3,result);
}

void test_CallbackRadiatorValidRoom(void){
    sprintf(received_message_buffer,"1101");
    int result = Callback();

    TEST_ASSERT_EQUAL_INT(1,result);
}

void test_CallbackWindowsValidRoom(void){
    sprintf(received_message_buffer,"1201");
    int result = Callback();

    TEST_ASSERT_EQUAL_INT(2,result);
}

void test_CallbackLightsValidRoom(void){
    sprintf(received_message_buffer,"1401");
    int result = Callback();

    TEST_ASSERT_EQUAL_INT(4,result);
}

void test_CallbackRadiatorInvalidRoom(void){
    sprintf(received_message_buffer,"0101");
    int result = Callback();

    TEST_ASSERT_EQUAL_INT(0,result);
}

void test_CallbackWindowsinvalidRoom(void){
    sprintf(received_message_buffer,"0201");
    int result = Callback();

    TEST_ASSERT_EQUAL_INT(0,result);
}

void test_CallbackLightsInvalidRoom(void){
    sprintf(received_message_buffer,"0401");
    int result = Callback();

    TEST_ASSERT_EQUAL_INT(0,result);
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
    int result=sendTempAndHumidity(1);

    TEST_ASSERT_EQUAL_INT(1,result);
}

void test_sendLight(){
    int result=sendLight(1);

    TEST_ASSERT_EQUAL_INT(1,result);
}

void test_sendReadings(){
    int result=sendReadings(1);

    TEST_ASSERT_EQUAL_INT(1,result);
}

int main(){
    UNITY_BEGIN();

    RUN_TEST(test_start);

    RUN_TEST(test_CallbackDoor);
    RUN_TEST(test_CallbackRadiatorValidRoom);
    RUN_TEST(test_CallbackWindowsValidRoom);
    RUN_TEST(test_CallbackLightsValidRoom);
    RUN_TEST(test_CallbackRadiatorInvalidRoom);
    RUN_TEST(test_CallbackWindowsinvalidRoom);
    RUN_TEST(test_CallbackLightsInvalidRoom);

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