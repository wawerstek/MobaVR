namespace MobaVR
{
    public interface IDamageable
    {
        public void Hit(HitData hitData);
        public void Die();
        public void Reborn();
    }
}