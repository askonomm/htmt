# Htmt

A simple templating language that is a superset of HTML/XML and is designed to be easy to read, write and have good editor support
due to it being HTML/XML based and thus not needing any additional editor plugins.

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

### `x:href`

Sets the `href` attribute of the element to the value of the attribute.

Htmt template where `url` is `/hello-world`:

```html
<a x:href="{url}">Hello, World!</a>
```

Results in:

```html
<a href="/hello-world">Hello, World!</a>
```

## Extending

To be written.