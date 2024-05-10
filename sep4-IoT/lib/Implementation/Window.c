
#include "Window.h"


void openWindow(){
    display_setValues(0,2,0,1);
    servo(180);
    servo(0);
    servo(90);
    _delay_ms(1000);
    servo(180);
}

void closeWindow(){
    display_setValues(0,2,0,0);
    servo(180);
    servo(0);
    servo(90);
    _delay_ms(1000);
    servo(0);
}
