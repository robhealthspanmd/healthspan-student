using System;
using System.Collections.Generic;
using System.Text;

namespace infrastructure.email
{
    public class EmailUtil
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return false;
                }
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
