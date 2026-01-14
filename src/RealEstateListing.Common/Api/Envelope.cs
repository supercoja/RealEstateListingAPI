namespace RealEstateListing.Common.Api;

    public class Envelope<T>
    {
        public T Result { get; }

        public List<string> ErrorMessages { get; }

        public DateTime TimeGenerated { get; }

        public bool IsSuccess => ErrorMessages == null || !ErrorMessages.Any();
        protected Envelope(T result, List<string> errorMessages)
        {
            Result = result;
            ErrorMessages = errorMessages;
            TimeGenerated = DateTime.UtcNow;
        }

        public static Envelope<T> Ok(T result)
        {
            return new Envelope<T>(result, null);
        }

        public static Envelope<T> Error(List<string> errorMessages)
        {
            return new Envelope<T>(default(T), errorMessages ?? new List<string>());
        }

        public static Envelope<T> Error(string errorMessage)
        {
            return new Envelope<T>(default(T), new List<string> { errorMessage });
        }
    }