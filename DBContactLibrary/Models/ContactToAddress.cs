using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContactLibrary.Models
{
    public class ContactToAddress
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public int ContactId { get; set; }

    }
}
