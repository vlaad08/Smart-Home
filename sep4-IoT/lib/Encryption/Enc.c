#include "Enc.h"
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>


void createIOTKeys(Enc * self){
    uECC_set_rng(simple_rng);
    self->curve=uECC_secp256r1();                                   // added brackets
    uECC_make_key(self->IOTPublicKey,self->IOTPrivateKey,self->curve);
}
/*
uint8_t* string_to_uint8(const char* str) {
    size_t length = strlen(str);
    uint8_t* result = (uint8_t*)malloc((length + 1) * sizeof(uint8_t)); // +1 for null terminator

    if (result != NULL) {
        for (size_t i = 0; i < length; ++i) {
            result[i] = (uint8_t)str[i];
        }
        result[length] = '\0'; // Add null terminator
    }

    return result;
}
*/
void createSharedKey(Enc * self, char* CloudPublicKey){
    uECC_shared_secret(CloudPublicKey,self->IOTPrivateKey,self->SharedKey,self->curve);
}

uint8_t * getSharedKey(Enc * self){
    return self->SharedKey;
}

uint8_t getIOTPublicKey(Enc *self){
    return self->IOTPublicKey;
}



#define IV_SIZE 16 // Size of the IV in bytes for AES (128 bits)

// Function to generate a random IV
void generate_iv(uint8_t *iv, size_t iv_size) {
    size_t i;
    for (i = 0; i < iv_size; ++i) {
        // Generate a random byte and store it in the IV
        iv[i] = rand() & 0xFF;
    }
}


int simple_rng(uint8_t *dest, unsigned size){
    static uint8_t value = 0; // Static variable to keep track of the current value
    // Increment the value for each call (otherwise the two private keys will be the same)
    value++;
    // Fill 'dest' with the current value
    for (unsigned i = 0; i < size; ++i)
    {
        dest[i] = value + i + 8; // 7 is my random number. To be more random, you could read a value from a analog input, using the ADC.
    }
    return 1; // Indicate success
}

char* print_hex(uint8_t *buf, size_t len) {
    char *result = (char *)malloc((2 * len + 1) * sizeof(char)); // Allocate memory for the result string
    if (result == NULL) {
        // Handle memory allocation failure
        return NULL;
    }

    size_t index = 0;
    for (size_t i = 0; i < len; i++) {
        index += sprintf(result + index, "%02X", buf[i]); // Append formatted hex value to the result string
    }

    return result; // Return the formatted hex string
}
