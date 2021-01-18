using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassRow : MonoBehaviour
{
    public GameObject[] grassTiles;
    public GameObject[] trees;
    private int grassTileCount;
    private int treeCount;
    private Vector3 nextTilePosition;
    [SerializeField]
    private bool isStartingRow;
    void Start()
    {
        nextTilePosition = transform.position - new Vector3(152, 0, 0);
        grassTileCount = grassTiles.Length;
        treeCount = trees.Length;
        for(int i = 0; i < 39; i++)
        {
            GameObject newTile = Instantiate(grassTiles[Random.Range(0, grassTileCount)], nextTilePosition, transform.rotation);
            newTile.transform.SetParent(transform);
            if(Random.Range(0, 10) == 0)
            {
                if (!isStartingRow || isStartingRow && nextTilePosition.x != 0) //prevent trees from spawning on player
                {
                    //spawn tree
                    Instantiate(trees[Random.Range(0, treeCount)], nextTilePosition + new Vector3(0, 1f, 0), transform.rotation);
                }       
            }
            nextTilePosition.Set(nextTilePosition.x + 8, nextTilePosition.y, nextTilePosition.z);
        }
    }

}
