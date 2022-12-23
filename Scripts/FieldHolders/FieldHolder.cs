using System;
using System.Collections;
using System.Collections.Generic;
using Games.InCircles.Scripts.FieldHolders.Fields;
using Games.InCircles.Scripts.Backgrounds.Factories;
using Games.InCircles.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Games.InCircles.Scripts.FieldHolders
{
    [Serializable]
    public class FieldHolder : MonoBehaviour, IFieldHolder
    {
        [SerializeField] private Fields.Field[] fields;
        [SerializeField] private TransferView[] transferViews;
        [SerializeField] private Canvas transferViewsCanvas;
        // [SerializeField] private StarParts starPartsType;
        [SerializeField] private StarController starController;
        private IStarPartFactory _starPartFactory;
        private BackgroundFactory _backgroundFactory;
        private TransferView _currentTransferView;
        private List<IExercise> _exercises = new List<IExercise>();
        private int _completedExercises;
        private int _filledFields;
        private int _completedExercisesEveryGame;
        public event Action CircleCompleted;
        public event Action<TransferView> FieldsExerciseCompleted;

        private int Length => fields.Length;
        private void Awake()
        {
            UpdateTransferView();
        }

        private void UpdateTransferView()
        {
            if (fields.Length != 1) _currentTransferView = transferViews[Random.Range(0, transferViews.Length)];
        }


        public void SetBackgroundFactory(BackgroundFactory backgroundFactory) => _backgroundFactory = backgroundFactory;
        public void SetStarPartFactory(IStarPartFactory starPartFactory) => _starPartFactory = starPartFactory;

        
        public bool InitFields(IExerciseFactory exerciseFactory)
        {
            var backgroundPrefab = _backgroundFactory.GetBackground();
            fields[_filledFields].InitField(exerciseFactory, backgroundPrefab);
            _filledFields++;
            
            return _filledFields < fields.Length;
        }

        
        private void OnEnable()
        {
            foreach (var field in fields)
            {
                field.ExerciseSet += FieldOnExerciseSet;
            }
        }

        private void FieldOnExerciseSet(IExercise exercise,StarPartType starPartType)
        {
            _exercises.Add(exercise);
            exercise.Completed += (x) =>ExerciseOnCompleted(x, starPartType);
        }

        private void ExerciseOnCompleted(GameObject completedExercise, StarPartType starPartType)  //todo: тут каждый раз звездочку создавать
        {
            var spriteTypeTuple = starController.GetStarPartSprite(starPartType);
            var starPartInstance = _starPartFactory.GetStarPart(spriteTypeTuple.CurrentSprite, completedExercise.transform.position, spriteTypeTuple.StarPartTyp);
            starPartInstance.MoveToStarStand(starController.GetStarStandPosition(),() =>
                StartCoroutine(ExerciseCompleteHandle(spriteTypeTuple.StarPartTyp)));
        }

        private IEnumerator ExerciseCompleteHandle(StarPartType type)
        {
            // starController.AddStarPart(starPartsType,type);
            starController.AddStarPart(type);
            yield return new WaitForSeconds(2f);
            _completedExercises++;
            _completedExercisesEveryGame++;
            if (_completedExercises == Length * Length)
            {

                ClearField();
                _filledFields = 0;
                _completedExercises = 0;
                _completedExercisesEveryGame = 0;
                UpdateTransferView();
                CircleCompleted?.Invoke();
            }
            
            else if (_completedExercisesEveryGame == Length)
            {
                _completedExercisesEveryGame = 0;
                FieldsExerciseCompleted?.Invoke(_currentTransferView);
            }
            
        }

        
        
        public void LoadNewExercise()
        {
            ClearField();

            foreach (var field in fields)
            {
                field.LoadExercise();
            }
        }

        private void ClearField()
        {
            starController.ResetStar();
            foreach (var exercise in _exercises)
            {
                // exercise.Completed -= ExerciseOnCompleted;
                exercise.Destroy();
            }

            _exercises.Clear();
        }

        
        public void Unload()
        {
            ClearField();
            Destroy(gameObject);
        }

        
        [Sirenix.OdinInspector.Button]
        public void ForcedWin()
        {
            foreach (var exercise in _exercises)
            {
                exercise.ForceComplete();
            }
        }

        public void SetCameraUI(Camera cameraUI)
        {
            foreach (var field in fields)
            {
                field.SetCamera(cameraUI);
            }

            if (transferViewsCanvas != null)
            {
                transferViewsCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                transferViewsCanvas.worldCamera = cameraUI;
                transferViewsCanvas.sortingOrder = 30;
            }
        }
    }

    public enum StarParts
    {
        Quarter = 1,
        Half = 2,
        Full = 4
    }
}