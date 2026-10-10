#### Status: draft
**Covered:** numbers, truthiness, variables, concatenation, control flow. 
**Not yet covered:** arithmetic and comparison operators, logical operators, functions, errors. Behavior in unlisted sections is undefined until specified.
### Numbers

we declare numbers using the `var` keyword, and we can represent numbers as integers, and decimal formats such as 10, 1.5, 3, 4.6, and so on. Even the variable can hold expressions that evaluates to a single value such as `var expr = (1/2) * 4; print expr; // 2`
- Numbers prints in plain decimal notation and never in exponent form.
- Whole numbers print without any decimal points.

| Input                              | Expected output             |
| ---------------------------------- | --------------------------- |
| `print 8;`                         | `8`                         |
| `print 22.5;`                      | `22.5`                      |
| `print 0.000001;`                  | `0.000001`                  |
| `print 1000000000000000;`          | `1000000000000000`          |
| `print 1 / 3;`                     | `0.3333333333333333`        |
| `print 0.1 + 0.2;`                 | `0.30000000000000004`       |
| `print 0.00000000000000000000001;` | `0.00000000000000000000001` |

Note that internally, the number value is defined as `Double` in implementation language.
### Truthness

Values such as `0`, `false`, `null` and empty string `""` are considered false and everything other than that are considered true. We can perform logical operations on them:
```C
not 0
not false
not null
not ""
```
all the above evaluates to true, where `not "true"` evaluates to false - you get the idea.
### Variables

We can declare a variable of any types using the `var` keyword and the type of variable depends on the value it contains, as Moa language supports number, booleans, null and string type so far 
```C
var a = 10, b = 10.5;
var c = a + b;
var d = "Moa Language", e = 100;
var f, g;
```
as you can see multiple variables of different types can be declared and assigned, where assignment operation can be optional. 
- Uninitialized variables doesn't contain `null` unless explicitly assigned `null` to the variable.
- Assigning a value to undeclared variable is considered an error
- Printing or accessing an uninitialized variable is considered an error unless explicitly assigned `null` to the variable.

### Concatenation

an operand of number type is implicitly converted to a string if and only if another operand is a string and the operator used is `+` which is usually called a concatenation operator.
```C
var a = 19;
var str = a + " is an integer";
print str;
a = a + 10;
print "a value is updated, a now contains " + a;
```

### Control Flow

control flow determines the order in which statements execute - allowing a program to execute a block of code, repeat it or skip it based on a condition we define
```C
var count = 3;
if (count < 5)
{
	print count + " is less than 5";
}
else {
	print count + " is greater than 5";
}
```

a `while` loop executes the body repeatedly as long as the condition expression is true
```C
var count = 0;

while (count < 5) {
	print "value of count var: " + count;
	count = count + 1;
}
```

we have the classic `for` loop as seen in C like lanuguages
```C
for (var count = 1; count <= 5; count = count + 1) {
	print "value of count var: " + count;
}
```