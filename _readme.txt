Carbon Copy README
==================

NOTES:
- Please read CarbonCopy\Resources\licence.txt for the licence under which this software and source code
  are distributed.
- When we update the version, remember to read _version.txt for places where we need to
  update the version number!


-----


This is where we'll be putting useful info like build information, dependencies, etc.

Libraries (generally DLLs) that need to be packaged with the solution should go in a
'_Libs' directory, to be located under 'Solution Items' in the solution explorer,
and under the directory in which the .sln file resides on the file system.

Carbon Copy is being developed in Visual Studio 2015, and therefore it's
recommended that you use VS2015 if you want to modify its code.  However I'd be interested
to know how well anyone who want to use something like Eclipse gets on (no doubt you'll
need to use that if you want to get this working on Linux with Mono).

I'm trying to keep all functionality in this solution to using version 2.0 of the .NET
framework; hopefully, that'll mean Mono does implement the vast majority, if not all, of
the functionality being used.  If you spot something using features introduced after 2.0,
please inform the developers.  All projects in this solution's 'Target Framework' should
be '.NET Framework 2.0' or earlier.

- Jez
