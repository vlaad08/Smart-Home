#include "unity.h"
#include "../fff.h"
#include <string.h>
#include <stdlib.h>

#include "light.h"

#include "LightInfo.h"

DEFINE_FFF_GLOBALS

FAKE_VALUE_FUNC(uint16_t,light_read)

void setUp(void) {}
void tearDown(void) {}

void test_lightLevel_Darkness(void){
    light_read_fake.return_val = 1000; 

    uint8_t * result = getLightInfo();
    
    TEST_ASSERT_NOT_NULL(result);
    TEST_ASSERT_EQUAL_STRING("LIGHT: 1000      ", result);

    free(result);
}

void test_lightLevel_Lightness(void){
    light_read_fake.return_val = 200; 

    uint8_t * result = getLightInfo();
    
    TEST_ASSERT_NOT_NULL(result);
    TEST_ASSERT_EQUAL_STRING("LIGHT: 200      ", result);

    free(result);
}

int main() {
    RESET_FAKE(light_read);

    UNITY_BEGIN();

    RUN_TEST(test_lightLevel_Darkness);
    RUN_TEST(test_lightLevel_Lightness);
    
    return UNITY_END();
}