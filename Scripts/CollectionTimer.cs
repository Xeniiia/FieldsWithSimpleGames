using System.Collections.Generic;
using System.Linq;
using Main.Scripts;
using UnityEngine;

namespace Games.InCircles.Scripts
{
    public class CollectionTimer : MonoBehaviour, ITimerTick, IOverlayCanvas
    {
        [SerializeField] private List<TimerTickDefault> timers;
    
        public void TimerStart(float seconds)
        {
            if (timers?.FirstOrDefault() == null) return;
        
            foreach (var timer in timers) timer.TimerStart(seconds);
        }

        public void TimerEnd()
        {
            if (timers?.FirstOrDefault() == null) return;
        
            foreach (var timer in timers) timer.TimerEnd();
        }

        public void TimerTick(float progress)
        {
            if (timers?.FirstOrDefault() == null) return;

            foreach (var timer in timers)
            {
                timer.TimerTick(progress);
            }
        }

        public void ShadeVisible(bool isShow)
        {
            if (timers?.FirstOrDefault() == null) return;
        
            foreach (var timer in timers) timer.ShadeVisible(isShow);
        }
    }
}
