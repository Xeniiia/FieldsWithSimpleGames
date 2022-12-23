using System;
using System.Collections;
using Games.InCircles.Scripts.FieldHolders.Fields;
using UnityEngine;

namespace Games.InCircles.Scripts.FieldHolders
{
    public interface IExercise
    {
        bool IsCompleted { get; set; }
        event Action<GameObject> Completed;
        void Generate(ContentDto contentDto, ItemSizeDto itemSizeDto, int itemsOnGrid, Vector3 direction);
        void ForceComplete();

        void Destroy();
    }
}