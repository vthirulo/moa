#include "chunk.h"

void init_chunk(Chunk *chunk)
{
    chunk->code = 0;
    chunk->capacity = 0;
    chunk->code = NULL;
}

void write_chunk(Chunk *chunk, uint8_t byte)
{
}
