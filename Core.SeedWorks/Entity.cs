using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeedWorks
{
    public abstract class Entity<T> where T : notnull, IEquatable<T>
    {
        public readonly T Id;

        protected Entity(T id)
        {
            Id = id;
        }
    }
}
