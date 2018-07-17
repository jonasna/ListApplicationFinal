using System;

namespace DialogServices
{
    public interface INotifyResultSet
    {
        event EventHandler ResultSetEvent;
    }
}