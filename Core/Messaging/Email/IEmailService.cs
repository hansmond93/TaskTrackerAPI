using System.Collections.Generic;
using System.Net.Mail;

namespace Core.Messaging.Email
{
    //Todo: attachment should be part of the Mail
    public abstract class MailBase
    {
        public bool BodyIsFile { get; set; }
        public string Body { get; set; }
        public string BodyPath { get; set; }
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string SenderDisplayName { get; set; }
        public bool IsBodyHtml { get; set; }
        public ICollection<string> To { get; set; }
        public ICollection<string> Bcc { get; set; }
        public ICollection<string> CC { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
    }

    public sealed class Mail : MailBase
    {
        //private string v1;
        //private string v2;
        //private string v3;

        private Mail()
        {
            IsBodyHtml = true;
            To = new List<string>();
            CC = new List<string>();
            Bcc = new List<string>();
            Attachments = new List<Attachment>();
        }

        public Mail(string sender, string subject, params string[] to)
            : this()
        {
            Sender = sender;
            Subject = subject;

            foreach (var rec in to)
                    To.Add(rec);
        }

        //public Mail(string sender, string subject, params string[] to) : this(sender, subject, to)
        //{
        //}

        //public Mail(string v1, string v2, string v3)
        //{
        //    this.v1 = v1;
        //    this.v2 = v2;
        //    this.v3 = v3;
        //}
    }
}