using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthChecker.Models
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class HealthCheckModel
    { 
        public string checker { get; set; }
         
        public string output { get; set; }
         
        public bool passed { get; set; }
         
        public double timestamp { get; set; }
         
        public double expires { get; set; }
    }

    public class Response
    { 
        public string hostname { get; set; }
         
        public string status { get; set; }
         
        public double timestamp { get; set; } 

        public List<HealthCheckModel> results { get; set; }
    }

}