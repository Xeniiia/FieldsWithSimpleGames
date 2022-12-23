using Main.Menu.NewMenu.Scripts.Items;
using Main.Menu.NewMenu.Scripts.Screens;
using UnityEngine;

namespace Games.InCircles.Scripts.UI
{
    public class StartPopupView : MonoBehaviour
    {
        [SerializeField] private MenuButton startButton;
        [SerializeField] private UIView currentView;
        [SerializeField] private UIView nextView;

        public bool IsVisible
        {
            get => currentView.IsVisible;
            set => currentView.IsVisible = value;
        }

        protected  void OnEnable()
        {
            startButton.OnClickEvent += NextWindow;
        }

        private void NextWindow()
        {
            IsVisible = false;
            nextView.IsVisible = true;
        }
    }
}