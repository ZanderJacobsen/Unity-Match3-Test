using System;
using UnityEngine;

namespace Match3
{
    public class GridObject<T>
    {
        Grid<GridObject<T>> grid;
        int x, y;
        T item;

        public GridObject(Grid<GridObject<T>> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        internal void SetValue(T item)
        {
            this.item = item;
        }

        public T GetValue() => item;
    }
}