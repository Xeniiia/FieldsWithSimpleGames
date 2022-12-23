using System;
using System.Collections;
using Backend.Scripts.Main;
using Main.Menu.NewMenu.Scripts.Notifications.Common;
using Main.Scripts.HelpfulSharedScripts.Functional;
using UnityEngine;

namespace Games.InCircles.Scripts.UI
{
    [DrawProperties(nameof(timer))]
    public class TransferView : NotificationView
    {
        [SerializeField] private CollectionTimer timer;
        public event Action TransferViewWasHide;

        private Coroutine _showWaitingViewCoroutine;


        public void ShowTransferView(float waitingTime)
        {
            this.RestartCoroutine(ref _showWaitingViewCoroutine, ShowWaitingView(waitingTime));
        }

        public override void BeforeHide()
        {
            base.BeforeHide();
            if(_showWaitingViewCoroutine != null) StopCoroutine(_showWaitingViewCoroutine);
        }

        private IEnumerator ShowWaitingView(float waitingTime)
        {
            IsVisible = true;

            timer.TimerStart(waitingTime);
            yield return new YieldElapsedTime(waitingTime, f => { timer.TimerTick(f); });

            IsVisible = false;
        }


        public override void AfterHide()
        {
            base.AfterHide();
            TransferViewWasHide?.Invoke();
        }
    }
}