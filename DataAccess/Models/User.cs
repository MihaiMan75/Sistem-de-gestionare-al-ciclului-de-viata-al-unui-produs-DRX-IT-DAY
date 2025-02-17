using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class User
    {
        public int id_user {  get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string PasswordHashed { get; set; }
        public string phone_number { get; set; }
    }
}
