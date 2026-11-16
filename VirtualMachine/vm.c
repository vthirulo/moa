
#include "vm.h"
#include "chunk.h"
#include "debug.h"
#include "value.h"

#include <stdio.h>

VM vm;

void init_VM()
{
}

void free_VM()
{
}

static InterpretResult run()
{
#define READ_BYTE() (*vm.instruction_pointer++)
#define READ_CONSTANT() (vm.chunk->constants.values[READ_BYTE()])

#ifdef DEBUG_TRACE_EXECUTION
    disassemble_instruction(vm.chunk, (int)(vm.instruction_pointer - vm.chunk->code));
#endif

    for (;;)
    {
        uint8_t instruction;
        switch (instruction = READ_BYTE())
        {
        case OP_CONSTANT: {
            Value constant = READ_CONSTANT();
            print_value(constant);
            printf("\n");
            break;
        }
        case OP_RETURN:
            return INTERPRET_OK;
        }
    }

#undef READ_BYTE
#undef READ_CONSTANT
}

InterpretResult interpret(Chunk* chunk)
{
    vm.chunk = chunk;
    vm.instruction_pointer = chunk->code;
    return run();
}