using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoliSytemsDraft.Model
{
    public class UserDetails
    {
        public String Firstname { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public int count { get; set; }
       public List<Country> country { get; set; }
    }
}
