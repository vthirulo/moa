
#ifndef MOA_VALUE_H
#define MOA_VALUE_H

#include "common.h"

typedef int Value;

typedef struct
{
    int count;
    int capacity;
    Value* values;
} ValueArray;

void init_value_array(ValueArray* array);

void write_value_array(ValueArray* array, Value value);

void free_value_array(ValueArray* array);

void print_value(Value value);

#endif
