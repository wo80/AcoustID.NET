AcoustID.NET
============

AcoustID fingerprinter and webservice access in C#. See https://acoustid.org/ for information about the AcoustID project.

The original code for this project can be found at https://bitbucket.org/acoustid.

[![Build status](https://ci.appveyor.com/api/projects/status/acmm1n366k8erqnj?svg=true)](https://ci.appveyor.com/project/wo80/acoustid-net)
[![Nuget downloads](http://wo80.bplaced.net/php/badges/nuget-dt-acoustid-net.svg)](https://www.nuget.org/packages/AcoustID.NET)

## Features.
* AcoustID fingerprint calculation.
* AcoustID webservice access:
  Lookup or submit fingerprints using the ```LookupService``` and ```SubmitService``` classes in the ```AcoustID.Web``` namespace.
* *Fingerprinter* example application.

The example application shows how to decode audio files using [NAudio](https://github.com/naudio/NAudio) or [Bass](http://www.un4seen.com/bass.html).

## Configuration.
If you want to use the AcoustID webservice in your application, you need to get an API key from https://acoustid.org/. Once you have the key, register it with AcoustID.NET by setting
```
AcoustID.Configuration.ClientKey = "APIKEY";
```

## License.

LGPL v2.1 (see COPYING.txt at https://bitbucket.org/acoustid/chromaprint/src)
