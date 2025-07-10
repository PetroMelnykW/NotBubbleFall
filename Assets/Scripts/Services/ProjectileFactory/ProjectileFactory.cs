using NotBubbleFall.Data;
using NotBubbleFall.Gameplay;
using System.Linq;
using UnityEngine;
using Enum = System.Enum;

namespace NotBubbleFall.Services
{
    public class ProjectileFactory : IProjectileFactory
    {
        const float SpecialProjectileSpawnChance = 0.1f;

        private BubbleDB _bubbleDB;
        private ProjectileDB _projectileDB;
        private IFieldController _fieldController;

        public void Resolve()
        {
            _bubbleDB = ServiceLocator.Resolve<BubbleDB>();
            _projectileDB = ServiceLocator.Resolve<ProjectileDB>();
            _fieldController = ServiceLocator.Resolve<IFieldController>();
        }

        public void Initialize() { }

        public BubbleProjectile CreateBubbleProjectile()
        {
            var bubbleProjectileObject = Object.Instantiate(_projectileDB.BubbleProjectilePrefab);
            var bubbleProjectile = bubbleProjectileObject.GetComponent<BubbleProjectile>();
            var bubble = bubbleProjectileObject.GetComponent<Bubble>();
            
            bubble.SetColor(_bubbleDB.GetUnlockedColorsForPhase(_fieldController.CurrentPhase).PickRandom());

            return bubbleProjectile;
        }

        public Projectile CreateSpecialProjectile(SpecialProjectileType projectileType)
        {
            var projectileObject = Object.Instantiate(_projectileDB.GetSpecialProjectilePrefab(projectileType));
            var projectile = projectileObject.GetComponent<Projectile>();

            return projectile;
        }

        public Projectile CreateRandomProjectile()
        {
            if (Random.value < SpecialProjectileSpawnChance)
            {
                var randomType = Enum.GetValues(typeof(SpecialProjectileType)).Cast<SpecialProjectileType>().PickRandom();

                return CreateSpecialProjectile(randomType);
            }

            return CreateBubbleProjectile();
        }
    }

}