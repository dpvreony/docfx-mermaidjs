# DocFx MermaidJS

## Mission Statement

To provide a plugin to convert MermaidJS notations to diagrams during the build of a DocFX project.

## Introduction

This DocFX MermaidJS plugin is a wrapper around the mermaid-cli NPM package. It is aimed at doing build time generation of the mermaid diagram so:

* The syntax can be validated at build time.
* The image is only built once.
* The image can be embedded in a pdf.

## Credits

* https://dotnet.github.io/docfx/
* https://mermaid.js.org/
* https://github.com/mermaid-js/mermaid-cli
* https://github.com/mermaid-js/mermaid
* https://github.com/KevReed/DocFx.Plugins.PlantUml

## Getting Started

### Pre-requisites

You will need:
* NodeJS
* NPM
* A docfx project

The instructions below assume the DocFX project is called "docfx_project"

### Setting up NodeJS

In your docfx project folder create a package.json file with content similar to:

```json
{
    "name": "docfx",
    "version": "1.0.0",
    "devDependencies": {
        "@mermaid-js/mermaid-cli": "9.2.2"
    }
}
```

Script and\or carry out a package restore using the following command

```cmd
pushd docfx_project && npm install && popd
```

### Setting up DocFX

Add a nuget package reference to "Dhgms.DocFX.Mermaid.Plugin" in your docfx_project

Update your docfx.json to include the template. This assumes you are using the default templates for the process. You need to put this plugin BEFORE your output template.

For HTML output:

```json
    "build": {
        "template": [
            "markdownmermaidjs",
            "default"
        ]
    }
```

For PDF output:

```json
    "pdf": {
        "template": [
            "markdownmermaidjs",
            "pdf.default"
        ]
    }
```

By default the plugin has the following behaviour:

* Uses inline emdedding in the HTML
* Creates png images
* Runs the mermaid-cli externally

You can adjust the settings by viewing the detailed documentation.

### Adding a diagram

In your markdown files add a code block with a mermaid descriptor like so:

```mermaid
graph TD;
    A-->B;
    A-->C;
    B-->D;
    C-->D;
```

Then you can run the build and you should see the image output in place of the mermaid markdown syntax.

## Viewing the documentation

The documentation can be found at https://dpvreony.github.io/docfx-mermaidjs/

## Contributing to the code

See the [contribution guidelines](CONTRIBUTING.md).
