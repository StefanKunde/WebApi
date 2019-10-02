// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

#if NETCORE2x
[assembly: AssemblyProduct("Microsoft OData Web API for ASP.NET Core")]
#elif NETCORE3x
[assembly: AssemblyProduct("Microsoft OData Web API for ASP.NET Core 3")]
#else
[assembly: AssemblyProduct("Microsoft OData Web API for ASP.NET")]
#endif
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
#if !NOT_CLS_COMPLIANT
[assembly: CLSCompliant(true)]
#else
[assembly: CLSCompliant(false)]
#endif
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyMetadata("Serviceable", "True")]
