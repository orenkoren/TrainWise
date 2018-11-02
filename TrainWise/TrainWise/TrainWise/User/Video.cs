using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrainWise.User
{
    public class Video
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("VideoName")]
        public string VideoName { get; set; }

        [BsonElement("OfUser")]
        public string OfUser { get; set; }

        [BsonElement("VideoPath")]
        public string VideoPath{ get; set; }

        [BsonElement("ExerciseType")]
        public string ExerciseType { get; set; }

        [BsonElement("Repetitions")]
        public int Repetitions { get; set; }

        [BsonElement("Weight")]
        public int Weight { get; set; }
    }
}
