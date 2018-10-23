using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TrainWise.User;

namespace TrainWise.Services
{
    public class MongoGridFs
    {
        private readonly IMongoDatabase _db;
        private readonly MongoGridFS _gridFs;

        public MongoGridFs(IMongoDatabase db)
        {
            _db = db;
            _gridFs = (db as MongoDatabase).GridFS;
        }

        public ObjectId AddFile(Stream fileStream, string fileName)
        {
            var fileInfo = _gridFs.Upload(fileStream, fileName);
            return (ObjectId)fileInfo.Id;
        }

        public Stream GetFile(ObjectId id)
        {
            var file = _gridFs.FindOneById(id);
            return file.OpenRead();
        }
    }
}
