# Htmt

A templating library for .NET projects designed to be easy to read, write and have good editor support
without needing any additional editor plugins. It fully supports trimming and native AOT compilation.

## Features

- **Simple syntax**: Htmt is a superset of HTML/XML, so you can write your templates in any text editor.
- **Interpolation**: You can interpolate values from a data dictionary into your templates.
- **Modifiers**: You can modify the interpolated values using modifiers.
- **Conditionals**: You can show or hide blocks using simple or complex expressions.
- **Partials**: You can include other templates inside your templates.
- **Loops**: You can loop over arrays and objects in your data dictionary.
- **Extendable**: You can implement custom attribute parsers and expression modifiers.

## Example syntax

```html
<!DOCTYPE html>
<html>
    <head>
        <title x:inner-text="{title}"></title>
    </head>
    <body>
        <h1 x:inner-text="{title}"></h1>
        
        <div class="posts" x:if="posts">
            <div x:for="posts" x:as="post">
                <h2 class="post-title">
                    <a x:attr-href="/blog/{post.url}" x:inner-text="{post.title | capitalize}"></a>
                </h2>
                <div class="post-date" x:inner-text="{post.date | date:yyyy-MM-dd}"></div>
                <div class="post-content" x:inner-html="{post.body}"></div>
            </div>
        </div>
    </body>
</html>
```

## Installation

```bash
dotnet add package Htmt
```

## Usage

A simple example of how to use Htmt with default configuration to generate HTML output:

```csharp
var template = "<h1 x:inner-text=\"{title}\"></h1>";
var data = new Dictionary<string, object?> { { "title", "Hello, World!" } };
var parser = new Htmt.Parser { Template = template, Data = data };
var html = parser.ToHtml();
```

You can also generate XML output via the `ToXml()` method.

## Attributes

### `x:inner-text`

Sets the inner text of the element to the value of the attribute.

Htmt template:

```html
<h1 x:inner-text="{title}"></h1>
```

Results in:

```html
<h1>Hello, World!</h1>
```

### `x:inner-html`

Sets the inner HTML of the element to the value of the attribute.

Htmt template where `content` is `<p>Hello, World!</p>`:

```html
<div x:inner-html="{content}"></div>
```

Results in:

```html
<div>
    <p>Hello, World!</p>
</div>
```

### `x:outer-text`

Sets the outer text of the element to the value of the attribute.
This is useful if you want to replace the entire element with text.

Htmt template where `title` is `Hello, World!`:

```html
<h1 x:outer-text="{title}"></h1>
```

Results in:

```html
Hello, World!
```

### `x:outer-html`

Sets the outer HTML of the element to the value of the attribute.
This is useful if you want to replace the entire element with HTML.

Htmt template where `content` is `<p>Hello, World!</p>`:

```html
<div x:outer-html="{content}"></div>
```

Results in:

```html
<p>Hello, World!</p>
```

### `x:inner-partial`

Sets the inner HTML of the element to the value of the attribute, which is a partial template.
This is useful if you want to include another template inside the current template.

Htmt template where `partial` is `<p>Hello, World!</p>`:

```html
<div x:inner-partial="partial"></div>
```

Results in:

```html
<div>
    <p>Hello, World!</p>
</div>
```

The `partial` key has to be inside the Data dictionary, and it has to be a string that contains a valid Htmt template.

The partial will inherit the data dictionary that the parent template has, so you can use the same data in the partial as you can in the parent template.

### `x:outer-partial`

Sets the outer HTML of the element to the value of the attribute, which is a partial template.
This is useful if you want to replace the entire element with another template.

Htmt template where `partial` is `<p>Hello, World!</p>`:

```html
<div x:outer-partial="partial"></div>
```

Results in:

```html
<p>Hello, World!</p>
```

The `partial` key has to be inside the Data dictionary, and it has to be a string that contains a valid Htmt template.

The partial will inherit the data dictionary that the parent template has, so you can use the same data in the partial as you can in the parent template.

### `x:if`

Removes the element if the attribute is falsey.

Htmt template where `show` is `false`:

```html
<div x:if="show">Hello, World!</div>
```

Results in:

```html
<!-- Empty -->
```

The `if` attribute also supports complex boolean expressions, like so:

```html
<div x:if="(show is true) and (title is 'Hello, World!')">Hello, World!</div>
```

This will only show the element if `show` is `true` and `title` is `Hello, World!`. The boolean expression validator
supports the following operators: `is`, `or` and `and`. You can also use parentheses to group expressions,
in case you want to have more complex expressions.

- The `is` operator is used to compare two values, and it supports the following types of values: `string`, `int`, `float`, `bool` and `null`.
- The `or` operator is used to combine two expressions with an OR operator.
- The `and` operator is used to combine two expressions with an AND operator.

### `x:unless`

Removes the element if the attribute is truthy.

Htmt template where `hide` is `true`:

```html
<div x:unless="hide">Hello, World!</div>
```

Results in:

```html
<!-- Empty -->
```

The `unless` attribute supports the same boolean expressions as the `if` attribute.

### `x:for`

Repeats the element for each item in the attribute.

Htmt template where `items` is an array of strings:

```html
<ul>
    <li x:for="items" x:as="item" x:inner-text="{item}"></li>
</ul>
```

Results in:

```html
<ul>
    <li>Item 1</li>
    <li>Item 2</li>
    <li>Item 3</li>
</ul>
```

