// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace AspNetCore3ODataSample.Web.Models
{
	using System.Collections.Generic;

	public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        //[NotMapped]
        public IDictionary<string, object> DynamicProperties { get; set; }

        public Level MyLevel { get; set; }
    }
}
