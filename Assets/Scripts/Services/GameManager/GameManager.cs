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
        private IAnimatable _endMenu;

        private bool _isProcessing = false;
        private bool _isGameRunning = false;

        public bool IsGameRunning => _isGameRunning;

        public async void StartGame()
        {
            if (_isProcessing) return;
            _isProcessing = true;

            await _startMenu.PlayAnimation("Hide");
            await _HUD.PlayAnimation("Show");

            _fieldController.StardField();

            _isProcessing = false;
            _isGameRunning = true;
        }

        public async void EndGame()
        {
            if (_isProcessing) return;
            _isProcessing = true;

            _fieldController.StopField();

            await _HUD.PlayAnimation("Hide");
            await _endMenu.PlayAnimation("Show");

            _isProcessing = false;
            _isGameRunning = false;
        }

        public async void RestartGame()
        {
            if (_isProcessing) return;
            _isProcessing = true;

            _fieldController.ClearField();

            await _endMenu.PlayAnimation("Hide");
            await _HUD.PlayAnimation("Show");

            _fieldController.StardField();

            _isProcessing = false;
            _isGameRunning = true;
        }

        public async void StopGame()
        {
            if (_isProcessing) return;
            _isProcessing = true;

            _fieldController.StopField();
            _fieldController.ClearField();

            await _endMenu.PlayAnimation("Hide");
            await _startMenu.PlayAnimation("Show");

            _isProcessing = false;
            _isGameRunning = false;
        }

        private void Awake()
        {
            ServiceLocator.Register<IGameManager>(this);
            SignalBus.Subscribe<PlayButtonPressedSignal>(OnPlayButtonPressed);
            SignalBus.Subscribe<HomeButtonPressedSignal>(OnHomeButtonPressed);
            SignalBus.Subscribe<AgainButtonPressedSignal>(OnAgainButtonPressed);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<IGameManager>(this);
            SignalBus.Unsubscribe<PlayButtonPressedSignal>(OnPlayButtonPressed);
            SignalBus.Unsubscribe<HomeButtonPressedSignal>(OnHomeButtonPressed);
            SignalBus.Unsubscribe<AgainButtonPressedSignal>(OnAgainButtonPressed);
        }

        private void Start()
        {
            _fieldController = ServiceLocator.Resolve<IFieldController>();
            _startMenu = ServiceLocator.Resolve<StartMenu>();
            _HUD = ServiceLocator.Resolve<HUD>();
            _endMenu = ServiceLocator.Resolve<EndMenu>();
        }

        private void OnPlayButtonPressed(object sender, PlayButtonPressedSignal signalData)
        {
            StartGame();
        }

        private void OnHomeButtonPressed(object sender, HomeButtonPressedSignal signalData)
        {
            StopGame();
        }

        private void OnAgainButtonPressed(object sender, AgainButtonPressedSignal signalData)
        {
            RestartGame();
        }
    }
}

