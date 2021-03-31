using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web;

namespace Utilities.Utility
{
    public static class UnobtrusiveSession
    {
        const string COOKIE_KEY = "UnobtrusiveSessionId";

        static HttpContext GetCurrContext()
        {
            if (HttpContext.Current == null)
            {
                throw new ArgumentException("HttpContext.Current is null");
            }
            return HttpContext.Current;
        }

        public static string GetSessionID()
        {
            var cookie = GetCurrContext().Request.Cookies[COOKIE_KEY];
            if (cookie != null) { return cookie.Value; }
            //set session id cookie
            var sessionID = Guid.NewGuid().ToString();
            var cookieObj = new HttpCookie(COOKIE_KEY, sessionID);
            cookieObj.HttpOnly = true;
            GetCurrContext().Response.SetCookie(cookieObj);
            return sessionID;
        }

        public static SessionObject Session
        {
            get
            {
                var cache = MemoryCache.Default;
                var sessId = GetSessionID();
                if (!cache.Contains(sessId))
                {
                    cache.Add(sessId, new SessionObject(sessId), new CacheItemPolicy()
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(20)
                    });
                }

                return (SessionObject)cache[sessId];
            }
        }

        public class SessionObject
        {
            public string SessionID { get; set; }

            readonly Dictionary<string, object> items = new Dictionary<string, object>();

            public SessionObject(string sessId)
            {
                SessionID = sessId;
            }

            public object this[string key]
            {
                get
                {
                    lock (items)
                    {
                        if (items.ContainsKey(key)) { return items[key]; }
                        return null;
                    }
                }
                set
                {
                    lock (items)
                    {
                        items[key] = value;
                    }
                }
            }
        }
    }
}