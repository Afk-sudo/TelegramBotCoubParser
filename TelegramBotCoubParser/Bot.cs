using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using AnimeChans_bot.Commands;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;


namespace AnimeChans_bot
{
    class Program
    {
        public static IReadOnlyList<Command> Commands
        {
            get => _commandsList.AsReadOnly();
        }

        private static ITelegramBotClient botClient;
        private static List<Command> _commandsList;
        private static List<string> _coubData = new List<string>();
        private static List<string> _coubDataAnime = new List<string>();
        private static string httpRequest = "";

        private static int iterationCounter;

        static void Main(string[] args)
        {
            GetCoub(httpRequest, _coubData);
            botClient = botClient = new TelegramBotClient(AppSetings.Token) {Timeout = TimeSpan.FromSeconds(10)};
            _commandsList = new List<Command>();
            _commandsList.Add(new GetCoub(_coubData));
            botClient.OnMessage += ReplyToUser;
            botClient.StartReceiving();
            Console.ReadKey();
            botClient.StopReceiving();
        }

        private static async void ReplyToUser(object sender, MessageEventArgs eventArgs)
        {
            var message = eventArgs?.Message;
            if(message == null)   
                return;
            foreach (var command in Commands)
            {
                if (command.Contains(message.Text))
                {
                    iterationCounter++;
                    Console.WriteLine($"Iteration: {iterationCounter}");
                    command.Execute(message, (TelegramBotClient)botClient);
                    break;
                }
            }
        }
        private static void GetCoub(string httpRequset, List<string> coubList)
        {
            HttpWebRequest request =  (HttpWebRequest)WebRequest.Create(httpRequset);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string coubJsonData = streamReader.ReadToEnd();
            response.Close();
            dynamic coubsJson = JsonConvert.DeserializeObject(coubJsonData);
            
            for (int coub = 0; coub < coubsJson.coubs.Count; coub++)
                {
                    string url = coubsJson.coubs[coub].file_versions.share["default"];
                    if (url == null)
                {
                    url = coubsJson.coubs[coub].file_versions.html5.video.url; 
                }
                if(url != null)
                    coubList.Add(url);
            }    
        }
    }
}