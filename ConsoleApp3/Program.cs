using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (WebSocket websocket = new WebSocket("wss://stream.binance.com:9443/ws/LOOMBTC@depth@100ms"))
            {
                websocket.Opened += new EventHandler(websocket_Opened);
                websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
                websocket.Closed += new EventHandler(websocket_Closed);
                websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
                websocket.Open();

                Console.WriteLine("before loop");
                while (websocket.State != WebSocketState.Open)
                {
                    //Console.Write("");
                }
                Console.WriteLine("after loop");
                websocket.Send("{\"method\": \"SUBSCRIBE\",\"params\":[\"loombtc@depth\"],\"id\": 1}");
                Console.ReadKey();
            }
        }

        private static void websocket_Opened(object sender, EventArgs e)
        {
            Console.WriteLine($"socket OPENED, sender: {sender} and eventargs e: {e}");
        }

        private static void websocket_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"socket ERROR, sender: {sender} and eventargs e: {e.Exception}");
        }

        private static void websocket_Closed(object sender, EventArgs e)
        {
            Console.WriteLine($"socket CLOSED, sender: {sender} and eventargs e: {e}");
        }

        private static void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine($"socket MESSAGE RECEIVED, sender: {sender} and eventargs e: {e.Message}");
        }
    }
}
