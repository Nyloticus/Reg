using System;

namespace Common
{
    public class BaseEntity<T> : IBaseEntity
    {
        public T Id { get; set; }

    }

    public interface IBaseEntity { }
}



