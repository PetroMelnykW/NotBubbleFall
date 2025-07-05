using UnityEngine;

namespace NotBubbleFall.Services
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        private IFieldController _fieldController;

        public void StartGame()
        {
            _fieldController.StardField();
        }

        public void EndGame()
        {
            _fieldController.StopField();
        }

        public void ResetGame()
        {
            _fieldController.StopField();
            _fieldController.ResetField();
        }

        private void Awake()
        {
            ServiceLocator.Register<IGameManager>(this);

        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<IGameManager>(this);

        }

        private void Start()
        {
            _fieldController = ServiceLocator.Resolve<IFieldController>();
            _fieldController.StardField();
        }
    }
}

