using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int width, height;
    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        SetUp();
    }

    private void SetUp() 
    {
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                Vector2 tempPosition = new Vector2 (i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "("+i+", "+j+")";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
