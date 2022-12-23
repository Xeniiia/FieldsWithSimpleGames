using System;
using System.Collections;
using Backend.Scripts.Main;
using OpenCVForUnity.CoreModule;
using UnityEngine;
using UnityEngine.UI;

namespace Games.InCircles.Scripts.Exercises.Games
{
    public class ContentItem : MonoBehaviour, ICell
    {
        [SerializeField] protected ParticleSystem _particleWin;
        [Range(0, 20)]
        [SerializeField] private int differenceDepth = 10;
        
        protected Image image;
        private RectTransform _rectTransform;
        private Animator _animator;
        private Coroutine _calibrateCoroutine;
        
        private static readonly int Wrong = Animator.StringToHash("Wrong");
        private static readonly int Delete = Animator.StringToHash("Delete");
        private int _depthAtStart;
        
        protected bool selectable;
        private bool _isWinner;
        private bool _calibrated;
        
        
        public event Action<ContentItem> SelectedTrue;

        private bool IsAnswer { get; set; }
        public int MiddleDepth => IsAnswer ? this.GetMiddleDepth() : 0;
        public int DefaultMiddleDepth => 0;
        public RotatedRect Rectangle { get; set; }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _animator = GetComponent<Animator>();
            image = GetComponent<Image>();
        }
        
        private void Update()
        {
            if (IsAnswer && Difference() > differenceDepth)
            {
                TrySelect();
            }
        }

        private int Difference()
        {
            var curDepth = MiddleDepth;
            // Debug.Log($"check different between {_depthAtStart} and {curDepth}");
            return curDepth - _depthAtStart;
        }
        
        private void OnMouseDown()
        {
            TrySelect();
        }

        protected virtual void TrySelect()
        {
            if (_isWinner && selectable)
            {
                ViewAsCorrect();
            }

            if (!_isWinner && selectable)
            {
                ViewAsWrong();
            }
        }

        protected void ViewAsCorrect()
        {
            _particleWin.Play();
            MarkAsCorrect();
        }

        private void MarkAsCorrect()
        {
            if (_calibrateCoroutine != null) StopCoroutine(_calibrateCoroutine);
            selectable = false;
            SelectedTrue?.Invoke(this);
        }

        protected void ViewAsWrong()
        {
            _animator.SetTrigger(Wrong);
        }


        public void SetSprite(Sprite sprite, Vector2 itemSize, float particlesRadius, bool selectable,
            bool isWinner = false)
        {
            _rectTransform.localScale = itemSize;
            var particleWinShape = _particleWin.shape;
            particleWinShape.radius = particlesRadius;
            image.sprite = sprite;
            _isWinner = isWinner;
            this.selectable = selectable;
        }
        

        public void StartDestroyItem()
        {
            _animator.SetTrigger(Delete);
        }

        private void DestroyItem()
        {
            Destroy(gameObject);
        }

        
        public void SetUnselectable()
        {
            selectable = false;
        }

        
        public void SetAsAnswer()
        {
            StartCoroutine(UpdateRectangle());
        }
        
        
        private IEnumerator UpdateRectangle()
        {
            yield return new WaitForEndOfFrame();
            this.GetRectangleFromCanvas();
            IsAnswer = true;
            _depthAtStart = MiddleDepth;
        }
    }
}