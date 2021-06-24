
using EmailMictoservice.DTO;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmailMictoservice {
    public class EmailService : IConsumer<EmailDTO> {

        private readonly MailjetClient client;
        private readonly string emailSender;
        public EmailService() {
            var publicKey = "26ec68c592d70fe330d0d4e7a538f497";
            var privateKey = "e5e971781eeb3fb7f17207cb677e111b";
            var email = "dedpanas1999@gmail.com";
            emailSender = email;
            client = new MailjetClient(
                publicKey,
                privateKey
            );
        }

        public async Task Consume(ConsumeContext<EmailDTO> context) {
            var obj = context.Message;
            List<string> emails = await GetEmailReceivers(obj.ToEveryone);
            await SendEmail(obj.Header,obj.Text,emails);
        }

        public async Task SendEmail(string header,string text, List<string> emails) {

            // construct your email with builder
            if (emails.Count < 1)
                return;

            var request = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(emailSender))
                .WithSubject(header)
                .WithHtmlPart(text);


            foreach(var mail in emails) {
                request.WithTo(new SendContact(mail));
            }
            var buided = request.Build();

            await client.SendTransactionalEmailAsync(buided);
        }

        public async Task<List<string>> GetEmailReceivers(bool toEveryone) {

            using (HttpClient client = new HttpClient()) {
                try {
                    HttpResponseMessage response = await client.GetAsync($"https://localhost:4001/api/users?toEveryone={toEveryone}");
                    if (response.StatusCode == HttpStatusCode.OK) {
                        HttpContent responseContent = response.Content;
                        string json = await responseContent.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<string>>(json);
                    }
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
                return new List<string>();

            }
        }
    }
}

