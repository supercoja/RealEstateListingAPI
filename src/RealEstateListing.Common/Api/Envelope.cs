namespace RealEstateListing.Common.Api;

    public static class Envelope
    {
        public static Envelope<T> Ok<T>(T result)
        {
            return new Envelope<T>(result, null);
        }

        public static Envelope<object> Error(string errorMessage)
        {
            return new Envelope<object>(null, new List<string> { errorMessage });
        }
        public static Envelope<object> Error(List<string> errorMessages)
        {
            return new Envelope<object>(null, errorMessages);
        }
    }


    public class Envelope<T>
    {
        public T Result { get; }

        public List<string> ErrorMessages { get; }

        public DateTime TimeGenerated { get; }

        public bool IsSuccess => ErrorMessages == null || !ErrorMessages.Any();

        internal Envelope(T result, List<string> errorMessages)
        {
            Result = result;
            ErrorMessages = errorMessages;
            TimeGenerated = DateTime.UtcNow;
        }
    }