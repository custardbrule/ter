using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeedWorks
{
    public interface IDelete<TKey>
    {
        TKey? DeletedBy { get; }
        bool IsDeleted { get; }
    }
}
