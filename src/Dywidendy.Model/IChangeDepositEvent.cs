using System;

namespace Dywidendy.Model
{
    public interface IChangeDepositEvent
    {
        decimal Value { get; }
        decimal Rate { get; }
        DateTime Date { get; }
    }
}