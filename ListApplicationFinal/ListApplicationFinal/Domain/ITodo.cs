using System;

namespace ListApplicationFinal.Domain
{
    public interface ITodo : ICloneable
    {
        string Name { get; set; }
        bool Complete { get; set; }
        string Description { get; set; }
    }
}