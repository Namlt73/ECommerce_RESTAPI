namespace ApiEcommerce.Dtos.Handles
{
    public class SuccessResponse : ApplicationResponse
    {
        public SuccessResponse() : base(true)
        {
        }

        public SuccessResponse(string message) : base(true, message)
        {
        }
    }
}
