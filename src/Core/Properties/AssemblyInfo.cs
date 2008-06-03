using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("Ninject Core Library")]
[assembly: Guid("b7f09600-5169-4c22-892b-b82cf3c1c22c")]

#if !NO_PARTIAL_TRUST
[assembly: AllowPartiallyTrustedCallers]
#endif