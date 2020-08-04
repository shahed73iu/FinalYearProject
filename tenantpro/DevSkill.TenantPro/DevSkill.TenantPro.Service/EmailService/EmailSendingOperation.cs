using Autofac;
using DevSkill.TenantPro.Tenantship.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Service.EmailService
{
    public class EmailSendingOperation
    {
        private ITenantService _tenantService;
        public IConfiguration Configuration { get; set; }
        public EmailSendingOperation(IConfiguration config)
        {
            _tenantService = Worker.AutofacContainer.Resolve<ITenantService>();
            Configuration = config;
        }
        public void SendEmail()
        {
            var tenantlist = _tenantService.GetExpiratedTenants();
            DateTime date = DateTime.Today.AddDays(30);
            if (tenantlist != null)
            {
                var msglist = new List<MailMessage>();
                foreach (var item in tenantlist)
                {
                    var msg = new MailMessage();
                    msg.Body = "Hello sir,Your contract has only 1 month validation.Please contract authority to renew";
                    msg.Subject = "Warning";
                    msg.SenderName = "Rakib Hasan";
                    msg.Sender = "nobelrakib03@gmail.com";
                    msg.Receiver = item.Email;
                    foreach (var contractperson in item.ContactPersons)
                    {
                        var msgtocontractperson = new MailMessage();
                        msgtocontractperson.Body = "Hello sir,Your contract has only 1 month validation.Please contract authority to renew";
                        msgtocontractperson.Subject = "Warning";
                        msgtocontractperson.SenderName = "Rakib Hasan";
                        msgtocontractperson.Sender = "nobelrakib03@gmail.com";
                        msgtocontractperson.Receiver = contractperson.Email;
                        msglist.Add(msgtocontractperson);
                    }
                    msglist.Add(msg);
                }

                using (var mailsender = new MailSender(Configuration))
                {
                    mailsender.Send(msglist);
                }

            }
        }
    }
}
