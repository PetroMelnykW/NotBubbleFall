using NotBubbleFall.Data;
using NotBubbleFall.Gameplay;

namespace NotBubbleFall
{
    public interface IProjectileFactory : IInitializableService
    {
        BubbleProjectile CreateBubbleProjectile();
        Projectile CreateSpecialProjectile(SpecialProjectileType projectileType);
        Projectile CreateRandomProjectile();
    }
}


