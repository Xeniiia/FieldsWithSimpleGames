using System.Collections.Generic;
using Games.InCircles.Scripts.FieldHolders.Fields;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Games.InCircles.Scripts.Exercises.Games
{
    public class WhatIsSuperfluous : Exercise
    {
        [SerializeField] private ContentItem proto;
        [SerializeField] private SpriteHolder[] categories;
        private List<ContentItem> items = new List<ContentItem>();

        public override void Generate(ContentDto contentDto, ItemSizeDto itemSizeDto, int itemsOnGrid,
            Vector3 direction)
        {
            for (int i = 0; i < 4; i++)
            {
                items.Add(Instantiate(proto, contentDto.GridContent, false));
            } 
            // yield return new WaitForEndOfFrame();
            FillItems(itemSizeDto);

        }

        private void FillItems(ItemSizeDto itemSizeDto)
        {
            var itemsWithoutSprite = new List<ContentItem>(items);
            
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
            var sprites = categories[rnd].GetRandomSprites(3);
            foreach (var sprite in sprites)
            {
                var rndItem = Random.Range(0, itemsWithoutSprite.Count);
                itemsWithoutSprite[rndItem].SetSprite(sprite, itemSize, particlesRadius, true);
                itemsWithoutSprite.RemoveAt(rndItem);
            }
        }

        private void AddTrueSprites(Vector2 itemSize, float particlesRadius, int rnd, ref List<ContentItem> itemsWithoutSprite)
        {
            var sprites = categories[rnd].GetRandomSprites(1);
            foreach (var sprite in sprites)
            {
                var rndItem = Random.Range(0, itemsWithoutSprite.Count);
                itemsWithoutSprite[rndItem].SetSprite(sprite, itemSize, particlesRadius, true, true);
                itemsWithoutSprite[rndItem].SetAsAnswer();
                itemsWithoutSprite[rndItem].SelectedTrue += Complete;
                itemsWithoutSprite.RemoveAt(rndItem);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            for (int i = 0; i < items.Count; i++)
            {
                Destroy(items[i].gameObject);
                //items[i].StartDestroyItem();
            }
            items.Clear();
            Destroy(gameObject);
        }

        private void Complete(ContentItem itemsWithoutSprite)
        {
            itemsWithoutSprite.SelectedTrue -= Complete;
            foreach (var item in items)
            {
                item.SetUnselectable();
            }
            TaskCompleted();
        }
    }
}