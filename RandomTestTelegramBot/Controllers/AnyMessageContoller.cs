using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using RandomTestTelegramBot.Controllers;
using MyTelegramBot.Controllers;

namespace RandomTestTelegramBot.Controllers
{
    internal class AnyMessageContoller
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private InlineKeyboardController _keyboardController;

        public AnyMessageContoller(ITelegramBotClient telegramBotClient, InlineKeyboardController inlineKeyboardController)
        {
            _telegramBotClient = telegramBotClient;
            _keyboardController = inlineKeyboardController;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[] {
                        InlineKeyboardButton.WithCallbackData($" Количество символов", "count"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел", "sum")
                    });
                    Console.WriteLine($"Контроллер {GetType().Name} получил сообщение: {message.Text}");
                    await _telegramBotClient.SendMessage(message.Chat.Id, $"<b> Данный бот может посчитать количество символов в сообщении, либо же посчитать сумму чисел. </b> {Environment.NewLine}"
                        + $"{Environment.NewLine}Вы можете написать любое сообщение и я посчитаю количество символов в нем, либо напишите несколько чисел через пробел и я напишу их сумму)", cancellationToken: ct, parseMode: ParseMode.Html,
                        replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
            }
        }

    }
}
