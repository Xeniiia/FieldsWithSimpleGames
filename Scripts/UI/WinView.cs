using System;
using System.Collections;
using Backend.Scripts.Main;
using Main.Menu.NewMenu.Scripts.Notifications.Common;
using Main.Scripts.HelpfulSharedScripts.Functional;
using UnityEngine;

namespace Games.InCircles.Scripts.UI
{
    [DrawProperties("waitingTime")]
    public class WinView : NotificationView
    {
        [SerializeField] private float waitingTime;
        public event Action WinViewWasHide;

        private Coroutine _showWaitingViewCoroutine;


        public void ShowWinView()
        {
            this.RestartCoroutine(ref _showWaitingViewCoroutine,ShowWaitingView());
        }

        private IEnumerator ShowWaitingView()
        {
            IsVisible = true;
            yield return new YieldElapsedTime(waitingTime);
            IsVisible = false;
        }

        public override void AfterHide()
        {
            base.AfterHide();
            WinViewWasHide?.Invoke();
        }
    }
}