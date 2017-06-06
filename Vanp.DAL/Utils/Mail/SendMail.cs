using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Vanp.DAL.Utils
{
    public static class Mail
    {
        public const string _DISPLAY_NAME = "VANP";
        public const string _FROM_MAIL_ADDRESS = "vanp.ws@gmail.com";
        public const string _FROM_MAIL_PASS = "vanp123456";
        #region SendMail handle
        public  static bool SendMail(string htmlContent, string[] mailTo, string subject, string displayname = _DISPLAY_NAME)
        {
            try
            {
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(_FROM_MAIL_ADDRESS, displayname);
                if (mailTo.Length > 0)
                {
                    mailTo.ToList().ForEach( o => {
                        if (Validation.IsEmail(o)) mm.To.Add(o);
                    });
                }
                mm.Subject = subject;
                mm.IsBodyHtml = true;
                mm.Body = htmlContent.ToString();
                var smtp = new System.Net.Mail.SmtpClient();

                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential(_FROM_MAIL_ADDRESS, _FROM_MAIL_PASS);
                smtp.SendCompleted += MailComplete;
                try
                {
                    smtp.Send(mm);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void MailComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                System.Diagnostics.Debug.Write("Email failed.");
                System.Diagnostics.Debug.Write(e.Error.ToString());
            }
            else if (e.Cancelled)
            {
                System.Diagnostics.Debug.Write("Email was cancelled.");
            }
            else
            {
                //handle sent email
                MailMessage message = (MailMessage)e.UserState;
            }
        }
        #endregion
    }
}