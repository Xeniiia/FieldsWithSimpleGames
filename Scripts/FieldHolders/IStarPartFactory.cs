using UnityEngine;

namespace Games.InCircles.Scripts.FieldHolders
{
    public interface IStarPartFactory
    {
        StarPart GetStarPart(Sprite starSprite, Vector3 transformPosition, StarPartType currentType);
    }
}