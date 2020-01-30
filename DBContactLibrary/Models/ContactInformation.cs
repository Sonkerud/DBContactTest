using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContactLibrary.Models
{
    public class ContactInformation
    {
        public int Id { get; set; }
        public string Info { get; set; }

        public override string ToString()
        {

            return $"{Info}";
        }
    }
}
