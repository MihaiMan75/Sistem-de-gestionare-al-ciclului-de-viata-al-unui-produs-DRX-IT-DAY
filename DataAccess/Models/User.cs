using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class User
    {
        public int id {  get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string passwordHashed { get; set; }
        public string phoneNumber { get; set; }
    }
}
