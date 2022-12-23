using System;
using RTLTMPro;
using UnityEngine;

namespace Games.InCircles.Scripts.FieldHolders.Fields
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private RTLTextMeshPro exerciseName;
        [SerializeField] private Transform substrate;
        [SerializeField] private RectTransform gridContent;
        [SerializeField] private RectTransform exampleContent;
        [SerializeField] private RectTransform answerContent;
        [SerializeField] private RectTransform coupleContent;
        [SerializeField] private RectTransform gridOrderContent;
        [SerializeField] private float direction;
        [SerializeField] private int itemsOnGrid;
        [SerializeField] private Vector2 scaleForItems;
        [SerializeField] private Vector2 scaleForExampleItems;
        [SerializeField] private Vector2 scaleForAnswerItems;
        [SerializeField] private float particlesRadius;
        
        [SerializeField] private StarPartType startPieceOnField;

        private IExerciseFactory _exerciseFactory;
        private GameObject _backgroundPrefab;

        public event Action<IExercise,StarPartType> ExerciseSet;


        public void InitField(IExerciseFactory exerciseFactory, GameObject backgroundPrefab)
        {
            exerciseName.text = exerciseFactory.GetName();
            _exerciseFactory = exerciseFactory;
            SetBackground(backgroundPrefab);
            LoadExercise();
        }

        private void SetBackground(GameObject backgroundPrefab)
        {
            if (_backgroundPrefab != null)
            {
                Destroy(_backgroundPrefab.gameObject);
            }
            _backgroundPrefab = Instantiate(backgroundPrefab, substrate.transform.position, Quaternion.Euler(0,0,direction), substrate);
        }

        public void LoadExercise()
        {
            var contentDto = new ContentDto()
            {
                GridContent = gridContent,
                AnswerContent = answerContent,
                ExampleContent = exampleContent,
                CoupleContent = coupleContent,
                GridOrderContent = gridOrderContent
            };
            var itemSizeDto = new ItemSizeDto()
            {
                ScaleForItems = scaleForItems,
                ScaleForExampleItems = scaleForExampleItems,
                ScaleForAnswerItems = scaleForAnswerItems,
                ParticlesRadius = particlesRadius
            };
            
            
            var exercise = _exerciseFactory.GetExercise(contentDto, itemSizeDto, itemsOnGrid, new Vector3(0, 0, direction));
            ExerciseSet?.Invoke(exercise,startPieceOnField);
        }

        public void SetCamera(Camera cameraUI)
        {
            var canvas = GetComponentInChildren<Canvas>();
            canvas.worldCamera = cameraUI;
        }
    }

    public struct ContentDto
    {
        public RectTransform GridContent { get; set; }
        public RectTransform ExampleContent { get; set; }
        public RectTransform AnswerContent { get; set; }
        public RectTransform CoupleContent { get; set; }
        public RectTransform GridOrderContent { get; set; }
    }

    public struct ItemSizeDto
    {
        public Vector2 ScaleForItems { get; set; }
        public Vector2 ScaleForExampleItems { get; set; }
        public Vector2 ScaleForAnswerItems { get; set; }
        public float ParticlesRadius { get; set; }
    }
}