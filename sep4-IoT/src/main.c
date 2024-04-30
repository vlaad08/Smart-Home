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
#include <stdbool.h>


#include "uECC.h"
#include "Enc.h"
// Buffer to hold the received message
char received_message_buffer[128];

bool contains(const char *haystack, const char *needle) {
    return strstr(haystack, needle) != NULL;
}

char** split(const char* str, const char* delim, int* num_tokens) {
    // Count the number of tokens
    int count = 1; // At least one token exists
    const char* tmp = str;
    while ((tmp = strstr(tmp, delim))) {
        count++;
        tmp += strlen(delim); // Move pointer past the delimiter
    }

    // Allocate memory for array of pointers
    char** tokens = (char**)malloc(count * sizeof(char*));
    if (tokens == NULL) {
        return NULL; // Memory allocation failed
    }

    // Split the string
    int i = 0;
    char* token = strtok((char*)str, delim);
    while (token != NULL) {
        tokens[i++] = token;
        token = strtok(NULL, delim);
    }

    *num_tokens = count;
    return tokens;
}

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

void Callback(){
    //pc_comm_send_string_blocking(received_message_buffer);
    wifi_command_TCP_transmit((uint8_t*)"Recieved ", 10);
    if (contains(received_message_buffer,"Cloud PK:"))
    {
        const char* delim = ":";
        int num_tokens; //its the number of how many splits happened in one string
        char** tokens = split(received_message_buffer, delim, &num_tokens);
        pc_comm_send_array_blocking(tokens[1],64);
        
        free(tokens);
    }
    
}

void setup(){
    pc_comm_init(9600,NULL);
    wifi_init();
    dht11_init();
    display_init();
    buttons_init();
    tone_init();
    leds_init();
    wifi_command_join_AP("Filip's Galaxy S21 FE 5G","jgeb6522");
    wifi_command_create_TCP_connection("192.168.175.153",6868,Callback,received_message_buffer);
}


int main(){
    setup();
    Enc enc;
    createIOTKeys(&enc);
    char* public_key_hex = print_hex(getIOTPublicKey(&enc), 32);
    char* connection = (char*)malloc((sizeof("Connected:") + strlen(public_key_hex) + 1) * sizeof(char));
    sprintf(connection, "Connected:%s", public_key_hex);
    wifi_command_TCP_transmit((uint8_t*)connection,strlen(connection));
    free(connection);

    periodic_task_init_a(getTemptAndHum,120000);


    while (1)
    {
        /* code */
    }
}