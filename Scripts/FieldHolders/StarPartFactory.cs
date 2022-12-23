using UnityEngine;

namespace Games.InCircles.Scripts.FieldHolders
{
    public class StarPartFactory : MonoBehaviour, IStarPartFactory
    {
        [SerializeField] private StarPart proto;
        public StarPart GetStarPart(Sprite starSprite, Vector3 transformPosition, StarPartType currentType)
        {
            var starPart = Instantiate(proto, transformPosition, Quaternion.identity);
            starPart.Init(starSprite);
            
            return starPart;
        }
    }
}