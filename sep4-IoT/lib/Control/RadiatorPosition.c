#include "RadiatorPosition.h"

uint8_t* setRadiatorLevel(uint8_t level,int hardwareId) {
    uint8_t * values= calloc(5,sizeof(uint8_t)); // Array to hold level, angle, and display values

    uint8_t angle = 0;
    display_setValues(hardwareId, 1, 0, level);
    
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
            angle = 0;           
            break;
    }
    
    servo(angle);

    values[0] = 0;       
    values[1] = 1;      
    values[2] = 0;      
    values[3] = level;  
    values[4] = angle;  
    return values;
}