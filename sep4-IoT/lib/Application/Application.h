#pragma once
#include <stdlib.h>
#include <string.h>
#include <stdio.h>

#include "TempAndHum.h"
#include "LightInfo.h"
#include "AdjustLight.h"
#include "RadiatorPosition.h"
#include "Window.h"
#include "AlarmDoor.h"
#include "Door.h"
#include "Connection.h"
#include "Tasks.h"

#ifdef __AVR__
  #include <util/delay.h>
#else
  #include <unistd.h>
#endif

extern _Bool UnlockingApproved;
extern char received_message_buffer[128];

int start();

int Callback();

char* breakingIn();

bool doorAction(uint8_t status);

_Bool doorApproval();

int windowAction(uint8_t status,int hardwareId);

int sendReadings();

int sendLight(int hardwareId);

int sendTempAndHumidity(int hardwareid);