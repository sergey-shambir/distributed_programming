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
        const int DatabaseCount = 4;

        const string KeyText = "text";
        const string KeyScore = "score";
        const string KeyStatus = "status";

        const string KeyStatistics = "stat-7da79c95-0a1b-4062-8325-8419d667f9a0";

        private ConnectionMultiplexer _redis;
        private IDatabase[] _databases = new IDatabase[DatabaseCount];

        public TextRepository()
        {
            this._redis = ConnectionMultiplexer.Connect("localhost");
        }

        public string GetText(string id)
        {
            return this.GetDatabase(id).HashGet(id, KeyText);
        }

        public TextStatus GetTextStatus(string id)
        {
            string value = this.GetDatabase(id).HashGet(id, KeyStatus);
            if (value == null)
            {
                return TextStatus.Pending;
            }
            return (TextStatus)Enum.Parse(typeof(TextStatus), value, true);
        }

        public string GetScore(string id)
        {
            var value = this.GetDatabase(id).HashGet(id, KeyScore);
            return value;
        }

        public string GetStatsReport()
        {
            string id = KeyStatistics;
            var value = this.GetDatabase(id).StringGet(id);
            return value;
        }

        public string CreateText(string text)
        {
            var id = Guid.NewGuid().ToString();
            var db = this.GetDatabase(id);
            db.HashSet(id, KeyText, text);
            db.HashSet(id, KeyStatus, TextStatus.Pending.ToString());
            return id;
        }

        public void SetTextScore(string id, float score)
        {
            this.GetDatabase(id).HashSet(id, KeyScore, score.ToString());
        }

        public void SetTextStatus(string id, TextStatus status)
        {
            this.GetDatabase(id).HashSet(id, KeyStatus, status.ToString());
        }

        public void SetStatsReport(string json)
        {
            string id = KeyStatistics;
            this.GetDatabase(id).StringSet(id, json);
        }

        // GetDatabase - returns database instance which must be used for given context id.
        private IDatabase GetDatabase(string id)
        {
            uint hash = this.GetHashFNV1(id);
            int index = (int)(hash % _databases.Length);
            if (this._databases[index] == null)
            {
                this._databases[index] = this._redis.GetDatabase(index);
            }
            Console.WriteLine("selected database #" + index + " for '" + id + "'");
            return this._databases[index];
        }

        // GetHashFNV1 - calculates hash using FNV1 algorithm
        // FNV1 provides stable hash which doesn't depend on runtime environment.
        private uint GetHashFNV1(string value)
        {
            const uint offsetFNV1 = 2166136261;
            const uint primeFNV1 = 16777619;
            uint hash = offsetFNV1;
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            foreach (byte b in bytes)
            {
                hash = hash * primeFNV1;
                hash = hash ^ b;
            }
            return hash;
        }
    }
}


