#pragma once

#include <string.h>
#include <stdlib.h>
#include <stdio.h>

#ifdef __AVR__
  #include <util/delay.h> // Include for AVR microcontroller
#else
  #include <unistd.h> // Include for POSIX systems (Linux, macOS, etc.)
#endif

#include "servo.h"
#include "display.h"


int openWindow(int hardwareId);

int closeWindow(int hardwareId);