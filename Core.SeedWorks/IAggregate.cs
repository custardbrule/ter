using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeedWorks
{
    public interface IAggregate<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; }
    }
}
