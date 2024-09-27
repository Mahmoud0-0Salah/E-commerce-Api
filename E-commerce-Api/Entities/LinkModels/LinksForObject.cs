using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public class LinksForObject<T> 
    {
        public T Value { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
