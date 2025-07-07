using NotBubbleFall.Signals;
using NotBubbleFall.UI;
using UnityEngine;

namespace NotBubbleFall.Services
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        private IFieldController _fieldController;
        private IAnimatable _startMenu;
        private IAnimatable _HUD;

        private bool _isStarting = false;

        public async void StartGame()
        {
            if (_isStarting) return;
            _isStarting = true;

            await _startMenu.PlayAnimation("Hide");
            await _HUD.PlayAnimation("Show");

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
            SignalBus.Subscribe<PlayButtonPressedSignal>(OnPlayButtonPressed);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<IGameManager>(this);
            SignalBus.Unsubscribe<PlayButtonPressedSignal>(OnPlayButtonPressed);
        }

        private void Start()
        {
            _fieldController = ServiceLocator.Resolve<IFieldController>();
            _startMenu = ServiceLocator.Resolve<StartMenu>();
            _HUD = ServiceLocator.Resolve<HUD>();
        }

        private void OnPlayButtonPressed(object sender, PlayButtonPressedSignal signalData)
        {
            StartGame();
        }
    }
}

