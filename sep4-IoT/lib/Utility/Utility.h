#pragma once
#include <stdbool.h>


bool contains(const char *haystack, const char *needle);

char** split(const char* str, const char* delim, int* num_tokens);