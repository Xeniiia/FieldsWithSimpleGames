using System;
using Games.InCircles.Scripts.FieldHolders;
using Games.InCircles.Scripts.FieldHolders.Fields;
using UnityEngine;

namespace Games.InCircles.Scripts.Exercises
{
    public abstract class Exercise : MonoBehaviour, IExercise
    {
        [SerializeField] protected string nameEx;
        public bool IsCompleted { get; set; }
        public event Action<GameObject> Completed;


        [Sirenix.OdinInspector.Button]
        protected virtual void TaskCompleted()
        {
            if (IsCompleted) return;
            Debug.Log(nameEx + " completed");
            IsCompleted = true;
            Completed?.Invoke(gameObject);
        }


        public abstract void Generate(ContentDto contentDto, ItemSizeDto itemSizeDto, int itemsOnGrid,
            Vector3 direction);

        public virtual void ForceComplete()
        {
            TaskCompleted();
        }

        public virtual void Destroy()
        {
            IsCompleted = false;
        }
    }
}