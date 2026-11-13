
cmake_minimum_required(VERSION 4.0)

set(CMAKE_C_COMPILER clang)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/bin)

project(moa-vm LANGUAGES C)

add_executable(${CMAKE_PROJECT_NAME} main.c
    memory.c
    debug.c
    chunk.c
    value.c
)

target_link_libraries(${CMAKE_PROJECT_NAME})

