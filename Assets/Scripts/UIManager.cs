using DG.Tweening;

using UnityEngine;

namespace Kawzar.Memento.Scripts
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup mainMenu;

        [SerializeField]
        private CanvasGroup gameplayMenu;

        public static UIManager Instance { get; private set; }

        public void ShowMainMenu()
        {
            gameplayMenu?.DOFade(0, 0.25f)
                .OnComplete(() =>
                {
                    GameManager.Instance.Player.gameObject.SetActive(false);
                    mainMenu.gameObject.SetActive(true);
                    mainMenu.DOFade(1, 0.25f);
                });
        }

        public void ShowGameplayMenu()
        {
            mainMenu?.DOFade(0, 0.25f)
                   .OnComplete(() =>
                   {
                       gameplayMenu.gameObject.SetActive(true);
                       gameplayMenu.DOFade(1, 0.25f);
                   });
        }
    }
}
