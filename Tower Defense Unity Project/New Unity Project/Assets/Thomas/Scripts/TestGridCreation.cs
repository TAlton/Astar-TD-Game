using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Thomas.Scripts;

public class TestGridCreation : MonoBehaviour
{
    [SerializeField] public int x;
    [SerializeField] public int y;
    [SerializeField] public int z;
    [SerializeField] public GridItem[,,] Tiles;
    [SerializeField] public float GridSpacing = 1f;
    private Vector3 CurrentTilePos = new Vector3(0f, 0f, 0f);

    private void Awake()
    {
        Tiles = new GridItem[x, z, y];

        //width x
        for(int i = 0; i < x; i++)
        {
            Tiles[i, 0, 0] = new GridItem(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            Tiles[i, 0, 0].Tile.transform.position = CurrentTilePos;
            Tiles[i, 0, 0].IndexX = i;
            Tiles[i, 0, 0].IndexZ = 0;
            Tiles[i, 0, 0].IndexY = 0;
            Tiles[i, 0, 0].g = int.MaxValue;
            Tiles[i, 0, 0].Parent = null;

            //depth z
            for (int j = 1; j < z; j++)
            {
                CurrentTilePos.z += GridSpacing;
                Tiles[i, j, 0] = new GridItem(GameObject.CreatePrimitive(PrimitiveType.Sphere));
                Tiles[i, j, 0].Tile.transform.position = CurrentTilePos;
                Tiles[i, j, 0].IndexX = i;
                Tiles[i, j, 0].IndexZ = j;
                Tiles[i, j, 0].IndexY = 0;
                Tiles[i, j, 0].g = int.MaxValue;
                Tiles[i, j, 0].Parent = null;

                //height y
                for (int k = 1; k < y; k++)
                {
                    CurrentTilePos.y += GridSpacing;
                    Tiles[i, j, k] = new GridItem(GameObject.CreatePrimitive(PrimitiveType.Sphere));
                    Tiles[i, j, k].Tile.transform.position = new Vector3(CurrentTilePos.x, CurrentTilePos.y, CurrentTilePos.z - GridSpacing);
                    Tiles[i, j, k].IndexX = i;
                    Tiles[i, j, k].IndexZ = j;
                    Tiles[i, j, k].IndexY = k;
                    Tiles[i, j, k].g = int.MaxValue;
                    Tiles[i, j, k].Parent = null;
                }
                CurrentTilePos.y = 0;
            }
            CurrentTilePos.z = 0;
            CurrentTilePos.x += GridSpacing;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
