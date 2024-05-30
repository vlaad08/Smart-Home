#pragma once

#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdbool.h>

#include "periodic_task.h"

#include "aes.h"

int taskSend(int (*function)(void));

int taskDoor(_Bool (*function)(void));

int taskSecurity(char* (*function)(void));