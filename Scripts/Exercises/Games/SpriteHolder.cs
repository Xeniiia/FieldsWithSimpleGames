using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Games.InCircles.Scripts.Exercises.Games
{
    [Serializable]
    public class SpriteHolder
    {
        [SerializeField] public Sprite[] allSprites;

        public Sprite[] GetRandomSprites(int count)
        {
            List<Sprite> sprites = allSprites.ToList();
            Sprite[] randomSprites = new Sprite[count];
            for (int i = 0; i < count; i++)
            {
                var rnd = Random.Range(0, sprites.Count);
                randomSprites[i] = sprites[rnd];
                sprites.RemoveAt(rnd);
            }

            return randomSprites;
        }

        public Sprite[] GetRangeSprites(int from, int count)
        {
            List<Sprite> sprites = allSprites.ToList();
            return sprites.GetRange(from, count).ToArray();
        }


        public int GetRandomIndex(int forCount)
        {
            var availableCount = allSprites.Length - forCount;
            var rnd = Random.Range(0, availableCount);
            
            return rnd;
        }
    }
}