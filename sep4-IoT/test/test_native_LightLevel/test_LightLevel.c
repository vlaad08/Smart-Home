#include "unity.h"
#include "../fff.h"
#include <string.h>
#include <stdlib.h>

#include "AdjustLight.h"

DEFINE_FFF_GLOBALS

FAKE_VOID_FUNC(leds_turnOff,uint8_t);
FAKE_VOID_FUNC(leds_turnOn,uint8_t);
FAKE_VOID_FUNC(display_setValues,uint8_t,uint8_t,uint8_t,uint8_t);

void setUp(void) {}
void tearDown(void) {}

void test_lightLevel_0(void){
    char * result = AdjustLight((uint8_t)0,1);
    
    TEST_ASSERT_NOT_NULL(result);
    TEST_ASSERT_EQUAL_STRING("0,4,0,0", result);

    free(result);
}

void test_lightLevel_1(void){
    char * result = AdjustLight((uint8_t)1,1);
    
    TEST_ASSERT_NOT_NULL(result);
    TEST_ASSERT_EQUAL_STRING("0,4,0,1", result);

    free(result);
}

void test_lightLevel_2(void){
    char * result = AdjustLight((uint8_t)2,1);
    
    TEST_ASSERT_NOT_NULL(result);
    TEST_ASSERT_EQUAL_STRING("0,4,0,2", result);

    free(result);
}

void test_lightLevel_3(void){
    char * result = AdjustLight((uint8_t)3,1);
    
    TEST_ASSERT_NOT_NULL(result);
    TEST_ASSERT_EQUAL_STRING("0,4,0,3", result);

    free(result);
}

void test_lightLevel_4(void){
    char * result = AdjustLight((uint8_t)4,1);
    
    TEST_ASSERT_NOT_NULL(result);
    TEST_ASSERT_EQUAL_STRING("0,4,0,4", result);

    free(result);
}

int main() {
    UNITY_BEGIN();

    RUN_TEST(test_lightLevel_0);
    RUN_TEST(test_lightLevel_1);
    RUN_TEST(test_lightLevel_2);
    RUN_TEST(test_lightLevel_3);
    RUN_TEST(test_lightLevel_4);

    return UNITY_END();
}