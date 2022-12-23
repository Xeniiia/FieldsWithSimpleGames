using System.Collections.Generic;
using System.Linq;
using Games.InCircles.Scripts.Exercises.Factories;
using Games.InCircles.Scripts.FieldHolders.Fields;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Games.InCircles.Scripts.Categories.Factories
{
    public class CategoryFactory : MonoBehaviour, ICategoryFactory
    {
        [SerializeField] private List<ExerciseFactory> modeToFactoryPair;
        private List<ExerciseFactory> _factories;
        
        private List<ExerciseFactory> _categoryAvailable = new List<ExerciseFactory>();

        private void Awake()
        {
            _factories = new List<ExerciseFactory>();
        }

        public IExerciseFactory GetCategory()
        {
            if (_categoryAvailable.Count == 0) ResetExercises();

            var rnd = Random.Range(0, _categoryAvailable.Count);
            var categoryRef = _categoryAvailable[rnd];
            _categoryAvailable.RemoveAt(rnd);
            
            return categoryRef;
        }

        private void ResetExercises()
        {
            _categoryAvailable = _factories.Where(x => x != null).ToList();
        }

        public void SetCategory(GameMode byMode)
        {
            _factories.Clear();
            foreach (var factory in modeToFactoryPair)
            {
                if (byMode.HasFlag(factory.Mode) && !_factories.Contains(factory))
                {
                    _factories.Add(factory);
                }
            }
        }
    }
}