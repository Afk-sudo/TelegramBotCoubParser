using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AnimeChans_bot.Commands
{
    public class GetCoub : Command
    {
        public override string Name => "GetCoub";
        public List<string> coubData;
        private int index = 0; 
        public override async void Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            string coubUrl = coubData[index];
            coubData.RemoveAt(index); 
            if (index < coubData.Count)
            {
                index++;
                Console.WriteLine(index);
            }  
            await botClient.SendVideoAsync(message.Chat, coubUrl);
            
        }
        public GetCoub(List<string> coubs)
        {
            coubData = coubs;
        }   
    }
}