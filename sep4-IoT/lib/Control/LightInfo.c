#include "LightInfo.h"

uint8_t * getLightInfo(uint8_t hardwareId) {
    uint16_t light_value = light_read(); 
    char * result=(char *)calloc(16, sizeof(char));
    sprintf(result, "%d-LIGHT: %d    ",hardwareId, light_value);
    return  (uint8_t *)result;
}