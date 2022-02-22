using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Helper
{
    public enum AuthorizationPolicy
    {
        ADMIN, ADMIN_AND_STAFF, STAFF , AUTHENTICATED_USER, ANY
    }
}
