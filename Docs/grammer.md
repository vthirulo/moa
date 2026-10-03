

# Moa Grammer

```

program        -> declaration* EOF ;

declaration    -> varDeclaration | statement ;
varDeclaration -> "var" IDENTIFIER ( "=" expression )? ";" ;

statement      -> exprStatement | printStatement | block ;
block          -> "{" declaration* "}" ;
exprStatement  -> expression ";" ;
printStatement -> "print" expression ";" ;

expression     → assignment ;
assignment     → IDENTIFIER "=" assignment | comma ;
comma          → equality ((",") _equality)* ;
equality       → comparison ( ( "!=" | "==" ) comparison )* ;
comparison     → term ( ( ">" | ">=" | "<" | "<=" ) _term )* ;
term           → factor ( ( "-" | "+" ) _factor )* ;
factor         → unary ( ( "/" | "*" ) _unary )* ;
unary          → ( "!" | "-" ) _unary | primary ;
primary        → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" | IDENTIFIER ;

```