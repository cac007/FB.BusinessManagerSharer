using Microsoft.Extensions.Configuration;
using System;

namespace FB.BusinessManagerSharer
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var apiAddress = config.GetValue<string>("fbapi_address");

            Console.Write("Введите access token для создания страниц:");
            var pageAccessToken=Console.ReadLine();
            Console.Write("Введите access token от Ads Manager-а:");
            var amAccessToken=Console.ReadLine();
            Console.Write("Сколько БМов надо создать и расшарить? Введите число:");
            var cnt=int.Parse(Console.ReadLine());
            var bms = new BMSharer(apiAddress, pageAccessToken,amAccessToken);
            bms.CreateAndShare(cnt);
        }
    }
}
