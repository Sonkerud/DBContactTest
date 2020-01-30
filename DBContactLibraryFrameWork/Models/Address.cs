using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContactLibraryFrameWork.Models
{
    public class Address 
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int Zip { get; set; }

        public override string ToString()
        {
            return $"{Street} {City} {Zip}";
        }
    }
}
