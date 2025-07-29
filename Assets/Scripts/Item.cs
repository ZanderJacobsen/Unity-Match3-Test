using System;
using UnityEngine;

namespace Match3
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Item : MonoBehaviour
    {
        public ItemType type;
        [SerializeField] GameObject outline;

        public void SetType(ItemType type)
        {
            this.type = type;
            GetComponent<SpriteRenderer>().sprite = type.sprite;
        }

        public ItemType GetType() => type;

        public void SetSelected(bool selected)
        {
            outline.SetActive(selected);
        }

        internal void DestroyItem() => Destroy(gameObject);
    }
}
