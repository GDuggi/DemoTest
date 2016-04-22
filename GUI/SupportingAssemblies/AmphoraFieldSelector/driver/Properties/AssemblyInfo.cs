using System.Reflection;
using System.Runtime.InteropServices;


[assembly:AssemblyTitle("driver")]
[assembly:AssemblyProduct("driver")]
[assembly:AssemblyDescription("description of driver.")]
[assembly:AssemblyCompany("Amphora Inc.")]
[assembly:AssemblyCopyright("Copyright © 2015, Amphora Inc.")]
#if DEBUG
[assembly:AssemblyConfiguration("Debug version")]
#else
[assembly:AssemblyConfiguration("Release version")]
#endif
[assembly:ComVisible(false)]

[assembly:AssemblyVersion("1.0.0.0")]
[assembly:AssemblyFileVersion("1.0.0.0")]
[assembly:AssemblyInformationalVersion("1.0.0.0")]

