using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Wlog.Models;
using Wlog.Web.Code.Repository;

namespace Wlog.Web.Code.Authentication
{
    public class WLogMembershipProvider:MembershipProvider
    {
        private static UserRepository _UserRep = new UserRepository();

        public override string ApplicationName
        {
            get
            {
                return "WLog";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);
            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser u = GetUser(username, false);

            if (u == null)
            {
                DateTime createDate = DateTime.Now;

                using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        UserEntry user = new UserEntry();
                        user.Username = username;
                        user.Password = EncodePassword(password);
                        user.Email = email;
                        user.IsAdmin = false;
                        user.PasswordQuestion = passwordQuestion;
                        user.PasswordAnswer = passwordAnswer;//EncodePassword(passwordAnswer);
                        user.IsApproved = isApproved;
                        user.CreationDate = createDate;
                        user.LastPasswordChangedDate = createDate;
                        user.LastActivityDate = createDate;
                        user.IsLockedOut = false;
                        user.LastLockedOutDate = createDate;
                        
                        try
                        {
                            int retId = (int)session.Save(user);

                            transaction.Commit();
                            if ((retId < 1))
                                status = MembershipCreateStatus.UserRejected;
                            else
                                status = MembershipCreateStatus.Success;
                        }
                        catch (Exception e)
                        {
                            status = MembershipCreateStatus.ProviderError;
                        }
                    }
                }

                return GetUser(username, false);
            }
            else
                status = MembershipCreateStatus.DuplicateUserName;
            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
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

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            UserRepository repo = new UserRepository();
            UserEntry u = repo.GetByUsername(username);
            if (u == null)
                return null;
            return GetMembershipUserFromUser(u);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            UserEntry usr = _UserRep.GetById((int)providerUserKey);
            return GetMembershipUserFromUser(usr);
        }

        public override string GetUserNameByEmail(string email)
        {
            string result = "";
            UserEntry usr = _UserRep.GetByEmail(email);
            if (usr != null) result = usr.Username;
            return result;
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 10; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
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
            UserEntry usr = _UserRep.GetByUsername(user.UserName);
            if (usr != null)
            {
                usr.Email = user.Email;
                usr.LastActivityDate = DateTime.Now;
                _UserRep.UpdateUser(usr);
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            UserEntry usr = _UserRep.GetByUsername(username);

            if (usr == null)
                return isValid;
            string pswEncode =  EncodePassword(password);
            if (pswEncode == usr.Password)
            {
                isValid = true;
                usr.LastLoginDate = DateTime.Now;
                _UserRep.UpdateUser(usr);
            }

            return isValid;
        }


        private string EncodePassword(string password)
        {
            //TODO:Set Security encode
            using (SHA1CryptoServiceProvider Sha = new SHA1CryptoServiceProvider())
            {
                return Convert.ToBase64String( Sha.ComputeHash(Encoding.ASCII.GetBytes(password)));
            }
        }

        /// <summary>
        /// Return the MembershipUser from User Entity
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        private MembershipUser GetMembershipUserFromUser(UserEntry usr)
        {
            MembershipUser u = new MembershipUser("WLogMembershipProvider",
                                                  usr.Username,
                                                  usr.Id,
                                                  usr.Email,
                                                  usr.PasswordQuestion,
                                                  "",
                                                  usr.IsApproved,
                                                  usr.IsLockedOut,
                                                  usr.CreationDate,
                                                  usr.LastLoginDate,
                                                  usr.LastActivityDate,
                                                  usr.LastPasswordChangedDate,
                                                  usr.LastLockedOutDate);

            return u;
        }
    }
}