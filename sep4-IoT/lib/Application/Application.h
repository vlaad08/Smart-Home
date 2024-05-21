#pragma once
#include <stdlib.h>
#include <string.h>
#include <stdio.h>

#include "wifi.h"
#include "pc_comm.h"
#include "periodic_task.h"

#include "uECC.h"
#include "Enc.h"
#include "aes.h"

#include "TempAndHum.h"
#include "LightInfo.h"
#include "AdjustLight.h"
#include "RadiatorPosition.h"
#include "Window.h"
#include "AlarmDoor.h"
#include "Door.h"
#include "Converter.h"

#ifdef __AVR__
  #include <util/delay.h>
#else
  #include <unistd.h>
#endif

extern bool UnlockingApproved;

int start();

void Callback();

char * breakingIn();

bool doorAction(uint8_t status);

bool doorApproval();

int sendReadings();

int sendLight();

int sendTempAndHumidity();

void transmitData(uint8_t * data,uint16_t length);

void sendReadingWrapper();