using Newtonsoft.Json.Linq;
using System;

namespace FB.BusinessManagerSharer
{
    public static class ErrorChecker
    {
        public static bool HasErrorsInResponse(JObject json,bool throwException=false)
        {
			if (json["error"]==null) return false;
			
            var msg=$"Ошибка при попытке выполнить запрос:{json["error"]}!";
            if (throwException)
                throw new Exception(msg);
            Console.WriteLine(msg);
            return true;
        }

        public static bool VideoIsNotReadyResponse(JObject json)
        {
            var error = json["error"]?["message"].ToString();
            var eut=json["error"]?["error_user_title"].ToString();
            var eum=json["error"]?["error_user_msg"].ToString();
            if (eut== "Video not ready for use in an ad")
            {
                return true;
            }
            return false;
        }

    }
}