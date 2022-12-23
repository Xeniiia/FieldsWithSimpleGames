using System;
using Games.InCircles.Scripts.FieldHolders.Fields;
using Games.InCircles.Scripts.UI;

namespace Games.InCircles.Scripts
{
    public interface IFieldHolder
    {
        bool InitFields(IExerciseFactory exerciseFactory);
        event Action CircleCompleted;
        event Action<TransferView> FieldsExerciseCompleted;
        void ForcedWin();
        void LoadNewExercise();
        void Unload();
    }
}