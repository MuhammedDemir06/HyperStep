using System;

public interface ITime
{
    event Action<int> OnTimeChanged;
}
