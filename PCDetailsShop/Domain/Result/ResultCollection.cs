using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Result
{
    public class ResultCollection<T> : BaseResult<IEnumerable<T>>
    {
        public long Count { get; set; }
    }
}
