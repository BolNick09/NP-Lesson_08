using System.Net;
using System.Net.Mail;


namespace SendMail_SMTP
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            MailAddress from = new MailAddress("test.smtp2@inbox.ru");
            List<MailAddress> to =
            [
                new MailAddress("test.smtp2@inbox.ru"),
                new MailAddress("BolNick09RUS@gmail.com"),
                new MailAddress("Bolshakov199709@rambler.ru"),
            ];

            foreach (MailAddress address in to)
            { 
                MailMessage message = new MailMessage(from, address);
                //message.CC.Add(new MailAddress("test.smtp2@inbox.ru"));копия
                message.Subject = $"Тема письма для {TrimAddress(address.Address)}";
                message.Body = @"
                            <html>
                            <body>
                                <h1>Заголовок текста письма</h1>
                                <p>текст письма</p>
                            </body>
                            </html>";
                message.IsBodyHtml = true;
                Attachment attachment = new Attachment("cage1.csv", "text/csv");
                message.Attachments.Add(attachment);

                using SmtpClient client = new SmtpClient("smtp.mail.ru");
                client.Credentials = new NetworkCredential("test.smtp2@inbox.ru", "0DE6LbSMnLCfAkzs1xpt");
                client.EnableSsl = true;

                await client.SendMailAsync(message);
                Console.WriteLine("Письмо отправлено");
            }


        }

        public static string TrimAddress(string address)
        {
            return address.Substring(0, address.IndexOf('@'));
        }
    }
}
