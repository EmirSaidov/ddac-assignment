using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class AdminModel
    {
        public int ID { get; set; }

        public virtual UserModel user { get; set; }
    }
}
