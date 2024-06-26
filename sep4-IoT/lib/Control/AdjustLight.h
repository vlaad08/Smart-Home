#pragma once

#include <string.h>
#include <stdlib.h>
#include <stdio.h>

#ifdef __AVR__
  #include <util/delay.h>
#else
  #include <unistd.h>
#endif

#include "leds.h"
#include "display.h"

char* AdjustLight(uint8_t level,int hardwareId);