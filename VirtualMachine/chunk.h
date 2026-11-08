
#ifndef MOA_CHUNK_H
#define MOA_CHUNK_H

#include "common.h"
#include "memory.h"

typedef enum {
  OP_RETURN,
} OpCode;

typedef struct {
  int count;
  int capacity;
  uint8_t *code;
} Chunk;

void init_chunk(Chunk *chunk);

void write_chunk(Chunk *chunk, uint8_t byte);

void free_chunk(Chunk *chunk);

#endif
