using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Games.InCircles.Scripts.Exercises.Games
{
    public class ContentItemWithNum : ContentItem
    {
        [SerializeField] private Image numImage;
        [SerializeField] private Image shadow;
        [SerializeField] private Image lighting;
        private InOrder _exercise;
        private int _value;

        public void InitAsItemWithValue(int value, InOrder exercise, Sprite shadowNum,  Sprite lightingNum)
        {
            _value = value;
            _exercise = exercise;
            shadow.sprite = shadowNum;
            lighting.sprite = lightingNum;

            numImage.sprite = image.sprite;
            
            var lightingColor = lighting.color;
            lightingColor.a = 0;
            lighting.color = lightingColor;

            var shadowColor = shadow.color;
            shadowColor.a = 0;
            shadow.color = shadowColor;
        }

        
        public void HideItem()
        {
            //throw new System.NotImplementedException();
            numImage.enabled = false;
            shadow.enabled = false;
            lighting.enabled = false;
            GetComponent<Image>().enabled = false;
        }

        
        protected override void TrySelect()
        {
            if (selectable)
            {
                bool res = _exercise.UpdateCurrentNumValue(_value);
                if (res)
                {
                    ViewLight();
                    ViewAsCorrect();
                }
                else
                {
                    ViewAsWrong();
                }
            }
        }

        private void ViewLight()
        {
            StartCoroutine(SlowImageAlphaChange(lighting, 0, 100, 1));
            StartCoroutine(SlowImageAlphaChange(shadow, 0, 100, 1));
        }

        private IEnumerator SlowImageAlphaChange(Image image, float from, float to, float duration)
        {
            float time = 0;
            while (time <= duration)
            {
                time += Time.deltaTime;
                var fadeImageColor = image.color;
                fadeImageColor.a = Mathf.Lerp(from, to, time / duration);
                image.color = fadeImageColor;
                yield return null;
            }
        }
    }
}