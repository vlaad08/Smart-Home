#include "AdjustLight.h"
#define MaxLightLevel 4

void custom_delay_ms(uint16_t milliseconds) {
#ifdef _AVR_
    _delay_ms(milliseconds); // Use _delay_ms() for AVR microcontroller
#else
    usleep(milliseconds * 1000); // Use usleep() for POSIX systems (convert milliseconds to microseconds)
#endif
}

char* AdjustLight(uint8_t level){
    leds_turnOff(1);
    leds_turnOff(2);
    leds_turnOff(3);
    leds_turnOff(4);
    uint8_t count=0;
    
    while (count<level)
    {
        if (count>4){
        count=4;
        break;
        }
        count++;
        leds_turnOn(count);
        custom_delay_ms(1000);
    }
    if (count>4)
    {
        count=4;
    }
    
    display_setValues(0,4,0,count);
    char* x = malloc(8);
    sprintf(x,"0,4,0,%d",count);
    return x;
}