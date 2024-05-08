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
#include <dht11.h>
#include <periodic_task.h>
#include <servo.h>
#include <stdio.h>
// Buffer to hold the received message
char received_message_buffer[128];

void getTemptAndHum(){
    uint8_t humidity_integer = 0; 
    uint8_t  humidity_decimal = 0; 
    uint8_t temperature_integer = 0; 
    
    uint8_t temperature_decimal = 0;

    DHT11_ERROR_MESSAGE_t status = dht11_get(&humidity_integer,&humidity_decimal,&temperature_integer,&temperature_decimal);

    if (status == DHT11_OK)
    {
        char result[50];
        sprintf(result, "TEMP: %d.%d; HUMI: %d.%d\n", temperature_integer, temperature_decimal, humidity_integer, humidity_decimal);
        wifi_command_TCP_transmit((uint8_t*)result,strlen(result));
    }
    else{
        wifi_command_TCP_transmit((uint8_t*)"Temp Hum Error ",16);
    }
}

void setRadiatorLevel() {
    // Map levels to angles
    uint8_t angle = 180; //100 is 90* ; zero is perfect start; 
    
    
    // Call the servo function with the mapped angle
    servo(angle);
}

void setRadiatorLevel2() {
    // Map levels to angles
    uint8_t angle = 0; //100 is 90* ; zero is perfect start; 
    
    
    // Call the servo function with the mapped angle
    servo(angle);
}


void Callback(){
    pc_comm_send_string_blocking(received_message_buffer);
    wifi_command_TCP_transmit((uint8_t*)"Recieved ", 10);
    if (strcmp(received_message_buffer,"Test")==0)
    {
        wifi_command_TCP_transmit((uint8_t*)"You requested x ",17);
    }
    
}


int main(){
    pc_comm_init(9600,NULL);
    wifi_init();
    dht11_init();
    display_init();
    buttons_init();
    tone_init();
    leds_init();
    wifi_command_join_AP("002","zabijemsazalentilku");
    wifi_command_create_TCP_connection("192.168.236.153",23,Callback,received_message_buffer);
    wifi_command_TCP_transmit((uint8_t*)"Connected ", 11);

    periodic_task_init_a(getTemptAndHum,120000);
    while(1){
       setRadiatorLevel2();
       _delay_ms(100);
    setRadiatorLevel(); 
    }
    




    while (1)
    {
        /* code */
    }
    
    
    
    display_setValues(1,0,0,0);
}
