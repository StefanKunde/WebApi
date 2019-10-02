// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace AspNetCore3ODataSample.Web.Controllers
{
	using System.Collections.Generic;
	using Microsoft.AspNet.OData;
	using Microsoft.AspNetCore.Mvc;
	using Models;

	public class PeopleController : ODataController
    {
        [EnableQuery]
        public IActionResult Get([FromODataUri]string keyFirstName, [FromODataUri]string keyLastName)
        {
            Person m = new Person
            {
                FirstName = keyFirstName,
                LastName = keyLastName,
                DynamicProperties = new Dictionary<string, object>
                {
                    { "abc", "abcValue" }
                },
                MyLevel = Level.High
            };

            return Ok(m);
        }

        [EnableQuery]
        public IActionResult Post([FromBody]Person person)
        {
            return Created(person);
        }
    }
}
