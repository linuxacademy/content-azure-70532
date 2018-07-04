using Microsoft.Azure;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConnectionMultiplexer.Connect(
                CloudConfigurationManager.GetSetting("RedisCacheName") + ",abortConnect=false,ssl=true,password=" + CloudConfigurationManager.GetSetting("RedisCachePassword"));

            var cache = connection.GetDatabase();
            cache.KeyDelete("i1");
            cache.KeyDelete("i2");
            cache.KeyDelete("i3");

            cache.StringSet("i1", 1);
            cache.StringSet("i2", 2);

            var i1 = cache.StringGet("i1");
            var i2 = cache.StringGet("i2");
            var i3 = cache.StringGet("i3");
            if (!i3.HasValue)
            {
                // get from real database
                // put in redis
                // return db value
            } else
            {
                //return i3;
            }

            cache.StringIncrement("i2");
            i2 = cache.StringGet("i2");
        }
    }
}
