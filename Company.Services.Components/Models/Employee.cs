using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Services.Components.Models
{
    [Table("Employee")]
    public class Employee
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement]
        public  string EmployeeID { get; set; }

        [BsonElement]
        public  string FirstName { get; set; }

        [BsonElement]
        public  string LastName { get; set; }

        [BsonElement]
        public  string PrimaryEmail { get; set; }

        [BsonElement]
        public  string PhoneNumber { get; set; }

        [BsonElement]
        public Boolean IsActive { get; set; }
    }
}