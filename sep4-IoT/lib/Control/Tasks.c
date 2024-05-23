#include "Tasks.h"


int taskSend(int (*function)(void)){
    periodic_task_init_a(function,5000);
    return 1;
}

int taskDoor(_Bool (*function)(void)){
    periodic_task_init_b(function,30000);
    return 1;
}

int taskSecurity(char* (*function)(void)){
    periodic_task_init_c(function,1000);
    return 1;
}