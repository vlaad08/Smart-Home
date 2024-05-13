#include "AlarmDoor.h"

char* alarm(_Bool isApproved){
    uint16_t distance = hc_sr04_takeMeasurement();

    char* ret = calloc(128,sizeof(char));
    if (distance <= 50)
    {
        if (!isApproved)
        {
            //display_setValues(13,14,10,13);
            sprintf(ret,"SOMEONE IS TRYING TO BREAK INTO THE HOUSE!!!");
            buzzer_beep();
        }
    }
    return ret;
}