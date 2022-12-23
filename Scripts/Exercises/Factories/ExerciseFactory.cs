using System.Collections.Generic;
using System.Linq;
using Games.InCircles.Scripts.Categories.Factories;
using Games.InCircles.Scripts.FieldHolders;
using Games.InCircles.Scripts.FieldHolders.Fields;
using Localization.Sample.Scripts;
using UnityEngine;

namespace Games.InCircles.Scripts.Exercises.Factories
{
    [System.Serializable]
    public abstract class ExerciseFactory : MonoBehaviour, IExerciseFactory
    {
        [SerializeField] private GameMode mode;
        [SerializeField] private Exercise[] exercises;
        [SerializeField] private string exercisesName;
        private List<Exercise> _exercisesAvailable = new List<Exercise>();

        public GameMode Mode => mode;
        
        public string GetName() => exercisesName.AsLocalizedKey("Games_InCircles");


        public virtual IExercise GetExercise(ContentDto contentDto, ItemSizeDto itemSizeDto, int itemsOnGrid, Vector3 direction)
        {
            if (_exercisesAvailable.Count == 0)
            {
                ResetExercises();
            }
            
            var rnd = Random.Range(0, _exercisesAvailable.Count);
            var exerciseRef = _exercisesAvailable[rnd];
            _exercisesAvailable.RemoveAt(rnd);
            var canvas = contentDto.GridContent.GetComponentInParent<Canvas>();
            var exercise = Instantiate(exerciseRef, Vector3.zero, Quaternion.identity, canvas.transform);
            exercise.transform.localPosition = Vector3.zero;
            exercise.Generate(contentDto, itemSizeDto, itemsOnGrid, direction);
            
            return exercise;
        }

        private void ResetExercises()
        {
            _exercisesAvailable = exercises.ToList();
        }
    }
}