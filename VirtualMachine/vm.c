
#include "vm.h"
#include "chunk.h"
#include "debug.h"
#include "value.h"

#include <stdio.h>

VM vm;

static InterpretResult run();

// stack operations
static void stack_reset();
static void stack_push(Value value);
static Value stack_pop();

void init_VM()
{
    stack_reset();
}

void free_VM()
{
}

InterpretResult interpret(Chunk* chunk)
{
    vm.chunk = chunk;
    vm.instruction_pointer = chunk->code;
    return run();
}

static InterpretResult run()
{
#define READ_BYTE() (*vm.instruction_pointer++)
#define READ_CONSTANT() (vm.chunk->constants.values[READ_BYTE()])
#define BINARY_OP(op)                                                                                                  \
    do                                                                                                                 \
    {                                                                                                                  \
        int b = stack_pop();                                                                                           \
        int a = stack_pop();                                                                                           \
        stack_push(a op b);                                                                                            \
    } while (false)

    for (;;)
    {
#ifdef DEBUG_TRACE_EXECUTION
        printf("     ");
        for (Value* slot = vm.stack; slot <= vm.stack_top; slot++)
        {
            printf("[");
            print_value(*slot);
            printf("]");
        }
        printf("\n");
        disassemble_instruction(vm.chunk, (int)(vm.instruction_pointer - vm.chunk->code));
#endif
        uint8_t instruction;
        switch (instruction = READ_BYTE())
        {
        case OP_CONSTANT: {
            Value constant = READ_CONSTANT();
            stack_push(constant);
            break;
        }
        case OP_NEGATE: {
            stack_push(-stack_pop());
            break;
        }
        case OP_RETURN:
            print_value(stack_pop());
            printf("\n");
            return INTERPRET_OK;
        case OP_ADD:
            BINARY_OP(+);
            break;
        case OP_SUBTRACT:
            BINARY_OP(-);
            break;
        case OP_MULTIPLY:
            BINARY_OP(*);
            break;
        case OP_DIVIDE:
            BINARY_OP(/);
            break;
        }
    }

#undef READ_BYTE
#undef READ_CONSTANT
#undef BINARY_OP
}

static void stack_reset()
{
    vm.stack_top = vm.stack;
}

static void stack_push(Value value)
{
    *vm.stack_top = value;
    vm.stack_top++;
}

static Value stack_pop()
{
    vm.stack_top--;
    return *vm.stack_top;
}