#pragma once
#include <stdbool.h>
#include <string.h>
#include "stdlib.h"
#include <stdio.h>

bool contains(const char *haystack, const char *needle);

char** split(const char* str, const char* delim, int* num_tokens);