using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("Ninject (Unit Tests)")]
[assembly: Guid("49c1f3df-f7bd-44b6-9c92-a59d7516e998")]

#if !NO_PARTIAL_TRUST
[assembly: AllowPartiallyTrustedCallers]
#endif