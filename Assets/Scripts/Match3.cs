using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
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
        [SerializeField] GameObject explosion;

        AudioManager audioManager;

        Grid<GridObject<Item>> grid;

        InputReader inputReader;
        bool disableInput = false;

        Vector2Int selectedItem = Vector2Int.one * -1;
        Vector2Int neighborItem = Vector2Int.one * -1;

        void Awake()
        {
            inputReader = GetComponent<InputReader>();
            audioManager = GetComponent<AudioManager>();
        }

        void Start()
        {
            InitializeGrid();
            inputReader.onDown += OnSelectItem;
            inputReader.onUp += OnReleaseItem;
        }

        void OnDestroy()
        {
            inputReader.onDown -= OnSelectItem;
            inputReader.onUp -= OnReleaseItem;
        }

        private void OnSelectItem()
        {
            if (disableInput)
                return;

            var gridPos = grid.GetXY(Camera.main.ScreenToWorldPoint(inputReader.Selected));
            var item = grid.GetValue(gridPos.x, gridPos.y)?.GetValue();

            // validate grid position
            if (!IsValidPosition(gridPos) || isEmptyPosition(gridPos))
            {
                return;
            }

            if (selectedItem == Vector2Int.one * -1)
            {
                item.SetSelected(true);
                SelectItem(gridPos);
            }
        }

        private void OnReleaseItem()
        {
            if (disableInput)
                return;

            neighborItem = grid.GetXY(Camera.main.ScreenToWorldPoint(inputReader.Selected));
            var item = grid.GetValue(selectedItem.x, selectedItem.y)?.GetValue();


            // validate grid position
            if (IsValidPosition(selectedItem))
            {
                item.SetSelected(false);
            }
            else if (!IsValidPosition(neighborItem) || isEmptyPosition(neighborItem))
            {
                return;
            }

            if (selectedItem == neighborItem)
            {

            }
            else if (!AreNeighbors(selectedItem, neighborItem))
            {
                audioManager.PlayDud(); // bad move sound
            }
            else
                StartCoroutine(RunGameLoop(selectedItem, neighborItem));

            DeselectItem(); // Reset selection
        }

        private bool AreNeighbors(Vector2Int a, Vector2Int b)
        {
            int dx = Mathf.Abs(a.x - b.x);
            int dy = Mathf.Abs(a.y - b.y);
            return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
        }


        private bool IsValidPosition(Vector2Int gridPos) => gridPos is { x: >= 0, y: >= 0 } && gridPos.x < width && gridPos.y < height;
        private bool isEmptyPosition(Vector2Int gridPos) => grid.GetValue(gridPos.x, gridPos.y) == null;

        private void DeselectItem()
        {
            audioManager.PlayDeselect();
            selectedItem = new Vector2Int(-1, -1);
        }

        private void SelectItem(Vector2Int gridPos)
        {
            audioManager.PlayClick();
            selectedItem = gridPos;
        }

        private IEnumerator RunGameLoop(Vector2Int gridPosA, Vector2Int gridPosB)
        {
            disableInput = true;
            yield return StartCoroutine(SwapGems(gridPosA, gridPosB));

            // TODO: Calc Gamescore?

            List<Vector2Int> matches = FindMatches();

            if (matches.Count == 0)
            {
                // Swap back if no matches
                yield return StartCoroutine(SwapGems(gridPosB, gridPosA));
                audioManager.PlayDud();
                disableInput = false;
                yield break;
            }

            // Remove matches
            yield return StartCoroutine(ClearItems(matches));
            // Items fall
            yield return StartCoroutine(ItemsFall());
            // Refill board
            yield return StartCoroutine(FillBoard());

            // TODO: Check Gameover

            yield return StartCoroutine(CascadeMatches());

            disableInput = false;
            yield return null;
        }

        private IEnumerator CascadeMatches()
        {
            List<Vector2Int> matches = FindMatches();
            if (matches.Count > 0)
            {
                yield return StartCoroutine(ClearItems(matches));
                // Items fall
                yield return StartCoroutine(ItemsFall());
                // Refill board
                yield return StartCoroutine(FillBoard());
                yield return StartCoroutine(CascadeMatches());
            }
        }

        private IEnumerator ItemsFall()
        {
            // TODO: Simplify
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (grid.GetValue(x, y) == null)
                    {
                        for (var i = y + 1; i < height; i++)
                        {
                            if (grid.GetValue(x, i) != null)
                            {
                                var item = grid.GetValue(x, i).GetValue();
                                grid.SetValue(x, y, grid.GetValue(x, i));
                                grid.SetValue(x, i, null);
                                item.transform
                                    .DOLocalMove(grid.GetWorldPositionCenter(x, y), 0.1f)
                                    .SetEase(ease);
                                // SFX play
                                audioManager.PlayWoosh();
                                yield return new WaitForSeconds(0.01f);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private IEnumerator FillBoard()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (grid.GetValue(x, y) == null)
                    {
                        //SFX play
                        audioManager.PlayPop();
                        CreateItem(x, y);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
        }

        private IEnumerator ClearItems(List<Vector2Int> matches)
        {

            foreach (var match in matches)
            {
                var item = grid.GetValue(match.x, match.y).GetValue();
                grid.SetValue(match.x, match.y, null);

                removeVFX(match);
                // SFX play
                audioManager.PlayMatch();

                item.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f, 1, 0.5f);

                yield return new WaitForSeconds(0.1f);

                item.DestroyItem();
            }
        }

        private void removeVFX(Vector2Int match)
        {
            // TODO: Pool
            var fx = Instantiate(explosion, transform);
            fx.transform.position = grid.GetWorldPositionCenter(match.x, match.y);
            Destroy(fx, 5f);
        }

        private List<Vector2Int> FindMatches()
        {
            HashSet<Vector2Int> matches = new();

            // Horizontal matches
            for (int y = 0; y < height; y++)
            {
                int matchLength = 1;
                for (int x = 1; x < width; x++)
                {
                    var previous = grid.GetValue(x - 1, y)?.GetValue();
                    var current = grid.GetValue(x, y)?.GetValue();
                    // var next = grid.GetValue(x + 1, y)?.GetValue();

                    if (previous == null || current == null)
                        continue;

                    if (current.GetType() == previous.GetType())
                    {
                        matchLength++;
                    }
                    else
                    {
                        if (matchLength >= 3)
                        {
                            for (int k = 0; k < matchLength; k++)
                            {
                                matches.Add(new Vector2Int(x - 1 - k, y));
                            }
                        }
                        matchLength = 1;
                    }
                }
                // Check for match at end of row
                if (matchLength >= 3)
                {
                    for (int k = 0; k < matchLength; k++)
                    {
                        matches.Add(new Vector2Int(width - 1 - k, y));
                    }
                }
            }

            //Vertical matches
            for (int x = 0; x < width; x++)
            {
                int matchLength = 1;
                for (int y = 1; y < height; y++)
                {
                    var previous = grid.GetValue(x, y - 1)?.GetValue();
                    var current = grid.GetValue(x, y)?.GetValue();
                    // var next = grid.GetValue(x + 1, y)?.GetValue();

                    if (previous == null || current == null)
                        continue;

                    if (current.GetType() == previous.GetType())
                    {
                        matchLength++;
                    }
                    else
                    {
                        if (matchLength >= 3)
                        {
                            for (int k = 0; k < matchLength; k++)
                            {
                                matches.Add(new Vector2Int(x, y - 1 - k));
                            }
                        }
                        matchLength = 1;
                    }
                }
                // Check for match at end of row
                if (matchLength >= 3)
                {
                    for (int k = 0; k < matchLength; k++)
                    {
                        matches.Add(new Vector2Int(x, height - 1 - k));
                    }
                }
            }

            return new List<Vector2Int>(matches);
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
            var item = Instantiate(itemPrefab, grid.GetWorldPositionCenter(x, y), Quaternion.identity, transform);
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
