using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FB.BusinessManagerSharer
{
    public class BMSharer
    {
        private string _pageAccessToken;
        private readonly string _amAccessToken;
        private RestClient _restClient;

        public BMSharer(string apiAddress, string pageAccessToken, string amAccessToken)
        {
            _pageAccessToken = pageAccessToken;
            _amAccessToken = amAccessToken;
            _restClient = new RestClient(apiAddress);
        }

        public void CreateAndShare(int cnt)
        {
            var links=new List<string>();
            for (int i = 0; i < cnt; i++)
            {
                Console.Write("Введите название нового fan page:");
                var fpName = Console.ReadLine();

                var request = new RestRequest($"me/accounts", Method.POST);
                request.AddParameter("access_token", _pageAccessToken);
                request.AddParameter("name", fpName);
                request.AddParameter("category", "2606");
                request.AddParameter("access_token", _pageAccessToken);
                var response = _restClient.Execute(request);
                var json = (JObject)JsonConvert.DeserializeObject(response.Content);
                ErrorChecker.HasErrorsInResponse(json, true);
                var fpId = json["id"].ToString();
                Console.WriteLine("Fan page создан!");

                Console.Write("Введите название нового Business Manager (2 слова):");
                var bmName = Console.ReadLine();
                request = new RestRequest($"me/businesses", Method.POST);
                request.AddParameter("access_token", _amAccessToken);
                request.AddParameter("name", bmName);
                request.AddParameter("vertical", "ADVERTISING");
                request.AddParameter("primary_page", fpId);
                request.AddParameter("timezone_id", "1");
                response = _restClient.Execute(request);
                json = (JObject)JsonConvert.DeserializeObject(response.Content);
                ErrorChecker.HasErrorsInResponse(json, true);
                var bmId = json["id"].ToString();
                Console.WriteLine("БМ создан!");

                Console.WriteLine("Шарим БМ...");
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var r = new Random();
                var email = new string(Enumerable.Repeat(chars, 8)
                  .Select(s => s[r.Next(s.Length)]).ToArray()) + "@gmail.com";

                request = new RestRequest($"{bmId}/business_users", Method.POST);
                request.AddParameter("access_token", _amAccessToken);
                request.AddParameter("email", email);
                request.AddParameter("role", "ADMIN");
                response = _restClient.Execute(request);
                json = (JObject)JsonConvert.DeserializeObject(response.Content);
                ErrorChecker.HasErrorsInResponse(json, true);
                Console.WriteLine("БМ расшарен!");

                Console.WriteLine("Получаю ссылку...");
                request = new RestRequest($"{bmId}/pending_users", Method.GET);
                request.AddQueryParameter("access_token", _amAccessToken);
                request.AddQueryParameter("fields", "id,role,email,invite_link,status");
                response = _restClient.Execute(request);
                json = (JObject)JsonConvert.DeserializeObject(response.Content);
                ErrorChecker.HasErrorsInResponse(json, true);
                Console.WriteLine("Ссылка получена:"+json["invite_link"]);
                links.Add(json["invite_link"].ToString());
            }
            Console.WriteLine("Используйте эти ссылки для добавления себе БМов:");
            links.ForEach(Console.WriteLine);
            Console.WriteLine("Все задания выполнены, лейте в плюс, господа!");
            Console.WriteLine("Нажмите Enter для завершения работы");
            Console.ReadLine();
        }
    }
}