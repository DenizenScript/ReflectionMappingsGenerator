Reflections Mapping Generator
-----------------------------

Quick tool developed by Alex Goodwin for the DenizenScript Team, for usage in Denizen to generate updates to the `ReflectionMappingsInfo` file as seen at https://github.com/DenizenScript/Denizen/blob/dev/v1_19/src/main/java/com/denizenscript/denizen/nms/v1_19/ReflectionMappingsInfo.java

Works as a quick-n-dirty command line program that downloads the source mappings from Microsoft's servers and applies them to the file. Presumes the file exists in the same standard format Denizen uses, can either use the file in the run folder with the same name, or specify a filename as the first and only command line argument.

Command line will expect you to input the version to use before it begins processing (for example, `1.19.1`).

### Building

Just open the `.sln` in Visual Studio 2022 and hit `Build Solution`.

### License

The MIT License (MIT)

Copyright (c) 2022 The Denizen Script Team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
