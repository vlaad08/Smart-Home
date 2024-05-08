#include "TempAndHum.h"



uint8_t * getTempAndHum(){
    uint8_t humidity_integer = 0; 
    uint8_t  humidity_decimal = 0; 
    uint8_t temperature_integer = 0; 
    
    uint8_t temperature_decimal = 0;

    DHT11_ERROR_MESSAGE_t status = dht11_get(&humidity_integer,&humidity_decimal,&temperature_integer,&temperature_decimal);
    char * result=(char *)calloc(16, sizeof(char));
    result[15]='\0';

    if (status == DHT11_OK)
    {
        sprintf(result, "T:%d.%d   H:%d.%d ", temperature_integer, temperature_decimal, humidity_integer, humidity_decimal);
    }
    else{
        strcpy(result, "Temp Hum Error");
    }

    return (uint8_t *)result;
}