using System;
using UnityEngine;

namespace Match3
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Item : MonoBehaviour
    {
        public ItemType type;
        [SerializeField] GameObject glow;


        public void SetType(ItemType type)
        {
            this.type = type;
            GetComponent<SpriteRenderer>().sprite = type.sprite;
        }

        public ItemType GetType() => type;

        public void SetSelected(bool selected)
        {
            glow.SetActive(selected);
        }

        internal void DestroyItem() => Destroy(gameObject);
    }
}
