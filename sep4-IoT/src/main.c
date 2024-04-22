#include "wifi.h"
#include "tone.h"
#include "servo.h"
#include "leds.h"
#include "buzzer.h"
#include "buttons.h"
#include "display.h"
#include "pc_comm.h"
#include <util/delay.h>
#include <stdlib.h>
#include <string.h>
// Buffer to hold the received message
char received_message_buffer[128];

void func(){
    pc_comm_send_string_blocking(received_message_buffer);
    if (strcmp(received_message_buffer,"FORCE")==0)
        {
            buzzer_beep();
        }
}

void increaseNumber(i){
    if (i > 16)
    {
        /
    }
    
}

int main(){
    pc_comm_init(9600,NULL);
    wifi_init();
    display_init();
    buttons_init();
    tone_init();
    leds_init();
    wifi_command_join_AP("Filip's Galaxy S21 FE 5G","jgeb6522");
    servo(90);
    wifi_command_create_TCP_connection("192.168.180.220",6868,func,received_message_buffer);
    uint16_t i = 1;
    while(1)
    {
        wifi_command_TCP_transmit((uint8_t*)"hello ", 7);
        _delay_ms(20);
        display_setValues(i,i,i,i);
        _delay_ms(1000);
        //servo(50);
        leds_turnOn(2);
        i++;
    }
}