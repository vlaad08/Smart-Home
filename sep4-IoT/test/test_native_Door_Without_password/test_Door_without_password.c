#include "AlarmDoor.h"
#include "unity.h"
#include "../fff.h"
#include <stdbool.h>
#include <string.h>
#include <stdlib.h>
#include "buzzer.h"

DEFINE_FFF_GLOBALS

FAKE_VALUE_FUNC(uint16_t, hc_sr04_takeMeasurement);
FAKE_VOID_FUNC(buzzer_beep);

void setUp(void) {
    RESET_FAKE(hc_sr04_takeMeasurement);
}

void tearDown(void) { 
}


void fake_sendMessage(const char* message) {
    // Do nothing, just a placeholder for testing
}

void test_openDoorWithoutApproval(void) {
 
    hc_sr04_takeMeasurement_fake.return_val = 40;
    bool isApproved = false;
    char* alarm_message = alarm(isApproved);
    TEST_ASSERT_NOT_NULL(alarm_message);
    TEST_ASSERT_EQUAL_STRING("SOMEONE IS TRYING TO BREAK INTO THE HOUSE!!!", alarm_message);
    free(alarm_message);
}
void test_openDoorWithApproval(void) {
    // Set up fake behavior of hc_sr04_takeMeasurement()
    hc_sr04_takeMeasurement_fake.return_val = 40;
    
    // Call the function under test with approval
    bool isApproved = true;
    char* alarm_message = alarm(isApproved);
    
    // Assert that the returned message is NULL since the door opening is approved
    TEST_ASSERT_NULL(alarm_message);
}

int main() {
    UNITY_BEGIN();

    RUN_TEST(test_openDoorWithoutApproval);
RUN_TEST(test_openDoorWithApproval);
    return UNITY_END();
}
