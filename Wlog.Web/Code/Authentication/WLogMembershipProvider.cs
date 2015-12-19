using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Wlog.Web.Code.Classes;
using Wlog.Web.Code.Helpers;

namespace Wlog.Web.Code.Authentication
{
    public class WLogMembershipProvider:MembershipProvider
    {
     

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

                using (UnitOfWork uow = new UnitOfWork())
                {

                    UserEntity user = new UserEntity();
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
                        uow.SaveOrUpdate(user);


                        if (user.Id < 1)
                            status = MembershipCreateStatus.UserRejected;
                        else
                        {
                            uow.Commit();
                            status = MembershipCreateStatus.Success;
                        }
                    }
                    catch (Exception e)
                    {
                        status = MembershipCreateStatus.ProviderError;
                        
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
            UserEntity usr = UserHelper.GetByUsername(username);
            if (usr == null)
                return null;
            return GetMembershipUserFromUser(usr);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            UserEntity usr = UserHelper.GetByUsername(providerUserKey as string);
            return GetMembershipUserFromUser(usr);
        }

        public override string GetUserNameByEmail(string email)
        {
            string result = "";
            UserEntity usr = UserHelper.GetByEmail(email);
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
            UserEntity usr = UserHelper.GetByUsername(user.UserName);
            if (usr != null)
            {
                usr.Email = user.Email;
                usr.LastActivityDate = DateTime.Now;
                UserHelper.UpdateUser(usr);
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            UserEntity usr = UserHelper.GetByUsername(username);

            if (usr == null)
                return isValid;
            string pswEncode =  EncodePassword(password);
            if (pswEncode == usr.Password)
            {
                isValid = true;
                usr.LastLoginDate = DateTime.Now;
                UserHelper.UpdateUser(usr);
            }

            return isValid;
        }


        private string EncodePassword(string password)
        {

            return UserHelper.EncodePassword(password);
        }

        /// <summary>
        /// Return the MembershipUser from User Entity
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        private MembershipUser GetMembershipUserFromUser(UserEntity usr)
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