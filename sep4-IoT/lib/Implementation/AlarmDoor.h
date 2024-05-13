#pragma once
#include <string.h>
#include "stdlib.h"
#include <stdio.h>
#include <stdbool.h>
#include "buzzer.h"
#include "display.h"
#include "hc_sr04.h"
#include "periodic_task.h"


char* alarm(bool isApproved);