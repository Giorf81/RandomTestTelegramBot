using MyTelegramBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Controllers
{
    internal class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramBotClient;
        public string choosedAction;



        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage storage)
        {
            _telegramBotClient = telegramBotClient;
            _memoryStorage = storage;
        }
        public async Task Handle(CallbackQuery? callback, CancellationToken ct)
        {
            if (callback.Data == null) return;
            _memoryStorage.GetSession(callback.From.Id).ChoosedAction = callback.Data;

            choosedAction = callback.Data switch { 
            "count" => "Подсчет количества символов",
            "sum" => "Сумма чисел",
            _ => String.Empty
            };
            await _telegramBotClient.SendMessage(callback.From.Id, $"<b>Выбрано действие - {choosedAction}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);

        }
    }
}
