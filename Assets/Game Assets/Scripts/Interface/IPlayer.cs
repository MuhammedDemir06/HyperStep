using System;

public interface IPlayer
{
    event Action<float> OnDashCooldownChanged;
}