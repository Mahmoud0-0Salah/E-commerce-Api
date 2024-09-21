using Newtonsoft.Json;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.ComponentModel;

namespace Shared.RequestFeatures
{
    public class ProductParameters : RequestParameters
    {

        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; } = int.MaxValue;

    }
}
