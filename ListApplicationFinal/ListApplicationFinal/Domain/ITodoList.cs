﻿using System;
using System.Collections.Generic;

namespace ListApplicationFinal.Domain
{
    public interface ITodoList
    {
        string Id { get; set; }
        string Name { get; set; }
        string Owner { get; set; }
        DateTime PointOfCreation { get; set; }
        IEnumerable<ITodo> ItemCollection { get; set; }
    }
}