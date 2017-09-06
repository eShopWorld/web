﻿namespace DevOpsFlex.Telemetry.Web
{
    using Core;

    /// <summary>
    /// BigBrother Web event fire when a reponse has already started (headers had been sent) when the exception handling method is called.
    /// </summary>
    public class ResponseAlreadyStartedExceptionEvent : BbExceptionEvent
    {
        public string Reason => $"This Exception was thrown after the API response had already started, so the {nameof(BigBrotherExceptionMiddleware)} returned imediatly and didn't attempt to populate the response";
    }
}
