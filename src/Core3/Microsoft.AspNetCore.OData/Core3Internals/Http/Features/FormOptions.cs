// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NETCORE3x

using System.IO;
using Microsoft.AspNetCore.WebUtilities;

namespace Microsoft.AspNetCore.Http.Features
{
    internal class FormOptionsEx
    {
        internal static readonly FormOptions Default = new FormOptions();
	}
}

#endif