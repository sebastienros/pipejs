# PipeJs
PipeJs is a Command Line JavaScript interpreter for .NET. It means you can execute custom JavaScript from the command line, by passing JavaScript statements and script files as arguments. 

## Quick Start

PipeJs is used by calling `pipejs.exe` from the command line. Here are some typical ways to use it:

### Returning a JavaScript primitive value
Any returned primitive value is displayed using `.toString()` on it. 

```
pipejs return Math.PI * 2
```
`6.28318530717959`

### Returning a JavaScript complex object
Complex objects are serialized into JSON.

```
pipejs var o = {}; o.Color = 'Red'; return o;
```
`{"Color":"Red"}`

### Piping arguments to a script
Any value can be passed to `pipejs` as the `this` argument.
```
echo { "foo" : "bar" } | pipejs return this.foo;
```
`bar`

### Outputing custom text to the console
The `console` object is available.
```
echo 9 | pipejs "if(this > 5) console.log(this); else console.warning(this);"
```

### Using an external script file
PipeJs can execute a script from a specific file.
```
pipejs @myScript.js
```

### Piping outputs to inputs
The output of a pipejs script can be used as the input of another one.
```
echo 9 | pipejs return this * 2 | pipejs return this - 10
```
`8`


