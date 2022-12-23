using UnityEngine;

namespace Games.InCircles.Scripts.FieldHolders.Fields
{
    public interface IExerciseFactory
    {
        IExercise GetExercise(ContentDto contentDto, ItemSizeDto itemSizeDto, int itemsOnGrid, Vector3 direction);
        string GetName();
    }
}