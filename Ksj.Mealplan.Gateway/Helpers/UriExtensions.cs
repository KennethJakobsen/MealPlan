using System;

namespace Ksj.Mealplan.Gateway.Helpers
{
    public static class UriExtensions
    {
        public static Uri AppendAction(this Uri serviceUri, Uri actionUri)
        {
            if (!serviceUri.IsAbsoluteUri)
                throw new ArgumentException("Uri must be an absolute uri.", nameof(serviceUri));

            if (actionUri.IsAbsoluteUri)
                throw new ArgumentException("Action uri must be a relative uri", nameof(actionUri));

            var splitAction = actionUri.ToString().Split('?');

            var uriBuilder = new UriBuilder(serviceUri);

            if (splitAction.Length == 2)
            {
                uriBuilder.Path += splitAction[0];
                uriBuilder.Query = splitAction[1];
            }
            else
            {
                uriBuilder.Path += actionUri;
            }

            return uriBuilder.Uri;
        }

        public static Uri ExtractAction(this Uri requestUri, string delimiter)
        {
            if (!requestUri.IsAbsoluteUri)
                throw new ArgumentException("Uri must be an absolute uri.", nameof(requestUri));

            if (string.IsNullOrWhiteSpace(delimiter))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(delimiter));

            var splitUri = requestUri.AbsoluteUri.Split(new []{delimiter}, StringSplitOptions.RemoveEmptyEntries);

            if(splitUri.Length != 2)
                throw new ArgumentException("Unable to find action uri part.", nameof(requestUri));

            return new Uri(splitUri[1], UriKind.Relative);
        }
    }
}