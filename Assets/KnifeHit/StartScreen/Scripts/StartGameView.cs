using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.KnifeHit.StartScreen
{
    public class StartGameView : MonoBehaviour
    {
        [SerializeField] private Settings settings;

        [SerializeField]
        private InGameStateView inGameStateView;
        
        void Awake()
        {
            settings.startButton.onClick.AddListener(OnStartClicked);
            settings.quitButton.onClick.AddListener(OnQuitClicked);
        }

        private void OnQuitClicked()
        {
            Application.Quit();
        }

        private void OnStartClicked()
        {
            gameObject.SetActive(false);
            inGameStateView.gameObject.SetActive(true);
        }

        [Serializable]
        public struct Settings
        {
            public Button startButton;
            public Button quitButton;
        }
    }
}