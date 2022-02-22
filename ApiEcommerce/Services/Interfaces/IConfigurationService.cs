using ApiEcommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface IConfigurationService
    {
        string GetAdminUserName();
        string GetAdminEmail();
        string GetAdminRoleName();
        string GetAdminPassword();
        int GetDefaultPage();
        int GetDefaultPageSize();
        string GetManageProductPolicyName();
        string GetCreateCommentPolicyName();
        string GetDeleteCommentPolicyName();
        string GetWhoIsAllowedToManageProducts();
        string GetWhoIsAllowedToCreateComments();
        AuthorizationPolicy GetWhoIsAllowedToDeleteComments();
        AuthorizationPolicy GetWhoIsAllowedToUpdateComments();
        string GetStandardUserRoleName();
        string GetAdminFirstName();
        string GetAdminLastName();

        string GetUpdateCommentPolicyName();
    }
}
