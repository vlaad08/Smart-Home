#include "Door.h"


/*void custom_delay_ms(uint16_t milliseconds) {
#ifdef __AVR__
    _delay_ms(milliseconds); // Use _delay_ms() for AVR microcontroller
#else
    usleep(milliseconds * 1000); // Use usleep() for POSIX systems (convert milliseconds to microseconds)
#endif
}*/

int openDoor(){
 display_setValues(0,3,0,1);
    servo(180);
    servo(0);
    servo(120);
    //custom_delay_ms(1000);
    servo(180);
    return 1;
}

int closeDoor(){
 display_setValues(0,3,0,0);
    servo(180);
    servo(0);
    servo(90);
    //custom_delay_ms(1000);
    servo(0);
    return 0;
}