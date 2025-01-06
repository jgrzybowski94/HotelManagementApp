namespace HotelManagement.Exceptions
{
    public class BadDateFormatException : Exception
    {
        private const string DefaultMessage = "The date provided is not in the correct format.";
        public BadDateFormatException() : base(DefaultMessage)
        {
        }
    }
}
