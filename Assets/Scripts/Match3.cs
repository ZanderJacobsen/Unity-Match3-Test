using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;

namespace Match3
{
    public class Match3 : MonoBehaviour
    {
        [SerializeField] int width = 8;
        [SerializeField] int height = 8;
        [SerializeField] float cellSize = 1f;
        [SerializeField] Vector3 originPosition = Vector3.zero;
        [SerializeField] bool debug = true;

        [SerializeField] Item itemPrefab;
        [SerializeField] ItemType[] itemTypes;
        [SerializeField] Ease ease = Ease.InQuad;

        Grid<GridObject<Item>> grid;

        InputReader inputReader;
        Vector2Int selectedItem = Vector2Int.one * -1;

        void Awake()
        {
            inputReader = GetComponent<InputReader>();
        }

        void Start()
        {
            InitializeGrid();
            inputReader.Fire += OnSelectItem;
        }

        void OnDestroy()
        {
            inputReader.Fire -= OnSelectItem;
        }

        private void OnSelectItem()
        {
            var gridPos = grid.GetXY(Camera.main.ScreenToWorldPoint(inputReader.Selected));

            // validate grid position

            if (selectedItem == gridPos)
            {
                DeselectItem();
            }
            else if (selectedItem == Vector2Int.one * -1)
            {
                SelectItem(gridPos);
            }
            else
            {
                StartCoroutine(RunGameLoop(selectedItem, gridPos));
            }
        }

        private void DeselectItem() => selectedItem = new Vector2Int(-1, -1);
        private void SelectItem(Vector2Int gridPos) => selectedItem = gridPos;

        private IEnumerator RunGameLoop(Vector2Int gridPosA, Vector2Int gridPosB)
        {
            yield return StartCoroutine(SwapGems(gridPosA, gridPosB));

            yield return null;
        }

        private IEnumerator SwapGems(Vector2Int gridPosA, Vector2Int gridPosB)
        {
            var gridObjectA = grid.GetValue(gridPosA.x, gridPosA.y);
            var gridObjectB = grid.GetValue(gridPosB.x, gridPosB.y);

            gridObjectA.GetValue().transform
                .DOLocalMove(grid.GetWorldPositionCenter(gridPosB.x, gridPosB.y), 0.5f)
                .SetEase(ease);
            gridObjectB.GetValue().transform
                .DOLocalMove(grid.GetWorldPositionCenter(gridPosA.x, gridPosA.y), 0.5f)
                .SetEase(ease);

            grid.SetValue(gridPosB.x, gridPosB.y, gridObjectA);
            grid.SetValue(gridPosA.x, gridPosA.y, gridObjectB);

            yield return new WaitForSeconds(0.5f);
        }

        // Init Grid
        void InitializeGrid()
        {
            grid = Grid<GridObject<Item>>.VerticalGrid(width, height, cellSize, originPosition, debug);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CreateItem(x, y);
                }
            }
        }

        void CreateItem(int x, int y)
        {
            Item item = Instantiate(itemPrefab, grid.GetWorldPositionCenter(x, y), Quaternion.identity, transform);
            item.SetType(itemTypes[Random.Range(0, itemTypes.Length)]);
            grid.SetValue(x, y, new GridObject<Item>(grid, x, y));
            var gridObject = new GridObject<Item>(grid, x, y);
            gridObject.SetValue(item);
            grid.SetValue(x, y, gridObject);
        }

        // Read player input and swap items

        // Start coroutine:
        // Swap animation
        // Matches?
        // Make Items Explode
        // Replace empty spot
        // Is Game Over?
    }
}
