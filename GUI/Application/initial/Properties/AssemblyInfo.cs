using System.Reflection;
using System.Runtime.InteropServices;


[assembly:AssemblyTitle("RiskManager")]
[assembly:AssemblyProduct("RiskManager")]
[assembly:AssemblyDescription("Amphora's application of choice to allow end-users to view market-risk and exposure, given their current holdings.\r\n\r\n"+
    "This application allows multi-window user-defined grouping and filtering of positions.")]
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

