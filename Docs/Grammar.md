

# Moa Grammar

```
program        -> declaration* EOF ;

declaration       -> funcDeclaration | varDeclaration | statement ;
varDeclaration    -> "var" IDENTIFIER ( "=" expression )? ";" ;
funcDeclaration   -> "func" function ;
function          -> IDENTIFIER "(" parameters?  ")" block ;
parameters        -> IDENTIFIER ( "," IDENTIFIER  )*;

statement         -> exprStatement | printStatement | block | ifStatement | whileStatement | forStatement | returnStatement ;
ifStatement       -> "if" "(" expression ")" statement ("else" statement )? ;
whileStatement    -> "while" "(" expression ")" statement ;
forStatement      -> "for" "(" 
    (varDeclaration | exprStatement | ";") 
    expression? ";" 
    expression? ")" statement 
;
block             -> "{" declaration* "}" ;
exprStatement     -> expression ";" ;
printStatement    -> "print" expression ";" ;
returnStatement   -> "return" expression? ";" ;

expression     → assignment ;
assignment     → IDENTIFIER "=" assignment | logic_or ;
logic_or       → logic_and ( "or" logic_and )* ;
logic_and      → comma ( "and" comma )* ;
comma          → equality ((",") equality)* ;
equality       → comparison ( ( "!=" | "==" ) comparison )* ;
comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
term           → factor ( ( "-" | "+" ) factor )* ;
factor         → unary ( ( "/" | "*" ) unary )* ;
unary          → ( "!" | "-" ) unary | call ;
call           → primary ( "(" arguments? ")" )* ;
arguments      → expression ( "," expression )* ;
primary        → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" | IDENTIFIER ;

```