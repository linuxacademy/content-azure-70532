using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingWithQueues
{
    class Program
    {
        static void Main(string[] args)
        {
            //queues();
            topics();

            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private static void topics()
        {
            var conn = "Endpoint=sb://la70532.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=lNmRa6tv+UXOAWJalX9ggE6uzb9lba8wVWkL5hd4rlM=";

            topicReceiver(conn, "mytopic", "MySubscription1", "Receiver 1");
            topicReceiver(conn, "mytopic", "MySubscription2", "Receiver 2");
            topicSender(conn);
        }

        private static void topicReceiver(string conn, string topicName, string subscriptionName, string receiverName)
        {
            var subscriptionClient = SubscriptionClient.CreateFromConnectionString(conn, topicName, subscriptionName);
            subscriptionClient.OnMessage(brokeredMessage =>
            {
                Console.WriteLine(receiverName + " got: " + brokeredMessage.GetBody<string>());
            });
        }

        private static void topicSender(string conn)
        {
            /*
            var nsm = NamespaceManager.CreateFromConnectionString(conn);
            if (!nsm.TopicExists("mytopic"))
            {
                nsm.CreateTopic("mytopic");
            }
            var topic = nsm.GetTopic("mytopic");
            if (!nsm.SubscriptionExists("mytopic", "MySubscription1"))
            {
                nsm.CreateSubscription("mytopic", "MySubscription1");
            }
            var subscription = nsm.GetSubscription("mytopic", "MySubscription1");
            */
            var topicClient = TopicClient.CreateFromConnectionString(conn, "mytopic");
            //topicClient.Send(new BrokeredMessage("This is from the sender of the topic client"));
            Enumerable.Range(0, 10).ToList().ForEach(i =>
            {
                var msg = new BrokeredMessage(i.ToString());
                topicClient.Send(msg);
                Console.WriteLine("Sent " + i.ToString());
            });

        }

        private static void queues() {
            var conn = "Endpoint=sb://la70532.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=lNmRa6tv+UXOAWJalX9ggE6uzb9lba8wVWkL5hd4rlM=";
            var queue = "MyQueue";

            listener(conn, queue, "Listener 1");
            listener(conn, queue, "Listener 2");
            send(conn, queue);

            Console.ReadKey();
        }

        private static void listener(string conn, string queue, string listenerName)
        {
            var client = QueueClient.CreateFromConnectionString(conn, queue);
            client.OnMessage(brokeredMessage =>
            {
                var data = brokeredMessage.GetBody<string>();
                Console.WriteLine($"{listenerName} received: {data}");
            });

        }

        private static void receive(QueueClient client)
        {
            client.OnMessage(brokeredMessage =>
            {
                Console.WriteLine(brokeredMessage.GetBody<string>());
            });
        }

        private static void send(string conn, string queue) {
            var client = QueueClient.CreateFromConnectionString(conn, queue);

            Enumerable.Range(0, 10).ToList().ForEach(i =>
            {
                var msg = new BrokeredMessage(i.ToString());
                client.Send(msg);
                Console.WriteLine("Sent " + i.ToString());
            });
        }
    }
}
