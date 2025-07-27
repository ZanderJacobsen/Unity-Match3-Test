using System;
using UnityEngine;

namespace Match3
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Item : MonoBehaviour
    {
        public ItemType type;

        public void SetType(ItemType type)
        {
            this.type = type;
            GetComponent<SpriteRenderer>().sprite = type.sprite;
        }

        public ItemType GetType() => type;

        internal void DestroyItem() => Destroy(gameObject);
    }
}
