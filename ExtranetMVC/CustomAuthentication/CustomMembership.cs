using Extranet_EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security;


namespace ExtranetMVC.CustomAuthentication
{
    public class CustomMembership : MembershipProvider
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            bool result = false;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            using (ExtranetDB dbContext = new ExtranetDB())
            {
                var user = (from us in dbContext.Users
                            where string.Compare(username, us.Username, StringComparison.OrdinalIgnoreCase) == 0
                            && string.Compare(password, us.Password, StringComparison.OrdinalIgnoreCase) == 0
                            && us.IsActive == true
                            select us).FirstOrDefault();

                result = (user != null) ? true : false;
            }

            if (!result)
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "DELLORTO.IT"))
                {
                    // validate the credentials
                    result = pc.ValidateCredentials(username, password, ContextOptions.Negotiate);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            CustomMembershipUser selectedUser = null;
            using (ExtranetDB dbContext = new ExtranetDB())
            {
                var user = (from us in dbContext.Users
                            where string.Compare(username, us.Username, StringComparison.OrdinalIgnoreCase) == 0
                            select us).FirstOrDefault();

                if (user != null)
                {
                    selectedUser = new CustomMembershipUser(user);
                }
            }
            if (selectedUser == null)
            {
                using (var context = new PrincipalContext(ContextType.Domain, "DELLORTO.IT"))
                {
                    var usr = UserPrincipal.FindByIdentity(context, username);
                    if (usr != null)
                        selectedUser = new CustomMembershipUser(usr);
                }
            }
            if (selectedUser == null)
            {
                using (var context = new PrincipalContext(ContextType.Domain, "INDIA"))
                {
                    var usr = UserPrincipal.FindByIdentity(context, username);
                    if (usr != null)
                        selectedUser = new CustomMembershipUser(usr);
                }
            }
            return selectedUser;
        }

        public override string GetUserNameByEmail(string email)
        {
            using (ExtranetDB dbContext = new ExtranetDB())
            {
                string username = (from u in dbContext.Users
                                   where string.Compare(email, u.Email) == 0
                                   select u.Username).FirstOrDefault();

                return !string.IsNullOrEmpty(username) ? username : string.Empty;
            }
        }

        #region Overrides of Membership Provider

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                throw new NotImplementedException();
            }

        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        //public new   bool ChangePassword //(string username, string oldPassword, string newPassword) //public override changepassword
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //using (ExtranetDB dbContext = new ExtranetDB())
        //        {
        //            var userp = dbContext.Users.Where(x => x.Username == username).FirstOrDefault();


        //            userp.Password = newPassword;
        //            dbContext.Entry(userp).State = EntityState.Modified;
        //            dbContext.SaveChanges();
        //            return true;
        //        }


        //verificare validità psw nuova

        //if (oldPassword == newPassword)
        //{
        //    return false;
        //}

        //if (PasswordIsValid(newPassword).Length > 0)
        //{
        //    return false;
        //}

        //else
        //{
        //    if (ValidateUser(username, oldPassword))
        //    {
        //        using (ExtranetDB dbContext = new ExtranetDB())
        //        {
        //            var userp = dbContext.Users.Where(x => x.Username == username).FirstOrDefault();


        //            userp.Password = newPassword;
        //            dbContext.Entry(userp).State = EntityState.Modified;
        //            dbContext.SaveChanges();
        //            return true;
        //        }

        //    }
        //}
        //return false;
        //}

        //public string[] PasswordIsValid(string password) //si potrebbe mettere bool in quanto il controllo lo faccio nel controller
        //{
        //    List<string> esito = new List<string>();
        //    //string[] esito= new string[0];
        //    if (password.Length < 8)
        //    {
        //        esito.Add("New password lenght must be 8 or more.");
        //    }
        //    if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript)) //both, lower and upper case
        //    {
        //        esito.Add("New password must contains at least 1 Uppercase character (A-Z) and 1 Lowercase(a-z) character");
        //    }
        //    if (Regex.IsMatch(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript)) //^[A-Z]+$
        //    {
        //        esito.Add("New password must contain at least 1 special character");
        //    }
        //    return esito.ToArray();
        //}

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            using (ExtranetDB dbContext = new ExtranetDB())
            {
                var userp = dbContext.Users.Where(x => x.Username == username).FirstOrDefault();
                userp.Password = newPassword;
                dbContext.Entry(userp).State = EntityState.Modified;
                dbContext.SaveChanges();
                return true;
            }



           
        }

        #endregion
    }
}