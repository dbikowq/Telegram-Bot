using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace onlineBotTelegram
{
    class Program
    {
        public static TelegramBotClient Bot;

        static void Main(string[] args)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Bot = new TelegramBotClient("1322924517:AAFaU38hlVEOw3Mi2BEQLXaGPYQepgJrUmU");
            var nameBot = Bot.GetMeAsync().Result;

            Bot.OnMessage += Bot_OnMessage;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private async static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            //long idd = -364335647;
            //long idd = -1414557052;
            Telegram.Bot.Types.Message msg = e.Message;
            if (msg==null || msg.Type!=Telegram.Bot.Types.Enums.MessageType.Text)
            {
                return;
            }

            String answer="";
            switch(msg.Text)
            {
                case "/gen":
                    answer = msg.From.FirstName + " " + genWord();
                    break;
                case "/gen@TryCreateMeBot":
                    answer = msg.From.FirstName + " " + genWord();
                    break;
                default: return;
            }
            //answer = "сори, я потестить";
            await Bot.SendTextMessageAsync(msg.Chat.Id, answer);

        }




        public static string genWord()
        {
            String postData = "con=";
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] postDataByte = enc.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://megagenerator.ru/words/");
            request.Method = "POST";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.CookieContainer = new CookieContainer();
            request.Accept = "*/*";
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.ContentLength = postDataByte.Length;


            Stream stream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(stream);
            sw.Write(postDataByte);
            stream.Write(postDataByte, 0, postDataByte.Length);
            stream.Close();


            WebResponse response = (HttpWebResponse)request.GetResponse();
            String tmp = new StreamReader(response.GetResponseStream()).ReadToEnd();

            System.Text.RegularExpressions.Match pattern;
            pattern = Regex.Match(tmp, @"\s+(\w+)\s+");

            return pattern.Groups[1].Value ;


        }



    }
}
