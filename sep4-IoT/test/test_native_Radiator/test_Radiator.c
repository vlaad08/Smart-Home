#include "unity.h"
#include "../fff.h"
#include <string.h>
#include <stdlib.h>

#include "servo.h"
#include"display.h"

#include "RadiatorPosition.h"

DEFINE_FFF_GLOBALS

FAKE_VALUE_FUNC(uint8_t*,setRadiatorLevel,uint8_t,int)

void setUp(void) {}
void tearDown(void) {}

void test_Radiator_Level0(void){
   uint8_t level = 0;
   uint8_t expected_values[] = {0, 1, 0, 0, 0};
   setRadiatorLevel_fake.return_val = expected_values;
   
   uint8_t* result = setRadiatorLevel(level,1);
    
   TEST_ASSERT_EQUAL_UINT8_ARRAY(expected_values, result, 5);
}

void test_Radiator_Level1(void){
   uint8_t level = 1;
   uint8_t expected_values[] = {0, 1, 0, 1, 30}; 
   setRadiatorLevel_fake.return_val = expected_values;
   
   uint8_t* result = setRadiatorLevel(level,1);
    
   TEST_ASSERT_EQUAL_UINT8_ARRAY(expected_values, result, 5);
}

void test_Radiator_Level2(void){
   uint8_t level = 2;
   uint8_t expected_values[] = {0, 1, 0, 2, 60}; 
   setRadiatorLevel_fake.return_val = expected_values;

   uint8_t* result = setRadiatorLevel(level,1);
    
   TEST_ASSERT_EQUAL_UINT8_ARRAY(expected_values, result, 5);
}

void test_Radiator_Level3(void){
   uint8_t level = 3;
   uint8_t expected_values[] = {0, 1, 0, 3, 90}; 
   setRadiatorLevel_fake.return_val = expected_values;
   
   uint8_t* result = setRadiatorLevel(level,1);
    
   TEST_ASSERT_EQUAL_UINT8_ARRAY(expected_values, result, 5);
}

void test_Radiator_Level4(void){
   uint8_t level = 4;
   uint8_t expected_values[] = {0, 1, 0, 4, 120}; 
   setRadiatorLevel_fake.return_val = expected_values;

   uint8_t* result = setRadiatorLevel(level,1);
    
   TEST_ASSERT_EQUAL_UINT8_ARRAY(expected_values, result, 5); 
}

void test_Radiator_Level5(void){
   uint8_t level = 5;
   uint8_t expected_values[] = {0, 1, 0, 5, 150}; 
   setRadiatorLevel_fake.return_val = expected_values;

   uint8_t* result = setRadiatorLevel(level,1);
    
   TEST_ASSERT_EQUAL_UINT8_ARRAY(expected_values, result, 5);
}

void test_Radiator_Level6(void){
   uint8_t level = 6;
   uint8_t expected_values[] = {0, 1, 0, 6, 180}; 
   setRadiatorLevel_fake.return_val = expected_values;
   
   uint8_t* result = setRadiatorLevel(level,1);
    
   TEST_ASSERT_EQUAL_UINT8_ARRAY(expected_values, result, 5);
}

int main() {
   UNITY_BEGIN();

   RUN_TEST(test_Radiator_Level0); 
   RUN_TEST(test_Radiator_Level1); 
   RUN_TEST(test_Radiator_Level2); 
   RUN_TEST(test_Radiator_Level3); 
   RUN_TEST(test_Radiator_Level4); 
   RUN_TEST(test_Radiator_Level5); 
   RUN_TEST(test_Radiator_Level6); 
    
   return UNITY_END();
}