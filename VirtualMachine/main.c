
#include "chunk.h"
#include "vm.h"

int main(int argc, const char* argv[])
{
    init_VM();
    Chunk chunk;
    init_chunk(&chunk);

    int constant = add_constant(&chunk, 9);

    write_chunk(&chunk, OP_CONSTANT);
    write_chunk(&chunk, constant);

    constant = add_constant(&chunk, 12);

    write_chunk(&chunk, OP_CONSTANT);
    write_chunk(&chunk, constant);

    write_chunk(&chunk, OP_ADD);

    write_chunk(&chunk, OP_RETURN);
    interpret(&chunk);
    free_VM();
    free_chunk(&chunk);
    return 0;
}