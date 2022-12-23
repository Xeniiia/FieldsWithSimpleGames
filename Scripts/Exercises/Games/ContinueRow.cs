using System.Collections.Generic;
using System.Linq;
using Games.InCircles.Scripts.FieldHolders.Fields;
using UnityEngine;

namespace Games.InCircles.Scripts.Exercises.Games
{
    public class ContinueRow : Exercise
    {
        [SerializeField] private ContentItem proto;
        [SerializeField] private SpriteHolder itemSprites;
        private readonly List<ContentItem> _items = new List<ContentItem>();

        public override void Generate(ContentDto contentDto, ItemSizeDto itemSizeDto, int itemsOnGrid,
            Vector3 direction)
        {
            int rowItemsCount;
            if (direction != Vector3.zero)
            {
                rowItemsCount = 4;
            }
            else
            {
                rowItemsCount = Random.Range(0, 100) > 75 ? 4 : 6;
            }

            var sprites = itemSprites.GetRandomSprites(rowItemsCount / 2).ToList();
            var exampleSpriteSequence = CreateItemsSequence(sprites, out var correctSprite, out var otherSprites);
            FillExampleItems(contentDto, itemSizeDto, exampleSpriteSequence);
            FillAnswerItems(contentDto, itemSizeDto, correctSprite, otherSprites, rowItemsCount);
        }

        private List<Sprite> CreateItemsSequence(List<Sprite> sprites, out Sprite correctSprite, out List<Sprite> otherSprites)
        {
            sprites = sprites.OrderBy(x => Random.value).ToList();
            List<Sprite> exampleSpriteSequence = new List<Sprite>();
            exampleSpriteSequence.AddRange(sprites);
            if (Random.Range(0, 100) > 50 && exampleSpriteSequence.Count > 2)
            {
                var sprCount = exampleSpriteSequence.Count - 1;
                exampleSpriteSequence[sprCount] = exampleSpriteSequence[Random.Range(0, sprCount)];
            }

            otherSprites = new List<Sprite>(exampleSpriteSequence);
            correctSprite = exampleSpriteSequence[0];
            //var delCount = exampleSpriteSequence.Count > 2 ? Random.Range(0, exampleSpriteSequence.Count - 1) : 0;
            //for (int i = 0; i < delCount; i++)
            //{
            //    correctSprite = otherSprites.Last();
            //    otherSprites.RemoveAt(otherSprites.Count - 1);
            //}
            
            exampleSpriteSequence.AddRange(otherSprites);
            var s = correctSprite;
            if (otherSprites.Contains(correctSprite)) otherSprites.RemoveAll(x => x == s);
            return exampleSpriteSequence;
        }

        private void FillExampleItems(ContentDto contentDto, ItemSizeDto itemSizeDto, IEnumerable<Sprite> sprites)
        {
            foreach (var exampleSprite in sprites)
            {
                var newExampleItem = Instantiate(proto, contentDto.ExampleContent, false);
                newExampleItem.SetSprite(exampleSprite, itemSizeDto.ScaleForExampleItems, itemSizeDto.ParticlesRadius, false);
                _items.Add(newExampleItem);
            }
        }

        private void FillAnswerItems(ContentDto contentDto, ItemSizeDto itemSizeDto, Sprite correctSprite, List<Sprite> otherSprites, int rowItemsCount)
        {
            var answerItems = new List<ContentItem>(otherSprites.Count + 1);
            for (var i = 0; i <= otherSprites.Count; i++)
            {
                var answerItem = Instantiate(proto, contentDto.AnswerContent, false);
                answerItems.Add(answerItem);
            }

            AddSpritesInAnswerItems(itemSizeDto, answerItems, correctSprite, otherSprites);
            _items.AddRange(answerItems);
        }

        private void AddSpritesInAnswerItems(ItemSizeDto itemSizeDto, List<ContentItem> answerItemsWithoutSprites, Sprite correctSprite, List<Sprite> otherSprites)
        {
            var answerItems = new List<ContentItem>();
            answerItems.AddRange(answerItemsWithoutSprites);
            FillTrueItem(itemSizeDto, correctSprite, answerItems);
            FillFalseItems(itemSizeDto, otherSprites, answerItems);
        }

        private void FillTrueItem(ItemSizeDto itemSizeDto, Sprite correctSprite, List<ContentItem> answerItems)
        {
            var rnd = Random.Range(0, answerItems.Count);
            answerItems[rnd].SetSprite(correctSprite, itemSizeDto.ScaleForAnswerItems, itemSizeDto.ParticlesRadius, true, true);
            answerItems[rnd].SetAsAnswer();
            answerItems[rnd].SelectedTrue += Complete;
            answerItems.RemoveAt(rnd);
        }

        private void Complete(ContentItem itemsWithoutSprite)
        {
            itemsWithoutSprite.SelectedTrue -= Complete;
            foreach (var item in _items)
            {
                item.SetUnselectable();
            }

            TaskCompleted();
        }

        private static void FillFalseItems(ItemSizeDto itemSizeDto, List<Sprite> otherSprites, List<ContentItem> answerItems)
        {
            var remainingSprites = otherSprites.ToList();
            foreach (var falseItem in answerItems)
            {
                var rnd = Random.Range(0, remainingSprites.Count);
                falseItem.SetSprite(remainingSprites[rnd], itemSizeDto.ScaleForAnswerItems, itemSizeDto.ParticlesRadius, true);
                remainingSprites.RemoveAt(rnd);
            }
        }


        public override void Destroy()
        {
            base.Destroy();
            foreach (var t in _items)
            {
                Destroy(t.gameObject);
            }

            _items.Clear();
            Destroy(gameObject);
        }
    }
}