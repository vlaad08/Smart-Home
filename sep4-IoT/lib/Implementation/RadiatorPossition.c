#include "RadiatorPossition.h"


void setRadiatorLevel(uint8_t level) {
    uint8_t angle = 0;
    display_setValues(0,4,0,level); //0=0 1=1 2=2 3=3 4=4 5=9 6=8 7=7 8=8 9=9 10=a 11=
    
    switch (level) {
        case 0:
            angle = 0;
            break;
        case 1:
            angle = 30;
            break;
        case 2:
            angle = 60;           
            break;
        case 3:
            angle = 90;           
            break;
        case 4:
            angle = 120;           
            break;
        case 5:
            angle = 150;           
            break;
        case 6:
            angle = 180;           
            break;
        default:
            // Invalid level, set angle to 0
            angle = 0;           
            break;
    }
    servo(angle);
}