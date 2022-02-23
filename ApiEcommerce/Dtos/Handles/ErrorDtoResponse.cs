namespace ApiEcommerce.Dtos.Handles
{
    public class ErrorDtoResponse : ApplicationResponse
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorDtoResponse() : base(false)
        {
        }

        public ErrorDtoResponse(string message) : base(false, message)
        {
        }
    }
}
