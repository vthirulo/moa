# moa

Interpreter written in Sea Sharp 

# moa language basics

### Data Types

we have booleans - true or false, numbers which represents integers and floating-point numbers, string literal enclosed with double quotes, and finally nil which means "no value"
```C
true;
false;

13;
13.05;

"Hello World";
"Compilers";
""; // empty string
"13.05"; // it's string
```

### Expressions

#### Arithmetic Operators

moa supports addition, subtraction, multiplication and divison for numbers only, but you can use addition symbol to concatenate two or more strings into one

```C

a = 10;
b = 5;
c = 0;

c = a + b; // 15
c = a - b; // 5
c = a * b; // 50
c = a / b; // 2

d = "Pineapple ";
e = "Honey";
f = "";

f = d + " and " + e; // "Pineapple  and Honey"
```

#### Comparison and Equality Operators

moa supports less than, less than or equal, greater than, greater than or equal, equality and inequality operators - where they return boolean results

```C
a < b;   // false 
a <= a;  // true 
a > b;   // true
b >= a;  // false 

a == b;  // false
a != b;  // true
```

#### Logical Operators

moa supports logical not (!), logical and (&&), and then logical or (||) operators, where logical not requires one operand and is always a prefix

```C
!a  // false
!0  // true

true && false  // false
true && true   // true

false || true   // true
false || false  // false
```

