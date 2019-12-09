using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonHelper
{
    /// <summary>
    /// MongoDB数据库
    /// </summary>
    public class MongoDBHelper
    {
        public static string connectionstring = "mongodb://root:-C2uVB_qVr7k-9PMCKcafdZ4KWf5fKngOc3-Er9Susohgny_vG@172.21.168.38:27017";
        public static string databaseName = "mqtt-server-for-webaccess";
        private readonly IMongoDatabase _database = null;

        public MongoDBHelper()
        {
            var client = new MongoClient(connectionstring);
            if (client != null)
            {
                _database = client.GetDatabase(databaseName);
            }
        }

        public MongoDBHelper(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            if (client != null)
            {
                _database = client.GetDatabase(databaseName);
            }
        }

        #region SELECT
        /// <summary>
        /// 根据查询条件，获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName">数据表名称</param>
        /// <param name="conditions">查询条件，查询条件编写按照Lamada表达式形式</param>
        /// <returns></returns>
        public List<T> GetList<T>(string TableName,Expression<Func<T, bool>> conditions = null)
        {
            try
            {
                var collection = _database.GetCollection<T>(TableName);
                if (conditions != null)
                {
                    return collection.Find(conditions).ToList();
                }
                return collection.Find(_ => true).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 根据查询条件，获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName">数据表名称</param>
        /// <param name="Filter">查询条件</param>
        /// <param name="LimitNum">默认的限制条数，默认为100</param>
        /// <returns></returns>
        public List<T> GetList<T>(string TableName, FilterDefinition<T> Filter)
        {
            try
            {
                var collection = _database.GetCollection<T>(TableName);
                List<T> searchResult = collection.Find(Filter).ToList();//Limit(LimitNum)
                return searchResult;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }
        #endregion

        #region INSERT
        /// <summary>
        /// 插入多条数据，数据用list表示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<T> InsertMany<T>(List<T> list,string TableName)
        {
            var collection = _database.GetCollection<T>(TableName);
            collection.InsertMany(list);
            return list;
        }
        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="document"></typeparam>
        /// <param name="CollectionName"></param>
        /// <returns></returns>
        public void InsertOne<T>(T document, string CollectionName)
        {
            var collection = _database.GetCollection<T>(CollectionName);
            collection.InsertOne(document);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除数据记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public int DeleteMany<T>(string TableName, FilterDefinition<T> Filter)
        {
            try
            {
                var collection = _database.GetCollection<T>(TableName);
                DeleteResult deleteResult = collection.DeleteMany(Filter);
                return (int)deleteResult.DeletedCount;
            }
            catch(Exception ex)
            {
                return - 1;
            }
            
        }
        #endregion


        #region capped collection监测数据
        public async Task TailCollectionAsync()
        {
            // Set lastInsertDate to the smallest value possible
            //BsonValue lastInsertDate = BsonMinKey.Value;
            var collection = _database.GetCollection<MongoDbTag>("raw_data");
            var options = new FindOptions<MongoDbTag>
            {
                // Our cursor is a tailable cursor and informs the server to await
                CursorType = CursorType.TailableAwait
            };

            // Initially, we don't have a filter. An empty BsonDocument matches everything.
            // BsonDocument filter = new BsonDocument();
            DateTime lastInsertDate = DateTime.MinValue;
            var filterBuilder1 = Builders<MongoDbTag>.Filter;
            var filter1 = filterBuilder1.And(filterBuilder1.Gt(x => x.ts, lastInsertDate));

            // NOTE: This loops forever. It would be prudent to provide some form of 
            // an escape condition based on your needs; e.g. the user presses a key.
            while (true)
            {
                // Start the cursor and wait for the initial response
                using (var cursor = await collection.FindAsync(filter1, options))
                {
                    // This callback will get invoked with each new document found
                    await cursor.ForEachAsync(document =>
                    {
                        // Set the last value we saw 
                        MongoDbTag tag = document as MongoDbTag;
                        lastInsertDate = tag.ts;
                    });
                }

                // The tailable cursor died so loop through and restart it
                // Now, we want documents that are strictly greater than the last value we saw
                filter1 = filterBuilder1.And(filterBuilder1.Gt(x => x.ts, lastInsertDate));
            }
        }
        /// <summary>
        /// 监测定长表格结尾数据
        /// </summary>
        /// <param name="CollectionName">collection名称</param>
        /// <param name="TimeColName">时间列的名称</param>
        /// <returns></returns>
        public async Task TailCollectionAsync(string CollectionName, string TimeColName, DateTime InitialTime= default(DateTime))
        {
            // Set lastInsertDate to the smallest value possible
            BsonValue lastInsertDate = (BsonValue)InitialTime;
            var collection = _database.GetCollection<BsonDocument>(CollectionName);
            var options = new FindOptions<BsonDocument>
            {
                // Our cursor is a tailable cursor and informs the server to await
                CursorType = CursorType.TailableAwait
            };

            // Initially, we don't have a filter. An empty BsonDocument matches everything.
            BsonDocument filter = new BsonDocument();
          
            // NOTE: This loops forever. It would be prudent to provide some form of 
            // an escape condition based on your needs; e.g. the user presses a key.
            while (true)
            {
                // Start the cursor and wait for the initial response
                using (var cursor = await collection.FindAsync(filter, options))
                {
                    try
                    {
                        // This callback will get invoked with each new document found
                        await cursor.ForEachAsync(document =>
                        {
                            // Set the last value we saw 
                            lastInsertDate = document[TimeColName];

                            // Write the document to the console.
                            Console.WriteLine("TailCollectionAsync==" + document.ToString());
                            if (BsonDocumentEvent != null)
                            {
                                BsonDocumentEvent(document);
                            }
                        });
                    }
                    catch(Exception ex)
                    {
                        string info=ex.Message;
                        Console.WriteLine(info);
                    }
                }

                // The tailable cursor died so loop through and restart it
                // Now, we want documents that are strictly greater than the last value we saw
                filter = new BsonDocument("$gt", new BsonDocument(TimeColName, lastInsertDate));
            }
        }
        /// <summary>
        /// BsonDocument委托
        /// </summary>
        public delegate void BsonDocumentDelegate(BsonDocument document);
        /// <summary>
        /// BsonDocument委托事件
        /// </summary>
        public event BsonDocumentDelegate BsonDocumentEvent = delegate { };
        #endregion
    }
    /// <summary>
    /// 云端的MongoDB数据库Tag类
    /// </summary>
    public class MongoDbTag
    {
        [BsonId]
        public ObjectId ID { get; set; }
        /// <summary>
        /// s云端为scada id 信息
        /// </summary>
        [BsonElement("s")]
        public string s { get; set; }
        /// <summary>
        /// t在云端为device/tag的组合
        /// </summary>
        [BsonElement("t")]
        public string t { get; set; }
        [BsonElement("v")]
        public object v { get; set; }
        [BsonElement("ts")]
        public DateTime ts { get; set; }
    }
    /// <summary>
    /// 本地的mongoDb，比云端多两个字段
    /// </summary>
    public class LocalMongoDbTag: MongoDbTag
    {
        [BsonElement("d")]
        public string d { get; set; }
      
        [BsonElement("createdAt")]
        public DateTime createdAt { get; set; }
    }

    
}
