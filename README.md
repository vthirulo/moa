# moa

Interpreter written in Sea Sharp 

# moa language basics

### Data Types

we have booleans - true or false, numbers which represents integers and floating-point numbers, string literal enclosed with double quotes, and finally `null` which means "no value"
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

moa supports logical `not`, logical `and`, and then logical `or` operators, where logical `not` requires one operand and is always a prefix

```C
not a  // false
not 0  // true

true and false  // false
true and true   // true

false or true   // true
false or false  // false
```
### Variables

you can create a variable using the `var` and assign it any value. By default, variable holds `null` value if the variable declared is not assigned a value by the user

```C
var count = 0;  // int type
var temp; // null
var name = "moa language";  // string type
```

### Control Flow

control flow determines the order in which statements execute - allowing a program to execute a block of code, repeat it or skip it based on a condition we define

```C
if (count < 5)
{
  print count + "is less than 5";
}
else {
  print count + "is greater than 5";
}
```
a `while` loop executes the body repeatedly as long as the condition expression is true

```C
var count = 0;

while (count < 5)
{
  print "value of count var: " + count;
  count = count + 1; 
}
```

we have the classic `for` loop as seen in C like lanuguages

```C
for (var count = 1; count <= 5; count = count + 1)
{
  print "value of count var: " + count;
} 
```

### Functions

use the `func` keyword to define a function in moa language, moa doesn't describe between declaration and definition like C language.

```C
func incrementor (int x)
{
  x = x + 1;
  return x;
}
```
In the above code, `x` is called the formal parameter. Now here is how we can call the above function

```C
incrementor(3);  // valid, but return value wasted

print incrementor(3);  // prints 4

var val = incrementor(3);  // variable named 'val' holds the value 4 which is for int type

incrementor();  // error 
```

When calling a function, the caller provides the values the function expects — these are called arguments (or actual parameters), since they're the actual values passed at the point of the call.

