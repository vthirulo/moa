#!/usr/bin/python

import sys

output_dir:str

def DefineAST(ast_file, output_dir:str, base_name:str, types:list[str]):

    print("\nabstract class " + base_name, file=ast_file)
    print("{", file=ast_file)
    print(file=ast_file)
    print("}", file=ast_file)

    for type in types:
        in_class_name = type.split(':')[0].replace(" ", "")
        fields = type.split(":")[1].strip()
        DefineTypes(ast_file, base_name, in_class_name, fields)

def DefineTypes(ast_file, base_name:str, class_name:str, fields:str):
    print("class " + class_name + " : " + base_name, file=ast_file)
    print("{", file=ast_file)

    FieldList = fields.split(', ')
    for a_field in FieldList:
        print("\t" + a_field + ";", file=ast_file)

    print("}", file=ast_file)

if len(sys.argv) == 1:
    print("Usage: generate_ast.py [output directory]")
else:
    output_dir = sys.argv[1]

    path:str = output_dir + "Expression" + ".cs"

    with open(path, mode="w", encoding="utf-8") as ast_file:
        print("\nnamespace Moa;", file=ast_file)
        DefineAST(ast_file, output_dir, "Expression", [
            "Binary     : Expression Left, Token Middle, Expression Right",
            "Grouping   : Expression Expr"
        ])
