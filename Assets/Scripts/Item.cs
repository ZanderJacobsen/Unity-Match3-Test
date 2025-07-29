using System;
using UnityEngine;

namespace Match3
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Item : MonoBehaviour
    {
        public ItemType type;
        [SerializeField] GameObject glow;
        SpriteRenderer spriteRenderer;

        int defaultSortingOrder;

        public void SetType(ItemType type)
        {
            this.type = type;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = type.sprite;
            defaultSortingOrder = spriteRenderer.sortingOrder;
        }

        public ItemType GetType() => type;

        public void SetSelected(bool selected)
        {
            spriteRenderer.sortingOrder = selected ? 100 : defaultSortingOrder;
            glow.SetActive(selected);
        }

        internal void DestroyItem() => Destroy(gameObject);
    }
}
