#ifndef WIN_TEST

#include "Enc.h"
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>


void createIOTKeys(Enc * self){
    uECC_set_rng(simple_rng);
    self->curve=uECC_secp256r1();
    uECC_make_key(self->IOTPublicKey,self->IOTPrivateKey,self->curve);
}

void createSharedKey(Enc * self, char* CloudPublicKey){
    uECC_shared_secret((uint8_t *)CloudPublicKey,self->IOTPrivateKey,self->SharedKey,self->curve);
}

uint8_t * getSharedKey(Enc * self){
    return self->SharedKey;
}

uint8_t * getIOTPublicKey(Enc *self){
    return self->IOTPublicKey;
}



#define IV_SIZE 16 

void generate_iv(uint8_t *iv, size_t iv_size) {
    size_t i;
    for (i = 0; i < iv_size; ++i) {
        iv[i] = rand() & 0xFF;
    }
}


int simple_rng(uint8_t *dest, unsigned size){
    static uint8_t value = 0;
    value++;
    for (unsigned i = 0; i < size; ++i)
    {
        dest[i] = value + i + 7;
    }
    return 1;
}

char* print_hex(uint8_t *buf, size_t len) {
    char *result = (char *)malloc((2 * len + 1) * sizeof(char));
    
    size_t index = 0;
    for (size_t i = 0; i < len; i++) {
        index += sprintf(result + index, "%02X", buf[i]); 
    }
    return result;
}

#endif