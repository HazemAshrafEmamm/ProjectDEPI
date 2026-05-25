using System.Net;
using System.Net.Mail;

namespace PL.Utilites
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            //logic to send email
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("yuossefezzatmostafa@gmail.com", "mmla jsud voeb ztrs");
                client.Send("yuossefezzatmostafa@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
}
}

