using Games.InCircles.Scripts.Categories.Factories;
using Games.InCircles.Scripts.FieldHolders.Fields;

namespace Games.InCircles.Scripts
{
    public interface ICategoryFactory
    {
        IExerciseFactory GetCategory();
        void SetCategory(GameMode mode);
    }
}