using System;
using Games.InCircles.Scripts.Backgrounds.Factories;
using Games.InCircles.Scripts.Categories.Factories;
using Games.InCircles.Scripts.FieldHolders;
using Games.InCircles.Scripts.UI;
using UnityEngine;

namespace Games.InCircles.Scripts
{
    public class FasadForGame : MonoBehaviour //todo: сменить название с рабочего
    {
        [SerializeField] private Camera cameraUI;
        [SerializeField] private FieldHolder[] fieldHolders;
        private ICategoryFactory _categoryFactory;
        private BackgroundFactory _backgroundFactory;
        private IFieldHolder _fieldHolder;
        private GameMode _currentGameMode = GameMode.SelectAll;
        public event Action<TransferView> ExercisesCompleted; 
        public event Action CircleExercisesCompleted; 


        private void Awake()
        {
            _categoryFactory = GetComponent<ICategoryFactory>();
            _backgroundFactory = GetComponent<BackgroundFactory>();
        }


        public void LoadFieldHolder(int playersCount,GameMode mode)
        {
            _currentGameMode = mode;
            var fieldHolderProto = playersCount switch
            {
                2 => fieldHolders[1],
                4 => fieldHolders[2],
                _ => fieldHolders[0]
            };
            _fieldHolder = SpawnFieldHolder(fieldHolderProto);
            InitCategories();
            _fieldHolder.CircleCompleted += CircleCompleted;
            _fieldHolder.FieldsExerciseCompleted += OnExerciseCompleted;
        }

        private IFieldHolder SpawnFieldHolder(FieldHolder fieldHolderProto)
        {
            var starPartFactory = GetComponent<IStarPartFactory>();
            var fieldHolder = Instantiate(fieldHolderProto, Vector3.zero, Quaternion.identity);
            fieldHolder.SetBackgroundFactory(_backgroundFactory);
            fieldHolder.SetStarPartFactory(starPartFactory);
            fieldHolder.SetCameraUI(cameraUI);

            return fieldHolder;
        }

        private void InitCategories()
        {
            _categoryFactory.SetCategory(_currentGameMode);
            bool isContinue;
            do
            {
                var exerciseFactory = _categoryFactory.GetCategory();
                isContinue = _fieldHolder.InitFields(exerciseFactory);
            } while (isContinue);
        }

        private void CircleCompleted()
        {
            Debug.Log("Circle completed");  /////////////
            CircleExercisesCompleted?.Invoke();
        }

        public void LoadNewCategories()
        {
            InitCategories();
        }

        private void OnExerciseCompleted(TransferView transferView)
        {
            ExercisesCompleted?.Invoke(transferView);
        }

        public void LoadNewExercisesOnFields()
        {
            _fieldHolder.LoadNewExercise();
        }


        public void ForcedWin()
        {
            _fieldHolder.ForcedWin();
        }

        private void OnDisable()
        {
            if (_fieldHolder == null) return;
            _fieldHolder.CircleCompleted -= CircleCompleted;
            _fieldHolder.FieldsExerciseCompleted -= OnExerciseCompleted;
        }

        public void UnloadFieldHolder()
        {
            if (_fieldHolder == null) return;
            _fieldHolder.Unload();
        }
    }
}