using Azure.Messaging.ServiceBus;
using SendMail.Model;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace SendMail
{
    public class Worker : IHostedService
    {
        private readonly ILogger<Worker> _logger;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Running {time}", DateTimeOffset.Now);
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            var queue = Environment.GetEnvironmentVariable("QUEUE");
            var client = new ServiceBusClient(connectionString);

            var options = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1
            };

            ServiceBusProcessor processor = client.CreateProcessor(queue, options);

            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            async Task MessageHandler(ProcessMessageEventArgs args)
            {
                ProcessMessage(args.Message);
                await args.CompleteMessageAsync(args.Message);
            }

            async Task ErrorHandler(ProcessErrorEventArgs args)
            {
                _logger.LogError(args.Exception.ToString());
            }

            return processor.StartProcessingAsync();
        }

        private void ProcessMessage(ServiceBusReceivedMessage receivedMessage)
        {
            var content = Encoding.UTF8.GetString(receivedMessage.Body);
            var mail = JsonSerializer.Deserialize<Mail>(content);
            if (mail == null || !mail.IsValid())
            {
                return;
            }

            MailMessage mensagemEmail = new MailMessage(mail.from, mail.to, mail.title, mail.content);
            mensagemEmail.IsBodyHtml = mail.isHtml;

            SmtpClient smptClient = new SmtpClient();
            smptClient.Port = mail.port;
            smptClient.Host = mail.host;
            smptClient.EnableSsl = true;
            smptClient.Credentials = new NetworkCredential(mail.user_name, mail.password);
            smptClient.Send(mensagemEmail);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Connection closed at: {time}", DateTimeOffset.Now);
        }

    }
}