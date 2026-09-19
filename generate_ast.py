#!/usr/bin/python

import sys

output_dir:str

def DefineAST(ast_file, output_dir:str, base_class_name:str, types:list[str]):

    print("\nabstract class " + base_class_name, file=ast_file)
    print("{", file=ast_file)
    DefineVisitor(ast_file, base_class_name, types)
    print("\tpublic abstract R Accept<R>(IVisitor<R> visitor);", file=ast_file)
    print("}", file=ast_file)

    for type in types:
        in_class_name = type.split(':')[0].replace(" ", "")
        fields = type.split(":")[1].strip()
        print("\n", file=ast_file)
        DefineTypes(ast_file, base_class_name, in_class_name, fields)

def DefineTypes(ast_file, base_class_name:str, class_name:str, fields:str):
    print("class " + class_name + " : " + base_class_name, file=ast_file)
    print("{", file=ast_file)

    FieldList = fields.split(', ')
    for a_field in FieldList:
        print("\t" + a_field + ";", file=ast_file)

    print("\n\tpublic override R Accept<R>(IVisitor<R> visitor)", file=ast_file)
    print("\t{", file=ast_file)
    print("\t\treturn visitor.visit" + class_name + base_class_name + "(this);", file=ast_file)
    print("\t}", file=ast_file)

    print("}", file=ast_file)

def DefineVisitor(ast_file, base_class_name:str, types:list[str]):
    print("\tpublic interface IVisitor<R>", file=ast_file)
    print("\t{", file=ast_file)

    for type in types:
        typeName = type.split(':')[0].replace(" ", "")
        print("\t\tR visit" + typeName + base_class_name + "(" + typeName + " expr);", file=ast_file)

    print("\t}", file=ast_file)

if len(sys.argv) == 1:
    print("Usage: generate_ast.py [output directory]")
else:
    output_dir = sys.argv[1]

    path:str = output_dir + "Expression" + ".cs"

    with open(path, mode="w", encoding="utf-8") as ast_file:
        print("\nnamespace Moa;", file=ast_file)
        DefineAST(ast_file, output_dir, "Expression", [
            "Unary      : Token? _operator, Expression? _right",
            "Binary     : Expression? _left, Token? _operator, Expression? _right",
            "Grouping   : Expression? _expr",
            "Literal    : Object? _value",
        ])
