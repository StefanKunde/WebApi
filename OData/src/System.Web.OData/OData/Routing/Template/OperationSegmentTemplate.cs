﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Properties;
using System.Web.OData.Routing.Conventions;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Edm;

namespace System.Web.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that can match a <see cref="OperationSegment"/>.
    /// </summary>
    public class OperationSegmentTemplate : ODataPathSegmentTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationSegmentTemplate"/> class.
        /// </summary>
        /// <param name="segment">The operation segment</param>
        public OperationSegmentTemplate(OperationSegment segment)
        {
            if (segment == null)
            {
                throw Error.ArgumentNull("segment");
            }

            Segment = segment;

            IEdmOperation operation = Segment.Operations.First();
            if (operation.IsFunction())
            {
                ParameterMappings = RoutingConventionHelpers.BuildParameterMappings(segment.Parameters, operation.FullName());
            }
        }

        /// <summary>
        /// Gets or sets the operation segment.
        /// </summary>
        public OperationSegment Segment { get; private set; }

        /// <summary>
        /// Gets the dictionary representing the mappings from the parameter names in the current function segment to the 
        /// parameter names in route data.
        /// </summary>
        public IDictionary<string, string> ParameterMappings { get; private set; }

        /// <inheritdoc/>
        public override bool TryMatch(ODataPathSegment pathSegment, IDictionary<string, object> values)
        {
            OperationSegment other = pathSegment as OperationSegment;
            if (other == null)
            {
                return false;
            }

            IEdmOperation operation = Segment.Operations.First();
            IEdmOperation otherOperation = other.Operations.First();

            if (operation.IsAction() && otherOperation.IsAction())
            {
                return operation == otherOperation;
            }
            else if (operation.IsFunction() && otherOperation.IsFunction())
            {
                if (operation.FullName() != otherOperation.FullName())
                {
                    return false;
                }

                IDictionary<string, object> parameterValues = other.Parameters.ToDictionary(e => e.Name,
                    e => ODataParameterHelper.TranslateNode(e.Value));
                if (RoutingConventionHelpers.TryMatch(ParameterMappings, parameterValues, values))
                {
                    foreach (var operationSegmentParameter in other.Parameters)
                    {
                        string name = operationSegmentParameter.Name;
                        object value = parameterValues[name];

                        RoutingConventionHelpers.AddFunctionParameters((IEdmFunction)otherOperation, name,
                            value, values, values, ParameterMappings);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}