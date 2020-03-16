using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Threading.Tasks;

namespace WSViajes.Comunes
{
    public class Mailer
    {

        public Mailer()
        {

        }

        public void Send(string pCorreoDestino, string pAsunto, string pCuerpoCorreo, string pNombreDestinatario = null)
        {
            try
            {
                string SMTP_SERVER = ConfigurationManager.AppSettings["SMTP_SERVER"];
                int SMTP_SERVER_PORT = Int32.Parse(ConfigurationManager.AppSettings["SMTP_SERVER_PORT"]);
                string SMTP_USER = ConfigurationManager.AppSettings["SMTP_USER"];
                string SMTP_PWD = ConfigurationManager.AppSettings["SMTP_PWD"];
                bool SMTP_SERVER_USE_SSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTP_SERVER_USE_SSL"]);

                var fromAddress = new MailAddress(SMTP_USER, "Avisos FastRun");
                var toAddress = new MailAddress(pCorreoDestino, pNombreDestinatario);

                var smtp = new SmtpClient
                {
                    Host = SMTP_SERVER,
                    Port = SMTP_SERVER_PORT,
                    EnableSsl = SMTP_SERVER_USE_SSL,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,                    
                    Credentials = new NetworkCredential(fromAddress.Address, SMTP_PWD)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = pAsunto,
                    Body = pCuerpoCorreo,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}