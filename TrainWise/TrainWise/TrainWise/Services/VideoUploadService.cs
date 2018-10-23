using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using TrainWise.User;
using System.IO;

namespace TrainWise.Services
{
    public static class UploadVideoService
    {
        static IMongoCollection<Video> todoItemsCollection;
        readonly static string dbName = "videodatabase";
        readonly static string collectionName = "Videos";
        static MongoClient client;

        static IMongoCollection<Video> ToDoItemsCollection
        {
            get
            {
                if (client == null || todoItemsCollection == null)
                {
                    var conx = "mongodb://trainwise:dz21MshCZ1NzQmQOtbTYu6A8XSXv950HOWIiQVqtDvkgoBkKUmcQJlUlWpVXrfQopv0aFPHcRsNm1lkm9W5KnA==@trainwise.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
                    MongoClientSettings settings = MongoClientSettings.FromUrl(
                        new MongoUrl(conx)
                    );

                    settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };

                    client = new MongoClient(settings);
                    var db = client.GetDatabase(dbName);

                    var collectionSettings = new MongoCollectionSettings { ReadPreference = ReadPreference.Nearest };
                    todoItemsCollection = db.GetCollection<Video>(collectionName, collectionSettings);
                }

                return todoItemsCollection;
            }
        }

        static MongoGridFs GridFs
        {
            get
            {
                if (client == null)
                {
                    var conx = "mongodb://trainwise:dz21MshCZ1NzQmQOtbTYu6A8XSXv950HOWIiQVqtDvkgoBkKUmcQJlUlWpVXrfQopv0aFPHcRsNm1lkm9W5KnA==@trainwise.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
                    MongoClientSettings settings = MongoClientSettings.FromUrl(
                        new MongoUrl(conx)
                    );

                    settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };

                    client = new MongoClient(settings);
                    var db = client.GetDatabase(dbName);
                    return new MongoGridFs(db);
                }
                return new MongoGridFs(client.GetDatabase(dbName));
            }
        }


        public async static Task<List<Video>> GetAllItems()
        {
            var allItems = await ToDoItemsCollection
                .Find(new BsonDocument())
                .ToListAsync();

            return allItems;
        }

        public async static Task<List<Video>> SearchByName(string name)
        {
            var results = await ToDoItemsCollection
                            .AsQueryable()
                            .Where(tdi => tdi.VideoName.Contains(name))
                            .Take(10)
                            .ToListAsync();

            return results;
        }

        public async static Task InsertItem(Video item)
        {
            await ToDoItemsCollection.InsertOneAsync(item);
        }

        public static void InsertGrid(string filePath)
        {
            var id = ObjectId.Empty;
            using (var file = File.OpenRead(filePath))
            {
                id = GridFs.AddFile(file, filePath);
            }
        }

        public async static Task<bool> DeleteItem(Video item)
        {
            var result = await ToDoItemsCollection.DeleteOneAsync(tdi => tdi.Id == item.Id);

            return result.IsAcknowledged && result.DeletedCount == 1;
        }
    }
}
