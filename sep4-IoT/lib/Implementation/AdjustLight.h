#pragma once
#include <string.h>
#include "stdlib.h"
#include <stdio.h>
#include "leds.h"
#include "pc_comm.h"
#ifdef __AVR__
#include <util/delay.h> // Include for AVR microcontroller
#else
#include <unistd.h> // Include for POSIX systems (Linux, macOS, etc.)
#endif
#include "display.h"



void AdjustLight(uint8_t * level);