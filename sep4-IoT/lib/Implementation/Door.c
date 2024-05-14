#include "Door.h"



int openDoor(){
 display_setValues(0,3,0,1);
    servo(180);
    servo(0);
    servo(120);
    servo(180);
    return 1;
}

int closeDoor(){
 display_setValues(0,3,0,0);
    servo(180);
    servo(0);
    servo(90);
    servo(0);
    return 0;
}