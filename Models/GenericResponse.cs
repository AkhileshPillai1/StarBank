using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBank.Models
{
    internal class GenericResponse
    {
        public bool isSuccess {  get; set; }
        public string errorMessage { get; set; } = string.Empty;
        public object? data { get; set; }
    }
}
