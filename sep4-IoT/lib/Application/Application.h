#pragma once
#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include <stdbool.h>

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


void setup();

void callback();

void breakingIn();

void doorAction(uint8_t status);

void doorApproval();

void sendReadings();

void sendLight();

void sendTempAndHumidity();

void transmitData(uint8_t * data,uint16_t length);