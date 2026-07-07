public interface IHealth
{
    void TakeDamage(float damage);
    void TakeHeal(float heal);
    event System.Action<float> OnHealthChanged; //change Amount
}
