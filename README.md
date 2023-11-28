# DocFx MermaidJS

## Mission Statement

To provide a plugin to convert MermaidJS notations to diagrams during the build of a DocFX project.

## Introduction

This DocFX MermaidJS plugin is a wrapper around the mermaid NPM package. It is aimed at doing build time generation of the mermaid diagram so:

* The syntax can be validated at build time.
* The image is only built once.
* The image can be embedded in a pdf.

## Credits

* https://dotnet.github.io/docfx/
* https://mermaid.js.org/
* https://github.com/mermaid-js/mermaid-cli
* https://github.com/mermaid-js/mermaid
* https://github.com/dpvreony/article-statiq-mermaid
* https://github.com/KevReed/DocFx.Plugins.PlantUml

## Getting Started

### 1. Create a console application (or similar)
### 2. Add a nuget package reference to "Dhgms.DocFX.Mermaid.Plugin" in your docfx_project
### 3. Add the following initialisation

```cs
                var options = new BuildOptions
                {
                    // Enable MermaidJS markdown extension
                    ConfigureMarkdig = pipeline => pipeline.UseMermaidJsExtension(new MarkdownContext())
                };
                await Docset.Build("docfx.json", options);
```

You can see an example of this in

1. [The sample console application in this repository (github.com/dpvreony/docfx-mermaidjs/tree/main/src/Dhgms.DocFx.MermaidJs.Sample.Cmd)](https://github.com/dpvreony/docfx-mermaidjs/tree/main/src/Dhgms.DocFx.MermaidJs.Sample.Cmd)
2. [The console application in my main documentation repository (github.com/dpvreony/documentation/tree/main/src/docfx_project)](https://github.com/dpvreony/documentation/tree/main/src/docfx_project)

NOTES:
* Only inline PNG is supported, this is due to a limitation in the plug in model and adding new files to the file cache on the fly. I may revisit this in future. The plug in itself exposes SVG data if you want to play with it.

You can adjust the settings by viewing the detailed documentation.

### 4. Adding a diagram

In your markdown files add a code block with a mermaid descriptor like so:

````
```mermaid
graph TD;
    A-->B;
    A-->C;
    B-->D;
    C-->D;
```
````

Then you can run the build and you should see the image output in place of the mermaid markdown syntax.

```mermaid
graph TD;
    A-->B;
    A-->C;
    B-->D;
    C-->D;
```


## Viewing the documentation

The documentation can be found at https://docs.dpvreony.com/projects/docfx-mermaidjs/

## Contributing to the code

See the [contribution guidelines](CONTRIBUTING.md).
