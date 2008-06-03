using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("Ninject MonoRail Integration Library")]
[assembly: Guid("a63fd7f1-c05a-484c-99f2-a94f6fb81ed2")]

#if !NO_PARTIAL_TRUST
[assembly: AllowPartiallyTrustedCallers]
#endif