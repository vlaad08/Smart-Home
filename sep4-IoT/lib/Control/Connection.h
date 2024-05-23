#pragma once

#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdbool.h>

#include "wifi.h"
#include "pc_comm.h"

#include "aes.h"

uint8_t* encriptionStart();

uint8_t* connect(WIFI_TCP_Callback_t callback_when_message_received, char *received_message_buffer);

uint8_t* transmitData(uint8_t * data,uint16_t length);

uint8_t* decrytpion();