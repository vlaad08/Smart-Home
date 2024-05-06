#pragma once
#include <stdio.h>
#include "uECC.h"

typedef struct Enc
{
    uint8_t IOTPublicKey[64];
    uint8_t IOTPrivateKey[32];
    uint8_t SharedKey[32];
    uECC_Curve curve;
} Enc;



void createIOTKeys(Enc * self);

int simple_rng(uint8_t *dest, unsigned size);

void createSharedKey(Enc * self, uint8_t CloudPublicKey);

uint8_t * getSharedKey(Enc * self);

uint8_t getIOTPublicKey(Enc *self);

void generate_iv(uint8_t *iv, size_t iv_size);

char* print_hex(uint8_t *buf, size_t len);


