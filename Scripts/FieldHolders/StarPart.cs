using System;
using System.Collections;
using Main.Scripts.HelpfulSharedScripts.Functional;
using UnityEngine;

namespace Games.InCircles.Scripts.FieldHolders
{
    public class StarPart : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer starPartSprite;
        [SerializeField] private ParticleSystem appearanceEffect;
        [SerializeField] private ParticleSystem finalEffect;

        private StarPartType _currentType;
        private Transform _transform;
        private Animator _animator;
        private static readonly int StartMove = Animator.StringToHash("StartMove");
        

        private void Awake()
        {
            _transform = transform;
            starPartSprite = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        public void Init(Sprite starSprite)
        {
            starPartSprite.sprite = starSprite;
            starPartSprite.color = Color.clear; 
        }


        public void MoveToStarStand(Vector3 getStarStandPosition, Action exerciseCompleteHandle)
        {
            StartCoroutine(MoveAndHandle(getStarStandPosition, exerciseCompleteHandle));
        }
        
        //Added by Alex K 
        private IEnumerator MoveAndHandle(Vector3 target, Action completeHandle)
        {
            appearanceEffect.Play();
            
            yield return new YieldElapsedTime(0.6f, f => { starPartSprite.color = Color.Lerp(Color.clear, Color.white, f); });
            
            _animator.SetTrigger(StartMove);
            
            var startPosition = _transform.position;
            yield return new YieldElapsedTime(1f, f => { _transform.position = Vector3.Lerp(startPosition, target, f); });
            
            finalEffect.Play();
            starPartSprite.sprite = null;
            
            completeHandle?.Invoke();
          
            Destroy(gameObject);
        }
    }
}