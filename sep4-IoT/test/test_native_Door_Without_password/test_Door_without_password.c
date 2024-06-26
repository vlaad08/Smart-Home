#include "unity.h"
#include "../fff.h"
#include <stdbool.h>
#include <string.h>
#include <stdlib.h>

#include "buzzer.h"

#include "AlarmDoor.h"

DEFINE_FFF_GLOBALS

FAKE_VALUE_FUNC(uint16_t, hc_sr04_takeMeasurement);
FAKE_VOID_FUNC(display_setValues,uint8_t,uint8_t,uint8_t,uint8_t);
FAKE_VOID_FUNC(buzzer_beep);

void setUp(void) {
    RESET_FAKE(hc_sr04_takeMeasurement);
}

void tearDown(void) { 
}

void test_openDoorWithoutApproval(void) {
    hc_sr04_takeMeasurement_fake.return_val = 40;

    bool isApproved = false;
    char* alarm_message = Alarm(isApproved);

    TEST_ASSERT_NOT_NULL(alarm_message);
    TEST_ASSERT_EQUAL_STRING("1-Hello, Thief! ", alarm_message);

    free(alarm_message);
}
void test_openDoorWithApproval(void) {
    hc_sr04_takeMeasurement_fake.return_val = 40;
    
    bool isApproved = true;
    char* alarm_message = Alarm(isApproved);

    TEST_ASSERT_NOT_NULL(alarm_message);
    TEST_ASSERT_EQUAL_STRING("Approved doors! ", alarm_message);

    free(alarm_message);
}

int main() {
    UNITY_BEGIN();

    RUN_TEST(test_openDoorWithoutApproval);
    RUN_TEST(test_openDoorWithApproval);
    
    return UNITY_END();
}
