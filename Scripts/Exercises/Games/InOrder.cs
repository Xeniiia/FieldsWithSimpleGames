using System.Collections.Generic;
using Games.InCircles.Scripts.FieldHolders.Fields;
using UnityEngine;

namespace Games.InCircles.Scripts.Exercises.Games
{
    public class InOrder : Exercise
    {
        [SerializeField] private ContentItemWithNum proto;
        [SerializeField] private SpriteHolder sprites;
        [SerializeField] private SpriteHolder shadows;
        [SerializeField] private SpriteHolder lightings;
        
        private List<ContentItemWithNum> items = new List<ContentItemWithNum>();
        private int _currentValue;
        private int _winValue;

        public override void Generate(ContentDto contentDto, ItemSizeDto itemSizeDto, int itemsOnGrid,
            Vector3 direction)
        {
            for (var i = 0; i < itemsOnGrid; i++)
            {
                items.Add(Instantiate(proto, Vector3.zero, Quaternion.Euler(direction), contentDto.GridOrderContent));
            }

            var countOfNums = ChangeCountOfNums(itemsOnGrid);

            var startIndex = sprites.GetRandomIndex(countOfNums);
            var numbersSprites = sprites.GetRangeSprites(startIndex, countOfNums);
            _winValue = startIndex + countOfNums - 1;
            _currentValue = startIndex - 1;

            if (itemsOnGrid == 9 && countOfNums != 9)
            {
                FillPartItems(itemSizeDto, startIndex, numbersSprites, countOfNums);
            }
            else
            {
                FillFullItems(itemSizeDto, startIndex, numbersSprites);
            }
        }

        private int ChangeCountOfNums(int itemsOnGrid)
        {
            var countOfNums = itemsOnGrid;
            if (itemsOnGrid == 9)
            {
                var rnd = Random.Range(0, 3);
                if (rnd == 0) countOfNums = 4;
                if (rnd == 1) countOfNums = 5;
            }

            return countOfNums;
        }

        private void FillFullItems(ItemSizeDto itemSizeDto, int startIndex, Sprite[] numbersSprites)
        {
            var itemsWithoutSprite = new List<ContentItemWithNum>(items);
            
            var currentValue = startIndex;
            foreach (var sprite in numbersSprites)
            {
                var rndItem = Random.Range(0, itemsWithoutSprite.Count);
                itemsWithoutSprite[rndItem].SetSprite(sprite, itemSizeDto.ScaleForItems, itemSizeDto.ParticlesRadius, true);
                itemsWithoutSprite[rndItem].SetAsAnswer();
                itemsWithoutSprite[rndItem].InitAsItemWithValue(currentValue, this, shadows.allSprites[currentValue], lightings.allSprites[currentValue]);
                itemsWithoutSprite.RemoveAt(rndItem);
                currentValue++;
            }
        }

        private void FillPartItems(ItemSizeDto itemSizeDto, int startIndex, Sprite[] numbersSprites, int countOfNums)
        {
            var itemsEven = new List<ContentItemWithNum>();
            var itemsOdd = new List<ContentItemWithNum>();
            
            var currentValue = startIndex;

            for (int i = 0; i < items.Count; i++)
            {
                if (i % 2 == 0)
                {
                    itemsEven.Add(items[i]);
                }
                else
                {
                    itemsOdd.Add(items[i]);
                }
            }

            if (countOfNums == 5)
            {
                InitItems(itemSizeDto, numbersSprites, itemsEven, currentValue, itemsOdd);
            }
            else if (countOfNums == 4)
            {
                InitItems(itemSizeDto, numbersSprites, itemsOdd, currentValue, itemsEven);
            }
        }

        private void InitItems(ItemSizeDto itemSizeDto, Sprite[] numbersSprites, List<ContentItemWithNum> itemsVisible, int currentValue,
            List<ContentItemWithNum> itemsForHide)
        {
            foreach (var sprite in numbersSprites)
            {
                var rndItem = Random.Range(0, itemsVisible.Count);
                itemsVisible[rndItem].SetSprite(sprite, itemSizeDto.ScaleForItems, itemSizeDto.ParticlesRadius, true);
                itemsVisible[rndItem].SetAsAnswer();
                itemsVisible[rndItem].InitAsItemWithValue(currentValue, this, shadows.allSprites[currentValue],
                    lightings.allSprites[currentValue]);
                itemsVisible.RemoveAt(rndItem);
                currentValue++;
            }

            foreach (var itemWithoutNum in itemsForHide)
            {
                itemWithoutNum.HideItem();
            }
        }


        public override void Destroy()
        {
            base.Destroy();
            for (int i = 0; i < items.Count; i++)
            {
                Destroy(items[i].gameObject);
            }

            items.Clear();
            Destroy(gameObject);
        }

        public bool UpdateCurrentNumValue(int value)
        {
            var res = false;
            if (value > _currentValue && value - _currentValue == 1)
            {
                res = true;
                _currentValue = value;
            }
            
            if (_currentValue == _winValue)
            {
                foreach (var item in items)
                {
                    item.SetUnselectable();
                }
                TaskCompleted();
            }

            return res;
        }
    }
}