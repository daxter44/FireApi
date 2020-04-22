using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using FireApi.Database;
using FireApi.Database.Entity;
using FireApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace FireApi.Workers.MQTTSync
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            X509Certificate le = new X509Certificate(@"C:\Users\User\Desktop\LetsEncrypt-CA.pem");

            X509Certificate cacert = new X509Certificate(@"C:\Users\User\Desktop\cacert.crt");
            MqttClient client = new MqttClient("broker.vps.suser.pl", 8883, true, null, null, MqttSslProtocols.TLSv1_2);

            client.MqttMsgPublishReceived += client_recievedMessage;
            client.Connect("pierwszy", "marcin", "63rGSuVkjGkGRMmJ");
            bool isconnecter = client.IsConnected;

            client.Subscribe(new string[] { "test/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });


            return base.StartAsync(cancellationToken);  
        }
        public void client_recievedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            DeviceStatus deviceStatus = new DeviceStatus();
            String[] messageTopic = e.Topic.Split('/');
            var device = dbContext.Device.Include(a => a.Status).FirstOrDefault(i => i.ID == Guid.Parse(messageTopic[1]));
            String waterTempS = System.Text.Encoding.Default.GetString(e.Message);
            decimal waterTemp = Decimal.Parse(waterTempS);
            device.Status.WaterPreasure =  waterTemp;
            try
            {
                dbContext.Device.Update(device);
                 dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException d)
            {
                throw new AppException(d.Message);

            }
            // Handle message received
            var message = System.Text.Encoding.Default.GetString(e.Message);
            _logger.LogInformation("Message received: " + message);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

              
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
