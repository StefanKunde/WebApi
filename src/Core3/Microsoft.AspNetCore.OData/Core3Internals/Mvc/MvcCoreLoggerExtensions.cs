// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NETCORE3x

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Mvc
{
	using ActionConstraints;

	internal static class MvcCoreLoggerExtensions
    {
		private static readonly Action<ILogger, string, Exception> _ambiguousActions;
		private static readonly Action<ILogger, string, string, IActionConstraint, Exception> _constraintMismatch;
        private static readonly Action<ILogger, string[], Exception> _noActionsMatched;

		static MvcCoreLoggerExtensions()
		{
			_ambiguousActions = LoggerMessage.Define<string>(
				LogLevel.Error,
				new EventId(1, "AmbiguousActions"),
				"Request matched multiple actions resulting in ambiguity. Matching actions: {AmbiguousActions}");

			_constraintMismatch = LoggerMessage.Define<string, string, IActionConstraint>(
				LogLevel.Debug,
				new EventId(2, "ConstraintMismatch"),
				"Action '{ActionName}' with id '{ActionId}' did not match the constraint '{ActionConstraint}'");

			_noActionsMatched = LoggerMessage.Define<string[]>(
				LogLevel.Debug,
				new EventId(3, "NoActionsMatched"),
				"No actions matched the current request. Route values: {RouteValues}");
        }

		public static void AmbiguousActions(this ILogger logger, string actionNames)
		{
			_ambiguousActions(logger, actionNames, null);
		}

		public static void ConstraintMismatch(
			this ILogger logger,
			string actionName,
			string actionId,
			IActionConstraint actionConstraint)
		{
			_constraintMismatch(logger, actionName, actionId, actionConstraint, null);
		}

        public static void NoActionsMatched(this ILogger logger, IDictionary<string, object> routeValueDictionary)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                string[] routeValues = null;
                if (routeValueDictionary != null)
                {
                    routeValues = routeValueDictionary
                        .Select(pair => pair.Key + "=" + Convert.ToString(pair.Value))
                        .ToArray();
                }
                _noActionsMatched(logger, routeValues, null);
            }
        }
	}
}

#endif