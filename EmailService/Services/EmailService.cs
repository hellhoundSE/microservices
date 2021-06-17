
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMictoservice.Controllers {
    public class EmailService {

        private readonly MailjetClient _client;
        private readonly string emailSender;
        public EmailService(string publicKey, string privateKey, string email) {
            emailSender = email;
            _client = new MailjetClient(
                Environment.GetEnvironmentVariable(publicKey),
                Environment.GetEnvironmentVariable(privateKey));
        }

        public async Task<bool> SendEmail(string text, List<string> emails) {

            MailjetRequest request = new MailjetRequest {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray
             {
                    new JObject {
                      {
                       "From", new JObject {
                        {"Email", emailSender},
                        {"Name", "ShronApka"}}
                      },
                      {"To", new JArray {
                          emails.Select(email => new JObject(){
                            {"Email",$"{email}"}
                          }) }
                        },
                      {
                       "Subject", "Twoje konto ShronApki"
                      },
                      {"HTMLPart", text}
                    }
             });
            MailjetResponse response = await _client.PostAsync(request);

            if(response.IsSuccessStatusCode)
                return true;

            return false;
        }

        internal async Task<List<string>> GetEmailReceivers(bool toEveryone) {

            //TODO
            //Get all admins / Get all users and return their emails

            return null;
        }
    }

}

