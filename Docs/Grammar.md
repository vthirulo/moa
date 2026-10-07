

# Moa Grammar

```

program        -> declaration* EOF ;

declaration    -> varDeclaration | statement ;
varDeclaration -> "var" IDENTIFIER ( "=" expression )? ";" ;

statement      -> exprStatement | printStatement | block | 
    ifStatement | whileStatement | forStatement ;
ifStatement    -> "if" "(" expression ")" statement ("else" statement )? ;
whileStatement -> "while" "(" expression ")" statement ;
forStatement   -> "for" "(" 
    (varDeclaration | exprStatement | ";") 
    expression? ";" 
    expression? ")" statement 
;
block          -> "{" declaration* "}" ;
exprStatement  -> expression ";" ;
printStatement -> "print" expression ";" ;

expression     → assignment ;
assignment     → IDENTIFIER "=" assignment | logic_or ;
logic_or       → logic_and ( "or" logic_and )* ;
logic_and      → comma ( "and" comma )* ;
comma          → equality ((",") _equality)* ;
equality       → comparison ( ( "!=" | "==" ) comparison )* ;
comparison     → term ( ( ">" | ">=" | "<" | "<=" ) _term )* ;
term           → factor ( ( "-" | "+" ) _factor )* ;
factor         → unary ( ( "/" | "*" ) _unary )* ;
unary          → ( "!" | "-" ) _unary | primary ;
primary        → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" | IDENTIFIER ;

```