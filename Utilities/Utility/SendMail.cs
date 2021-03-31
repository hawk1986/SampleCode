using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Utilities.Extensions;

namespace Utilities.Utility
{
    public class SendMail
    {
        #region readonly fields
        readonly string _fromName;
        readonly string _fromAddress;
        readonly string[] _toAddress;
        readonly string[] _ccAddress;
        readonly string _smtpServer;
        readonly string _smtpUser;
        readonly string _smtpPassword;
        readonly int _smtpPort;
        readonly string _subject;
        readonly string _body;
        readonly string[] _filePath;
        readonly bool _htmlFormat;
        readonly string _mailEncoding = "utf-8";
        readonly MailPriority _priority;
        #endregion

        #region constructor
        /// <summary>
        /// 建立發送信件
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public SendMail(string fromAddress, string[] toAddress, string subject, string body)
            : this(null, fromAddress, toAddress, null, subject, body, null, null, 0, true, MailPriority.Normal)
        {
        }

        /// <summary>
        /// 建立發送信件
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="filePath"></param>
        public SendMail(string fromAddress, string[] toAddress, string subject, string body, string filePath)
            : this(null, fromAddress, toAddress, null, subject, body, new string[] { filePath }, null, 0, true, MailPriority.Normal)
        {
        }

        /// <summary>
        /// 建立發送信件
        /// </summary>
        /// <param name="fromName"></param>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public SendMail(string fromName, string fromAddress, string[] toAddress, string subject, string body)
            : this(fromName, fromAddress, toAddress, null, subject, body, null, null, 0, true, MailPriority.Normal)
        {
        }

        /// <summary>
        /// 建立發送信件
        /// </summary>
        /// <param name="fromName"></param>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="htmlFormat"></param>
        /// <param name="priority"></param>
        public SendMail(string fromName, string fromAddress, string[] toAddress, string subject, string body, bool htmlFormat, MailPriority priority)
            : this(fromName, fromAddress, toAddress, null, subject, body, null, null, 0, htmlFormat, priority)
        {
        }

        /// <summary>
        /// 建立發送信件
        /// </summary>
        /// <param name="fromName"></param>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="ccAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="filePath"></param>
        /// <param name="smtpServer"></param>
        /// <param name="smtpPort"></param>
        public SendMail(string fromName, string fromAddress, string[] toAddress, string[] ccAddress, string subject, string body, string[] filePath, string smtpServer, int smtpPort)
            : this(fromName, fromAddress, toAddress, ccAddress, subject, body, filePath, smtpServer, smtpPort, true, MailPriority.Normal)
        {
        }

        /// <summary>
        /// 建立發送信件
        /// </summary>
        /// <param name="fromName"></param>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="ccAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="filePath"></param>
        /// <param name="smtpServer"></param>
        /// <param name="smtpPort"></param>
        /// <param name="htmlFormat"></param>
        /// <param name="priority"></param>
        public SendMail(string fromName, string fromAddress, string[] toAddress, string[] ccAddress, string subject, string body, string[] filePath, string smtpServer, int smtpPort, bool htmlFormat, MailPriority priority)
        {
            string smtpTest = Tools.GetConfigValue("SmtpTest", string.Empty);
            bool runTest = !string.IsNullOrEmpty(smtpTest);
            _fromName = string.IsNullOrEmpty(fromName) ? fromAddress : fromName;
            _fromAddress = fromAddress;
            _toAddress = runTest ? smtpTest.Split(',') : toAddress;
            _ccAddress = runTest ? smtpTest.Split(',') : ccAddress;
            _subject = string.IsNullOrEmpty(subject) ? "郵件主旨" : subject;
            if (runTest)
            {
                StringBuilder allMailAddress = new StringBuilder(512);
                allMailAddress.Append("this is To address:<br />");
                if (toAddress != null && toAddress.Length > 0)
                {
                    foreach (string mail in toAddress)
                        allMailAddress.Append(mail).Append("<br />");
                }
                allMailAddress.Append("this is Cc address:<br />");
                if (ccAddress != null && ccAddress.Length > 0)
                {
                    foreach (string mail in ccAddress)
                        allMailAddress.Append(mail).Append("<br />");
                }
                body = string.Concat(body.NullToEmpty(), "<br /><br />", allMailAddress.ToString());
            }
            _body = string.IsNullOrEmpty(body) ? "郵件內容" : body;
            _filePath = filePath;
            _smtpServer = string.IsNullOrEmpty(smtpServer) ? Tools.GetConfigValue("SmtpServer", "localhost") : smtpServer;
            _smtpPort = smtpPort > 0 ? smtpPort : Tools.GetConfigValue("SmtpPort", 25);
            _smtpUser = Tools.GetConfigValue("SmtpUser", "localhost");
            _smtpPassword = Tools.GetConfigValue("SmtpPassword", "localhost");
            _htmlFormat = htmlFormat;
            _priority = priority;
        }

        /// <summary>
        /// 建立發送信件
        /// </summary>
        /// <param name="fromName"></param>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="ccAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public SendMail(string fromName, string fromAddress, string[] toAddress, string[] ccAddress, string subject, string body)
            : this(fromName, fromAddress, toAddress, ccAddress, subject, body, null, null, 0, true, MailPriority.Normal)
        {
        }
        #endregion

        #region public method
        /// <summary>
        /// 發送 mail
        /// </summary>
        /// <returns></returns>
        public bool Send()
        {
            var message = new MailMessage();
            message.From = new MailAddress(_fromAddress, _fromName, Encoding.GetEncoding(_mailEncoding));

            //to mail Address
            foreach (string toAddress in _toAddress)
            {
                message.To.Add(new MailAddress(toAddress));
            }

            //cc mail Address
            if (_ccAddress != null)
            {
                foreach (string ccAddress in _ccAddress)
                {
                    message.CC.Add(new MailAddress(ccAddress));
                }
            }

            //Subject
            message.Subject = _subject;
            message.SubjectEncoding = Encoding.GetEncoding(_mailEncoding);

            //Body
            message.Body = _body;
            message.BodyEncoding = Encoding.GetEncoding(_mailEncoding);

            message.IsBodyHtml = _htmlFormat;
            message.Priority = _priority;

            //設定附件檔案(Attachment)
            if (_filePath != null && _filePath.Length > 0)
            {
                foreach (string filePath in _filePath)
                {
                    var attachment = new Attachment(filePath);
                    attachment.Name = Path.GetFileName(filePath);
                    attachment.NameEncoding = Encoding.GetEncoding(_mailEncoding);
                    attachment.TransferEncoding = TransferEncoding.Base64;
                    attachment.ContentDisposition.Inline = true;
                    attachment.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                    message.Attachments.Add(attachment);
                }
            }

            //send mail
            var smtpClient = new SmtpClient(_smtpServer, _smtpPort);
            smtpClient.EnableSsl = Tools.GetConfigValue("SmtpSSL", false);
            smtpClient.UseDefaultCredentials = Tools.GetConfigValue("SmtpUseDefaultCredentials", false);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Timeout = Tools.GetConfigValue("SmtpTimeout", 15000);
            // 由 config 決定是否需要帳密登入發送信件
            if (Tools.GetConfigValue("IsCredential", false))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
            }

            try
            {
                smtpClient.Send(message);

                return true;
            }
            finally
            {
                message.Dispose();
                smtpClient.Dispose();
            }
        }
        #endregion
    }
}
