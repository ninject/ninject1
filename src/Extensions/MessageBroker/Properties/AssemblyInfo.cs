using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("Ninject Message Broker Extension")]
[assembly: Guid("5614967b-6e86-4b1e-95b3-a1b82308eecb")]

#if !NO_PARTIAL_TRUST
[assembly: AllowPartiallyTrustedCallers]
#endif