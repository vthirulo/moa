#ifndef MOA_VM_H
#define MOA_VM_H

#include "chunk.h"

typedef struct
{
    Chunk* chunk;
    uint8_t* instruction_pointer;
} VM;

typedef enum
{
    INTERPRET_OK,
    INTERPRET_COMPILE_ERROR,
    INTERPRET_RUNTIME_ERROR,
} InterpretResult;

void init_VM();
void free_VM();

InterpretResult interpret(Chunk* chunk);

#endif
