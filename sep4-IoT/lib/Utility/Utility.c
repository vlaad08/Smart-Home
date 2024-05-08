#include "Utility.h"
#include <stdio.h>


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