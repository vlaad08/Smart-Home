#include "unity.h"
#include "../fff.h"

#include "Tasks.h"


DEFINE_FFF_GLOBALS

typedef int (*func_Send)(void);
typedef _Bool (*func_Door)(void);
typedef char* (*func_Security)(void);

FAKE_VOID_FUNC(periodic_task_init_a, func_Send, uint32_t);
FAKE_VOID_FUNC(periodic_task_init_b, func_Door, uint32_t);
FAKE_VOID_FUNC(periodic_task_init_c, func_Security, uint32_t);

void setUp(void) {}
void tearDown(void) {}

int mock_Send(void) {
    return 1;
}

_Bool mock_Door(void) {
    return 1;
}

char* mock_Security(void) {
    return "secure";
}

void test_taskSend()
{
    int result = taskSend(mock_Send);

    TEST_ASSERT_EQUAL_INT(1,result);
}

void test_taskDoor()
{
    int result = taskDoor(mock_Door);

    TEST_ASSERT_EQUAL_INT(1,result);
}

void test_taskSecurity()
{
    int result = taskSecurity(mock_Security);

    TEST_ASSERT_EQUAL_INT(1,result);
}

int main(){
    UNITY_BEGIN();

    RUN_TEST(test_taskSend);
    RUN_TEST(test_taskDoor);
    RUN_TEST(test_taskSecurity);

    return UNITY_END();
}