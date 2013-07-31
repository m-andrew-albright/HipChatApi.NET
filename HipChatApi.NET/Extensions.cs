using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HipChatApi
{
    public static class Extensions
    {
        public static IEnumerable<T2> SelectSafely<T1, T2>(this IEnumerable<T1> e, Func<T1, T2> f)
        {
            return e.IsNullOrEmpty() ? new List<T2>() : e.Select(f);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> e)
        {
            return e == null || !e.Any();
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> e)
        {
            return !IsNullOrEmpty(e);
        }

        public static int? ToInt(this string str)
        {
            int i;
            if (int.TryParse(str, out i))
            {
                return i;
            }

            return null;
        }

        public static bool IsInt(this string str)
        {
            int i;
            return int.TryParse(str, out i);
        }

        public static bool Success(this IRestResponse response)
        {
            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
