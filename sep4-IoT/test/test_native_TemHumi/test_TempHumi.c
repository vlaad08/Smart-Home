#include "unity.h"
#include "../fff.h"
#include <string.h>
#include <stdlib.h>

#include "dht11.h"

#include "TempAndHum.h"

DEFINE_FFF_GLOBALS

FAKE_VALUE_FUNC(DHT11_ERROR_MESSAGE_t, dht11_get, uint8_t*, uint8_t*, uint8_t*, uint8_t*);

void setUp(void) {}
void tearDown(void) {}

void test_getTemptAndHum_Success(void) {
    dht11_get_fake.return_val = DHT11_OK;

    char* result = getTempAndHum();

    TEST_ASSERT_NOT_NULL(result);
    TEST_ASSERT_EQUAL_STRING("T:0.0   H:0.0 ", result);

    free(result);
}

void test_getTemptAndHum_Error(void) {
    dht11_get_fake.return_val = DHT11_FAIL;

    char* result = getTempAndHum();

    TEST_ASSERT_NOT_NULL(result);
    TEST_ASSERT_EQUAL_STRING("Temp Hum Error", result);
    
    free(result);
}


int main() {
    RESET_FAKE(dht11_get);

    UNITY_BEGIN();

    RUN_TEST(test_getTemptAndHum_Success);
    RUN_TEST(test_getTemptAndHum_Error);
    
    return UNITY_END();
}