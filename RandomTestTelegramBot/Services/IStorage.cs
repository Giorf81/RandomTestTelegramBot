using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTelegramBot.Models;

namespace MyTelegramBot.Services
{
    internal interface IStorage
    {   
       Session GetSession(long chatId);
    }
}
