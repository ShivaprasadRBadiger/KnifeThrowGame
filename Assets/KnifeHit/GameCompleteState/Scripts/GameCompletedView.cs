using System;
using EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.KnifeHit.GameCompleteState
{
   public class GameCompletedView : MonoBehaviour
   {

      public enum CompletionState
      {
         Won,
         Lost
      }
   
      [SerializeField]
      Settings settings;
      private void Awake()
      {
         settings.exitToMenu.onClick.AddListener(OnGameRestartClicked);
         EventManager.RegisterHandler(CustomEventType.OnLevelCompleted,OnLevelCompletedHandler);
         EventManager.RegisterHandler(CustomEventType.OnLevelFailed,OnLevelFailedHandler);
         gameObject.SetActive(false);
      }

      private void OnLevelFailedHandler(object obj)
      {
        Invoke(nameof(ShowLostScreen),1f);
      }

      private void ShowLostScreen()
      {
         Initialize(CompletionState.Lost);
      }

      private void ShowWonScreen()
      {
         Initialize(CompletionState.Won);
      }
     

      private void OnLevelCompletedHandler(object obj)
      {
         Invoke(nameof(ShowWonScreen),1f);
      }

      private void OnGameRestartClicked()
      {
         SceneManager.LoadScene(0);
      }

      public void Initialize(CompletionState state)
      {
         switch (state)
         {
            case CompletionState.Lost:
               settings.titleText.text = "YOU LOST!";
               break;
            case CompletionState.Won:
               settings.titleText.text = "YOU WON!";
               break;
            default:
               throw new ArgumentOutOfRangeException(nameof(state), state, null);
         }
         gameObject.SetActive(true);
      }

      [Serializable]
      public struct Settings
      {
         public TextMeshProUGUI titleText;
         public Button exitToMenu;
      }
   }
}
