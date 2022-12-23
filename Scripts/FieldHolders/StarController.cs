using System;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts;
using UnityEngine;

namespace Games.InCircles.Scripts.FieldHolders
{
    [Flags]
    public enum StarPartType
    {
        LeftBottom = 1 << 1,
        RightBottom = 1 << 2,
        LeftTop = 1 << 3,
        RightTop = 1 << 4
    }

    public class StarController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer starPartsPlace;
        [SerializeField] private Transform starStand;

        [SerializeField] private List<StarPartToArrayTuple> allArrays;

        private StarPartType _currentStarPartType;
        private Stack<StarPartToArrayTuple> _stack;

        private void Awake()
        {
            ResetStar();
            _stack = new Stack<StarPartToArrayTuple>();
        }

        public Vector3 GetStarStandPosition() => starStand.position;

        // private Stack<StarPartToArrayTuple> GetRandomStack(StarParts starPart)
        // {
        //     var starPartToArrayTuple = allArrays
        //             .Where(a => GetNumberOfOnes((int)a.StarPartTyp) == (int)starPart )
        //             .ToArray();
        //     
        //     Utility.ShuffleArray(starPartToArrayTuple);
        //     return new Stack<StarPartToArrayTuple>(starPartToArrayTuple);
        // }

        public void ResetStar()
        {
            starPartsPlace.sprite = null;
            _currentStarPartType = 0;
        }

        private static int GetNumberOfOnes(int value)
        {
            var ones = 0;

            while (value != 0)
            {
                if ((value & 1) == 1) ones++;
                value = (value >> 1);
            }

            return ones;
        }

        public StarPartToArrayTuple GetStarPartSprite(StarPartType part)
        {
            // if (_stack.Count == 0) _stack = GetRandomStack(part);
            // return _stack.Pop();
            return allArrays.Find(a => a == part);
        }

        public void AddStarPart(StarPartType type)
        {
            _currentStarPartType |= type;
            starPartsPlace.sprite = allArrays.Find(a => a == _currentStarPartType).CurrentSprite;
        }
    }

    [Serializable]
    public class StarPartToArrayTuple : IEquatable<StarPartType>
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((StarPartToArrayTuple)obj);
        }

        [SerializeField] private StarPartType starPart;
        [SerializeField] private Sprite spriteArray;

        public Sprite CurrentSprite => spriteArray;
        public StarPartType StarPartTyp => starPart;

        protected bool Equals(StarPartToArrayTuple other)
        {
            return starPart == other.starPart && Equals(spriteArray, other.spriteArray);
        }

        public static bool operator ==(StarPartToArrayTuple c1,StarPartType c2) => c1 != null && c1.StarPartTyp == c2;
        public static bool operator !=(StarPartToArrayTuple c1, StarPartType c2) => !(c1 == c2);

        public bool Equals(StarPartType other)
        {
            return starPart == other;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)starPart * 397) ^ (spriteArray != null ? spriteArray.GetHashCode() : 0);
            }
        }
    }
}