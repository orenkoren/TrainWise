using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrainWise.User
{
    public class VideoFile
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("VideoName")]
        public string VideoName { get; set; }

        [BsonElement("VideoContent")]
        public byte[] VideoContent { get; set; }
    }
}
