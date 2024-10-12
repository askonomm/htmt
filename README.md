# Htmt

A simple templating language for .NET projects that is a superset of HTML/XML and is designed to be easy to read, write and have good editor support
due to it being HTML/XML based and thus not needing any additional editor plugins. It fully supports 
trimming and native AOT compilation.

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
                <h2>
                    <a x:href="/blog/{post.url}" x:inner-text="{post.title}"></a>
                </h2>
                <div x:inner-html="{post.body}"></div>
            </div>
        </div>
    </body>
</html>
```

Note how in `x:if`, `x:for` and `x:as` attributes the value is not enclosed in curly braces whereas in `x:inner-text` and `x:inner-html` it is.
That's because these attributes are not meant to be interpolated, but rather to be evaluated as expressions.

Attributes that are meant to be interpolated are enclosed in curly braces, like `{title}` and `{post.title}`, 
which will be replaced with the value of the `title` and `post.title` keys in the data dictionary, respectively. 
It also means you can add other text around the interpolation, like `Hello, {name}!`.

## Installation

```bash
dotnet add package Htmt
```

## Usage

A simple example of how to use Htmt with default configuration to generate HTML output:

```csharp
var template = "<h1 x:inner-text=\"{title}\"></h1>";
var data = new Dictionary<string, object> { { "title", "Hello, World!" } };
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

### `x:*` (Generic Value Attributes)

Above are all the special attributes that do some logical operation, but you can also use the `x:*` attributes to set any attribute on an element to the value of the attribute.

For example, to set the `href` attribute of an element, you can use the `x:href` attribute:

```html
<a x:href="/blog/{slug}">Hello, World!</a>
```

Results in:

```html
<a href="/blog/hello-world">Hello, World!</a>
```

If `slug` is `hello-world`.

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

A custom attribute parser must implement the `IAttributeParser` interface:

```csharp
public interface IAttributeParser
{
    public string XTag { get; }
    
    public void Parse(XmlDocument xml, Dictionary<string, object> data, XmlNodeList? nodes);
}
```

The `Parse` method is where the attribute parser should do its work, and the `XTag` property should return the xtag pattern for the nodes it should parse. 
For example if you want to add a custom attribute parser for an attribute called `x:custom`, you would do the following:

```csharp
public class CustomAttributeParser : IAttributeParser
{
    public string XTag => "//*[@x:custom]";
    
    public void Parse(XmlDocument xml, Dictionary<string, object> data, XmlNodeList? nodes)
    {
        foreach (XmlNode node in nodes)
        {
            // all of the nodes are nodes that have the `x:custom` attribute,
            // so you can do whatever you want with them here.
        }
    }
}
```

To get an array of default attribute parsers, you can call `Htmt.Parser.DefaultAttributeParsers()`, 
if you want to add your custom attribute parsers to the default ones. But you can also mix and match however you like.

#### List of built-in attribute parsers

- `Htmt.AttributeParsers.InnerTextAttributeParser` - Parses the `x:inner-text` attribute.
- `Htmt.AttributeParsers.InnerHtmlAttributeParser` - Parses the `x:inner-html` attribute.
- `Htmt.AttributeParsers.OuterTextAttributeParser` - Parses the `x:outer-text` attribute.
- `Htmt.AttributeParsers.OuterHtmlAttributeParser` - Parses the `x:outer-html` attribute.
- `Htmt.AttributeParsers.IfAttributeParser` - Parses the `x:if` attribute.
- `Htmt.AttributeParsers.UnlessAttributeParser` - Parses the `x:unless` attribute.
- `Htmt.AttributeParsers.ForAttributeParser` - Parses the `x:for` attribute.
- `Htmt.AttributeParsers.GenericValueAttributeParser` - Parses the `x:*` attributes.

## Limitations

Htmt is written on top of the `System.Xml` namespace, which means it has the same limitations as `XmlDocument`. 
This means that some HTML that is otherwise allowed by the browsers will throw an error when parsed by Htmt. 

For example, the following HTML will throw an error:

```html
<img src="image.jpeg">
```

This is because the `img` tag self-closes in HTML, but not in XML. 
To fix this, you can add a closing tag like so:

```html
<img src="image.jpeg" />
```

While a third-party library would probably solve this, my goal was to keep the library 
dependency-free and as simple as possible because I'm just one person and I want something that I can maintain easily 
for a long time.