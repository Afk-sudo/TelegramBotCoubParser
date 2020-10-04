using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnimeChans_bot
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract void Execute(Message message, TelegramBotClient botClient);

        public bool Contains(string command)
        {
            return command.Contains(this.Name);
        }
    }
}