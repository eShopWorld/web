﻿using System.Diagnostics.CodeAnalysis;

namespace Eshopworld.Web
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// The middleware component that handles exceptions through <see cref="IBigBrother"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BigBrotherExceptionMiddleware
    {
        internal readonly RequestDelegate Next;
        internal readonly IBigBrother Bb;
        private readonly HttpStatusCode _responseHttpStatusCodeOnException;

        /// <summary>
        /// Initializes a new instance of <see cref="BigBrotherExceptionMiddleware"/>.
        /// </summary>
        /// <param name="next">The next <see cref="RequestDelegate"/> in the pipeline.</param>
        /// <param name="bigBrother">The <see cref="IBigBrother"/> that we want to stream exception telemetry to.</param>
        /// <param name="responseHttpStatusCodeOnException">The <see cref="HttpStatusCode"/> we want to return in the response when handling an exception.</param>
        public BigBrotherExceptionMiddleware(RequestDelegate next, IBigBrother bigBrother, HttpStatusCode responseHttpStatusCodeOnException = HttpStatusCode.InternalServerError)
        {

            Next = next;
#if DEBUG
            Bb = bigBrother ?? throw new ArgumentNullException(nameof(bigBrother), $"{nameof(IBigBrother)} isn't registred as a service.");
#else
            Bb = bigBrother;
#endif
            _responseHttpStatusCodeOnException = responseHttpStatusCodeOnException;
        }

        /// <summary>
        /// Middleware entry point, called by the MVC pipeline.
        /// </summary>
        /// <param name="context">The HTTP-specific information about an individual HTTP request.</param>
        /// <returns>[ASYNC] <see cref="Task"/> future promise.</returns>
        public virtual async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        /// <summary>
        /// Handles exceptions by populating the response inside the <see cref="HttpContext"/> depending on
        ///     which type of exception is being handled.
        /// </summary>
        /// <param name="context">The HTTP-specific information about an individual HTTP request.</param>
        /// <param name="exception"></param>
        /// <returns>[ASYNC] <see cref="Task"/> future promise.</returns>
        /// <remarks>
        /// This method will unwrap any <see cref="AggregateException"/> until it finds a regular exception
        ///     and then process the regular exception through the pipeline.
        /// </remarks>
        internal virtual async Task HandleException(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                Bb.Publish(new ResponseAlreadyStartedExceptionEvent { Exception = exception });
                return;
            }

            // Continuously unwrap AggregateException
            while (exception is AggregateException aex)
            {
                if (aex.InnerExceptions.Any())
                    exception = aex.InnerExceptions.First();
                else
                    break;
            }

            string result;

            context.Response.ContentType = "application/json";
            if (exception is BadRequestException badRequest)
            {
                result = JsonConvert.SerializeObject(badRequest.ToResponse());
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                result = JsonConvert.SerializeObject(
                    new ErrorResponse
                    {
#if DEBUG
                        Message = exception.Message,
                        StackTrace = exception.StackTrace
#else
                        Message = "Sorry, but something bad happened!"
#endif
                    });

                context.Response.StatusCode = (int)_responseHttpStatusCodeOnException;
            }

            Bb.Publish(exception.ToExceptionEvent());
            await context.Response.WriteAsync(result);
        }
    }
}