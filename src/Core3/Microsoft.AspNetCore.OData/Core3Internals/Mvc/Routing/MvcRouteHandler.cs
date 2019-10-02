// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NETCORE3x

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
#pragma warning disable 1591

namespace Microsoft.AspNetCore.Mvc.Routing
{
	[SuppressMessage("Microsoft.Design", "CA1001")]
	[SuppressMessage("Microsoft.Design", "CA2213")]
    public class MvcRouteHandler : IRouter
    {
        private readonly IActionInvokerFactory _actionInvokerFactory;
        private readonly IActionSelector _actionSelector;
        private readonly ILogger _logger;
        private readonly DiagnosticListener _diagnosticListener;

        public MvcRouteHandler(
            IActionInvokerFactory actionInvokerFactory,
            IActionSelector actionSelector,
            DiagnosticListener diagnosticListener,
            ILoggerFactory loggerFactory)
        {
            _actionInvokerFactory = actionInvokerFactory;
            _actionSelector = actionSelector;
            _diagnosticListener = diagnosticListener;
            _logger = loggerFactory.CreateLogger<MvcRouteHandler>();
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // We return null here because we're not responsible for generating the url, the route is.
            return null;
        }

        public Task RouteAsync(RouteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var candidates = _actionSelector.SelectCandidates(context);
            if (candidates == null || candidates.Count == 0)
            {
				// TODO
                //_logger.NoActionsMatched(context.RouteData.Values);
                return Task.CompletedTask;
            }

            var actionDescriptor = _actionSelector.SelectBestCandidate(context, candidates);
            if (actionDescriptor == null)
            {
				// TODO
                //_logger.NoActionsMatched(context.RouteData.Values);
                return Task.CompletedTask;
            }

            context.Handler = (c) =>
            {
                var routeData = c.GetRouteData();

                var actionContext = new ActionContext(context.HttpContext, routeData, actionDescriptor);
                var invoker = _actionInvokerFactory.CreateInvoker(actionContext);
                if (invoker == null)
                {
					// TODO
					//throw new InvalidOperationException(
					//	Resources.FormatActionInvokerFactory_CouldNotCreateInvoker(
					//		actionDescriptor.DisplayName));
                    throw new InvalidOperationException(actionDescriptor.DisplayName);
                }

                return invoker.InvokeAsync();
            };

            return Task.CompletedTask;
        }
    }
}

#endif