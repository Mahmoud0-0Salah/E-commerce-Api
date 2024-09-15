using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class ProductParameters : RequestParameters
    {

        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; } = int.MaxValue;
        public bool ValidAgeRange => MaxPrice > MinPrice;
    }
}
