#include "unity.h"
#include "../fff.h"
#include <string.h>
#include <stdlib.h>

#include "servo.h"
#include "display.h"

#include "Window.h"

DEFINE_FFF_GLOBALS

FAKE_VOID_FUNC(servo,uint8_t);
FAKE_VOID_FUNC(display_setValues, uint8_t, uint8_t, uint8_t, uint8_t);

void setUp(void) {}
void tearDown(void) {}

void test_openWindow(void){
    int result=openWindow();
    
    TEST_ASSERT_EQUAL_UINT8(1,result);
}

void test_closeWindow(void){
    int result=closeWindow();

    TEST_ASSERT_EQUAL_UINT8(0,result);
}

int main(){
    UNITY_BEGIN();

    RUN_TEST(test_openWindow);
    RUN_TEST(test_closeWindow);

    return UNITY_END();
}