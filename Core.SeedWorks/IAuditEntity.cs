using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeedWorks
{
    public interface IAuditEntity<TKey>
    {
        TKey? CreatedBy { get; }
        TKey? UpdatedBy { get; }
        DateTimeOffset CreatedAt { get; }
        DateTimeOffset UpdatedAt { get; }
    }
}
