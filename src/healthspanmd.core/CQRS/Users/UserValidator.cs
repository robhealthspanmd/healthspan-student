using healthspanmd.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Users
{
    public class UserValidator
    {
        private bool _IsValid;
        public bool IsValid
        {
            get
            {
                return _IsValid;
            }
        }

        private Dictionary<string, string> _FieldMessages;
        public Dictionary<string, string> FieldMessages
        {
            get
            {
                return this._FieldMessages;
            }
        }

        public void Validate(UserModel userToValidate, IUserQueries userQueries)
        {
            _IsValid = true;
            _FieldMessages = new Dictionary<string, string>();

            // Email: Check for valid email
            if (!RegexUtilities.IsValidEmail(userToValidate.Email))
            {
                _IsValid = false;
                _FieldMessages.Add("email", "Invalid email format");
            }
            else
            {
                // Email: Check for email already in use
                var userWithSameEmail = userQueries.GetUserDetailModel(userToValidate.Email, false);
                if (userWithSameEmail != null)
                {
                    if (userWithSameEmail.UserId != userToValidate.UserId)
                    {
                        _IsValid = false;
                        _FieldMessages.Add("email", "Email is already used");
                    }
                }
            }

            // Phone: Check for phone already in use
            //var usersWithSamePhone = userQueries.GetUserList(new GetUserListQueryFilter
            //{
            //    Phone = userToValidate.Phone
            //});
            //if (usersWithSamePhone.Count > 0)
            //{
            //    foreach (var user in usersWithSamePhone)
            //    {
            //        if (user.UserId != userToValidate.UserId)
            //        {
            //            _IsValid = false;
            //            _FieldMessages.Add("phone", "Phone is already used");
            //            break;
            //        }
            //    }
            //} 
        }
    }
}
