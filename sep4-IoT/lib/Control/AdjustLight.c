#include "AdjustLight.h"

void static custom_delay_ms(uint16_t milliseconds) {
#ifdef __AVR__
    _delay_ms(milliseconds); // Use _delay_ms() for AVR microcontroller
#else
    usleep(milliseconds * 1000); // Use usleep() for POSIX systems (convert milliseconds to microseconds)
#endif
}

char* AdjustLight(uint8_t level,int hardwareId){
    custom_delay_ms(1000);
    leds_turnOff(1);
    leds_turnOff(2);
    leds_turnOff(3);
    leds_turnOff(4);
    uint8_t count=0;
    
    while (count<level)
    {
        count++;
        leds_turnOn(count);
        custom_delay_ms(1000);
    }
    
    display_setValues(hardwareId,4,0,count);
    char* x = (char *) malloc(12*sizeof(char));
    sprintf(x,"0,4,0,%d",count);
    return x;
}