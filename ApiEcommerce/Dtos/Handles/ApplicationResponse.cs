using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.Handles
{
    public class ApplicationResponse
    {
        public ApplicationResponse(bool success)
        {
            Success = success;
        }

        protected ApplicationResponse(bool success, string message) : this(success)
        {
            if (FullMessages == null)
                FullMessages = new List<string>();
            FullMessages.Add(message);
        }

        public bool Success { get; set; }
        public ICollection<string> FullMessages { get; set; } = new List<string>();
    }
}
