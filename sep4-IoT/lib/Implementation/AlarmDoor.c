#include "AlarmDoor.h"

char* alarm(bool isApproved){
    uint16_t distance = hc_sr04_takeMeasurement();

    char * ret = (char *) malloc(16*sizeof(char));
    if (distance <= 50)
    {
        if (!isApproved)
        {
            //display_setValues(13,14,10,13);
            sprintf(ret,"Hello,Thief");
            buzzer_beep();
        }
    }
    return ret;
}