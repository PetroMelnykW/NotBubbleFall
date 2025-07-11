namespace NotBubbleFall.Gameplay
{
    public class RocketProjectile : Projectile
    {
        const float CustomSpeed = 15f;

        private IFieldController _fieldController;

        private void Start()
        {
            _fieldController = ServiceLocator.Resolve<IFieldController>();
            SetCustomSpeed(CustomSpeed);
        }

        protected override void OnHitBubble(Bubble hitBubble)
        {
            _fieldController.PopBubble(hitBubble);
        }
    }
}

