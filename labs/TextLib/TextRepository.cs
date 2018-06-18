using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using StackExchange.Redis;

namespace TextLib
{
    public class TextRepository
    {

        const string KeyText = "text";
        const string KeyScore = "score";

        private IDatabase _database;

        public TextRepository()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            this._database = redis.GetDatabase();
        }

        public string GetText(string id)
        {
            return this._database.HashGet(id, KeyText);
        }

        public (string, bool) GetScore(string id)
        {
            var value = this._database.HashGet(id, KeyScore);
            return (value, !value.IsNullOrEmpty);
        }

        public string CreateText(string text)
        {
            var id = Guid.NewGuid().ToString();
            this._database.HashSet(id, KeyText, text);
            return id;
        }

        public void SetTextScore(string id, float score)
        {
            this._database.HashSet(id, KeyScore, score.ToString());
        }
    }
}


