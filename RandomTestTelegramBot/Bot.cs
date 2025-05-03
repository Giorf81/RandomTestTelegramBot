using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MyTelegramBot.Controllers;
using RandomTestTelegramBot.Controllers;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

internal class Bot : BackgroundService
{
    private ITelegramBotClient _telegramClient;
    private AnyMessageContoller _messageController;
    private InlineKeyboardController _keyboardController;

    public Bot(ITelegramBotClient telegramClient, AnyMessageContoller anyMessageContoller, InlineKeyboardController inlineKeyboardController)
    {
        _telegramClient = telegramClient;
        _messageController = anyMessageContoller;
        _keyboardController = inlineKeyboardController;

    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, new ReceiverOptions() { AllowedUpdates = { } }, cancellationToken: stoppingToken);
        Console.WriteLine("Бот запущен");
    }
    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _keyboardController.Handle(update.CallbackQuery, cancellationToken);
            return;
        }
        Console.WriteLine($"Контроллер {GetType().Name} получил сообщение: {update.Message.Text}");
        


        if (update.Type == UpdateType.Message)
        {
                
            switch (update.Message!.Type)
            {
                case MessageType.Text:
                    if (update.Message.Text == "/start")
                        await _messageController.Handle(update.Message, cancellationToken);
                    else if (_keyboardController.choosedAction == "Подсчет количества символов" || _keyboardController.choosedAction == "count")
                        await _telegramClient.SendMessage(update.Message.From.Id, $"Длина сообщения: {update.Message.Text.Length} знаков", cancellationToken: cancellationToken);
                    else if (_keyboardController.choosedAction == "Сумма чисел" || _keyboardController.choosedAction == "sum")
                    {
                        string[] numbers = update.Message.Text.Split(' ');
                        int sum = 0;
                        foreach (string number in numbers)
                        {
                            if (int.TryParse(number, out int num))
                            {
                                sum += num;
                            }
                        }
                        await _telegramClient.SendMessage(update.Message.From.Id, $"Сумма чисел: {sum}", cancellationToken: cancellationToken);
                    }
                    
                    return;
                default:
                    Console.WriteLine($"Получено сообщение типа: {update.Message.Type}");
                    await _telegramClient.SendMessage(update.Message.From.Id, $"Данный тип сообщений не поддерживается. Пожалуйста отправьте текст.", cancellationToken: cancellationToken);

                    return;
            }
        }
    }


        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error: \n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(errorMessage);
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(1000);
            return Task.CompletedTask;
        }
    }


