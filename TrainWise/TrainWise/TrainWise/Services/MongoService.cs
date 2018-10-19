using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using TrainWise.User;

namespace TrainWise.Services
{
    public static class MongoService
    {
        static IMongoCollection<Credentials> todoItemsCollection;
        readonly static string dbName = "userdatabase";
        readonly static string collectionName = "Users";
        static MongoClient client;

        static IMongoCollection<Credentials> ToDoItemsCollection
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
                    todoItemsCollection = db.GetCollection<Credentials>(collectionName, collectionSettings);
                }

                return todoItemsCollection;
            }
        }

        public async static Task<List<Credentials>> GetAllItems()
        {
            var allItems = await ToDoItemsCollection
                .Find(new BsonDocument())
                .ToListAsync();

            return allItems;
        }

        public async static Task<List<Credentials>> SearchByName(string name)
        {
            var results = await ToDoItemsCollection
                            .AsQueryable()
                            .Where(tdi => tdi.Username.Contains(name))
                            .Take(10)
                            .ToListAsync();

            return results;
        }

        public async static Task InsertItem(Credentials item)
        {
            await ToDoItemsCollection.InsertOneAsync(item);
        }

        public async static Task<bool> DeleteItem(Credentials item)
        {
            var result = await ToDoItemsCollection.DeleteOneAsync(tdi => tdi.Id == item.Id);

            return result.IsAcknowledged && result.DeletedCount == 1;
        }
    }
}
