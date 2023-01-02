# DocFx MermaidJS

## Mission Statement

To provide a plugin to convert MermaidJS notations to diagrams during the build of a DocFX project.

## Introduction

This DocFX MermaidJS plugin is a wrapper around the mermaid-cli NPM package

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

Update your docfx.json to include the template.

For HTML output

```json
    "build": {
        "template": [
            "markdownmermaidjs",
            "default"
        ]
    }
```

For PDF output

```json
    "pdf": {
        "template": [
            "markdownmermaidjs",
            "pdf.default"
        ]
    }
```

### Adding a diagram

By default the plugin has the following behaviour:

* Uses inline emdedding in the HTML
* Creates png images
* Runs the mermaid-cli externally

## Viewing the documentation

The documentation can be found at https://dpvreony.github.io/docfx-mermaidjs/

## Contributing to the code

See the [contribution guidelines](CONTRIBUTING.md).