Note that the `x:as` attribute is optional. If you just want to loop over a data structure,
but you don't care about using the data of each individual iteration, you can omit it.

### `x:attr-*` (Generic Value Attributes)

Above are all the special attributes that do some logical operation, but you can also use the `x:attr-*` attributes to set any attribute on an element to the value of the attribute.

For example, to set the `href` attribute of an element, you can use the `x:attr-href` attribute:

```html
<a x:attr-href="/blog/{slug}">Hello, World!</a>
```

Results in:

```html
<a href="/blog/hello-world">Hello, World!</a>
```

If `slug` is `hello-world`.

## Modifiers

All interpolated values in expressions can be modified using modifiers. Modifiers are applied to the value of the attribute, and they can be chained, like so:

```html
<h1 x:inner-text="{title | uppercase | reverse}"></h1>
```

Modifiers can also take arguments, like so:

```html
<h1 x:inner-text="{title | truncate:10}"></h1>
```

### `date`

Formats a date string using the specified format.

```html
<p x:inner-text="{date | date:yyyy-MM-dd}"></p>
```

### `uppercase`

Converts the value to uppercase.

```html
<p x:inner-text="{title | uppercase}"></p>
```

### `lowercase`

Converts the value to lowercase.

```html
<p x:inner-text="{title | lowercase}"></p>
```

### `capitalize`

Capitalizes the first letter of the value.

```html
<p x:inner-text="{title | capitalize}"></p>
```

### `reverse`

Reverses the value.

```html
<p x:inner-text="{title | reverse}"></p>
```

### `truncate`

Truncates the value to the specified length.

```html
<p x:inner-text="{title | truncate:10}"></p>
```

## Extending

### Attribute Parsers

You can add (or replace) attribute parsers in Htmt by adding them to the `AttributeParsers` array,
when creating a new instance of the `Parser` class.

```csharp
var parser = new Htmt.Parser
{
    Template = template,
    Data = data,
    AttributeParsers = [
        new MyCustomAttributeParser()
    ]
};
```

A custom attribute parser must extend the `BaseAttributeParser` parser, like so:

```csharp
public class CustomAttributeParser : BaseAttributeParser
{
    public override string XTag => "//*[@x:custom]";
    
    public override void Parse(XmlNodeList? nodes)
    {
        foreach (XmlNode node in nodes)
        {
            // You can parse expressions here with ParseExpression(), and 
            // do anything you want with the nodes as you'd otherwise do with XmlDocument stuff.
        }
    }
}
```

The `Parse` method is where the attribute parser should do its work, and the `XTag` property should return the xtag pattern for the nodes it should parse.

To get an array of default attribute parsers, you can call `Htmt.Parser.DefaultAttributeParsers()`,
if you want to add your custom attribute parsers to the default ones, but you can also mix and match however you like.

#### List of built-in attribute parsers

- `Htmt.AttributeParsers.InnerTextAttributeParser` - Parses the `x:inner-text` attribute.
- `Htmt.AttributeParsers.InnerHtmlAttributeParser` - Parses the `x:inner-html` attribute.
- `Htmt.AttributeParsers.OuterTextAttributeParser` - Parses the `x:outer-text` attribute.
- `Htmt.AttributeParsers.OuterHtmlAttributeParser` - Parses the `x:outer-html` attribute.
- `Htmt.AttributeParsers.InnerPartialAttributeParser` - Parses the `x:inner-partial` attribute.
- `Htmt.AttributeParsers.OuterPartialAttributeParser` - Parses the `x:outer-partial` attribute.
- `Htmt.AttributeParsers.IfAttributeParser` - Parses the `x:if` attribute.
- `Htmt.AttributeParsers.UnlessAttributeParser` - Parses the `x:unless` attribute.
- `Htmt.AttributeParsers.ForAttributeParser` - Parses the `x:for` attribute.
- `Htmt.AttributeParsers.GenericValueAttributeParser` - Parses the `x:*` attributes.

### Modifiers

You can add (or replace) modifiers in Htmt by adding them to the `Modifiers` array,
when creating a new instance of the `Parser` class.

```csharp
var parser = new Htmt.Parser
{
    Template = template,
    Data = data,
    Modifiers = [
        new MyCustomModifier()
    ]
};
```

A custom modifier must implement the `IExpressionModifier` interface:

```csharp
public interface IExpressionModifier
{
    public string Name { get; }

    public object? Modify(object? value, string? args = null);
}
```

The `Modify` method is where the modifier should do its work, and the `Name` property should return the name of the modifier.

To get an array of default modifiers, you can call `Htmt.Parser.DefaultExpressionModifiers()`,

if you want to add your custom modifiers to the default ones, but you can also mix and match however you like.

#### List of built-in modifiers

- `Htmt.ExpressionModifiers.DateExpressionModifier` - Formats a date string using the specified format.
- `Htmt.ExpressionModifiers.UppercaseExpressionModifier` - Converts the value to uppercase.
- `Htmt.ExpressionModifiers.LowercaseExpressionModifier` - Converts the value to lowercase.
- `Htmt.ExpressionModifiers.CapitalizeExpressionModifier` - Capitalizes the first letter of the value.
- `Htmt.ExpressionModifiers.ReverseExpressionModifier` - Reverses the value.
- `Htmt.ExpressionModifiers.TruncateExpressionModifier` - Truncates the value to the specified length.