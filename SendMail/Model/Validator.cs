using System.ComponentModel.DataAnnotations;

namespace SendMail.Model
{
    public static class Validator
    {
        public static bool IsValid(this Mail mail)
        {
            var emailAddressAttribute = new EmailAddressAttribute();
            if (string.IsNullOrEmpty(mail.to) || !emailAddressAttribute.IsValid(mail.to))
            {
                return false;
            }

            if (string.IsNullOrEmpty(mail.from) || !emailAddressAttribute.IsValid(mail.from))
            {
                return false;
            }

            if (string.IsNullOrEmpty(mail.title))
            {
                return false;
            }

            if (string.IsNullOrEmpty(mail.content))
            {
                return false;
            }

            if (string.IsNullOrEmpty(mail.host))
            {
                return false;
            }

            if (string.IsNullOrEmpty(mail.user_name))
            {
                return false;
            }

            if (string.IsNullOrEmpty(mail.password))
            {
                return false;
            }

            return true;
        }
    }
}
