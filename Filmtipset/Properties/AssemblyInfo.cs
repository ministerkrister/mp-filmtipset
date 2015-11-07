using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MediaPortal.Common.Utils;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("MP-Filmtipset")]
[assembly: AssemblyDescription("A Filmtipset.se plugin for MediaPortal")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("MP-Filmtipset")]
[assembly: AssemblyCopyright("GNU General Public License v3")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3f94d4ab-d5e5-49f3-a910-4b10c5dfb195")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

#region MediaPortal Compatibility
// MP 1.6.0 Pre Release
[assembly: CompatibleVersion("1.5.100.0")]
[assembly: UsesSubsystem("MP.SkinEngine")]
[assembly: UsesSubsystem("MP.Plugins.TV")]
[assembly: UsesSubsystem("MP.Externals.HTMLAgilityPack")]
[assembly: UsesSubsystem("MP.Externals.Gentle")]

#endregion
