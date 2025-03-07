using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Result
{
    public class CollectionResult<T> : BaseResult<List<T>>
    {
        public long Count { get; set; }
    }
}
