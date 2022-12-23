using System.Collections.Generic;
using Games.InCircles.Scripts.FieldHolders.Fields;
using UnityEngine;

namespace Games.InCircles.Scripts.Exercises.Games
{
    public class FindCouple : Exercise
    {
        [SerializeField] private ContentItem proto;
        [SerializeField] private SpriteHolder[] categories;
        
        private readonly List<ContentItem> _items = new List<ContentItem>();
        private int _countTrueSprites;

        public override void Generate(ContentDto contentDto, ItemSizeDto itemSizeDto, int itemsOnGrid,
            Vector3 direction)
        {
            for (var i = 0; i < 3; i++)
            {
                _items.Add(Instantiate(proto, Vector3.zero, Quaternion.Euler(direction), contentDto.CoupleContent));
            }
            FillItems(itemSizeDto);
        }

        private void FillItems(ItemSizeDto itemSizeDto)
        {
            var itemsWithoutSprite = new List<ContentItem>(_items);
            var rnd = Random.Range(0, categories.Length);

            AddFalseSprites(itemSizeDto.ScaleForItems, itemSizeDto.ParticlesRadius, rnd, ref itemsWithoutSprite);

            int trueRnd;
            do
            {
                trueRnd = Random.Range(0, categories.Length);
            } while (trueRnd == rnd);
            
            AddTrueSprites(itemSizeDto.ScaleForItems, itemSizeDto.ParticlesRadius, trueRnd, ref itemsWithoutSprite);
        }

        private void AddFalseSprites(Vector2 itemSize, float particlesRadius, int rnd, ref List<ContentItem> itemsWithoutSprite)
        {
            var sprites = categories[rnd].GetRandomSprites(1);
            foreach (var sprite in sprites)
            {
                var rndItem = Random.Range(0, itemsWithoutSprite.Count);
                itemsWithoutSprite[rndItem].SetSprite(sprite, itemSize, particlesRadius, true);
                itemsWithoutSprite.RemoveAt(rndItem);
            }
        }

        private void AddTrueSprites(Vector2 itemSize, float particlesRadius, int rnd, ref List<ContentItem> itemsWithoutSprite)
        {
            var sprites = categories[rnd].GetRandomSprites(2);
            foreach (var sprite in sprites)
            {
                var rndItem = Random.Range(0, itemsWithoutSprite.Count);
                itemsWithoutSprite[rndItem].SetSprite(sprite, itemSize, particlesRadius, true, true);
                itemsWithoutSprite[rndItem].SetAsAnswer();
                itemsWithoutSprite[rndItem].SelectedTrue += AddTrueSelectedAnswer;
                itemsWithoutSprite.RemoveAt(rndItem);
            }
        }

        private void AddTrueSelectedAnswer(ContentItem itemsWithoutSprite)
        {
            _countTrueSprites++;
            itemsWithoutSprite.SelectedTrue -= AddTrueSelectedAnswer;
            if (_countTrueSprites == 2)
            {
                Complete();
            }
        }

        private void Complete()
        {
            foreach (var item in _items)
            {
                item.SetUnselectable();
            }
            TaskCompleted();
        }

        public override void Destroy()
        {
            base.Destroy();
            for (int i = 0; i < _items.Count; i++)
            {
                Destroy(_items[i].gameObject);
                //items[i].StartDestroyItem();
            }
            _items.Clear();
            Destroy(gameObject);
        }
    }
}