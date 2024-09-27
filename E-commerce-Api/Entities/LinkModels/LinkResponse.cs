using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public  class LinkResponse<T>
    {
        public bool HasLinks { get; set; }
        public List<T> Entities { get; set; }
        public LinkCollectionWrapper<LinksForObject<T>> LinkedEntities { get; set; }
        public LinkResponse()
        {
            LinkedEntities = new LinkCollectionWrapper<LinksForObject<T>>();
            Entities = new List<T>();
        }
    }
}
