using Extranet_EF;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Web.Security;
using System.Linq;

namespace ExtranetMVC.CustomAuthentication
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Extranet_EF.Roles> Roles { get; set; }
        public string DOI { get; set; }

        #endregion

        public CustomMembershipUser(Users user) : base("CustomMembership", user.Username, user.UserId, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Roles = user.Roles;
            DOI = "n";
            
        }
        public CustomMembershipUser(UserPrincipal user) : base("CustomMembership", user.SamAccountName, 0, user.EmailAddress, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = 0;
            FirstName = user.GivenName;
            LastName = user.Surname;
            Roles = user.GetGroups().Select(g => new Extranet_EF.Roles { RoleId = 0, RoleName = g.DisplayName }).ToList();
            DOI = "y";
        }

        
    }
}