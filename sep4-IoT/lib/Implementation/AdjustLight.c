#include "AdjustLight.h"
#define MaxLightLevel 4


void AdjustLight(uint8_t * level){
    leds_turnOff(1);
    leds_turnOff(2);
    leds_turnOff(3);
    leds_turnOff(4);
    uint8_t count=0;
    
    while (count<=MaxLightLevel||count<*level)
    {
        count++;
        leds_turnOn(count);
        _delay_ms(1000);
    }
    if (count>4)
    {
        count=4;
    }
    
    display_setValues(0,4,0,count);
}