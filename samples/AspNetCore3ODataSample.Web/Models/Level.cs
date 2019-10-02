// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace AspNetCore3ODataSample.Web.Models
{
	using System.Runtime.Serialization;

	[DataContract(Name = "level")]
    public enum Level
    {
        [EnumMember(Value = "low")]
        Low,

        [EnumMember(Value = "medium")]
        Medium,

        [EnumMember(Value = "veryhigh")]
        High
    }
}
