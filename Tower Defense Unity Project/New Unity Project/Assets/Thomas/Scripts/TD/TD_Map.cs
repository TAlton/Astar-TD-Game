using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Map : MonoBehaviour
{
    public Vector2Int map_size_;
    public TD_Tile prefab_;
    public TD_Tile destination_;
    public TD_Tile[,] map_tiles_;
    TD_Tile_Factory tile_factory_;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(Vector2Int arg_map_dim)
    {
        this.map_size_ = arg_map_dim;
    }
    public void Init(int arg_x, int arg_y, TD_Tile_Factory arg_factory)
    {
        this.map_size_ = new Vector2Int(arg_x, arg_y);
        this.tile_factory_ = arg_factory;
        map_tiles_ = new TD_Tile[arg_y, arg_x];

        //offset caries on tile scale; this could be 0.5 for performance sake
        Vector2 ls_offset = new Vector2((map_size_.x  - 1) * (prefab_.transform.localScale.x / 2), (map_size_.y - 1) * (prefab_.transform.localScale.y / 2));

        for(int i_y = 0; i_y < map_size_.y; i_y++)
        {
            for(int i_x = 0; i_x < map_size_.x; i_x++)
            {
                TD_Tile ls_tile = map_tiles_[i_y, i_x] = Instantiate(prefab_);
                ls_tile.transform.position = new Vector3(i_x - ls_offset.x, 0, i_y - ls_offset.y);
                ls_tile.Content = tile_factory_.Get(TileContentType.EMPTY);
                ls_tile.Parent = null;
                ls_tile.g = int.MaxValue;
                ls_tile.IndexX = i_x;
                ls_tile.IndexY = i_y;
            }
        }
    }
    public void Init(TD_Tile_Factory arg_factory, List<Vector3Int> arg_loaded_tiles)
    {
        this.map_size_ = new Vector2Int(arg_loaded_tiles[0].x, arg_loaded_tiles[0].y);
        this.tile_factory_ = arg_factory;
        map_tiles_ = new TD_Tile[map_size_.y, map_size_.x];

        //offset caries on tile scale; this could be 0.5 for performance sake
        Vector2 ls_offset = new Vector2((map_size_.x - 1) * (prefab_.transform.localScale.x / 2), (map_size_.y - 1) * (prefab_.transform.localScale.y / 2));

        int ls_loaded_tiles_index = 1;

        for (int i_y = 0; i_y < map_size_.y; i_y++)
        {
            for (int i_x = 0; i_x < map_size_.x; i_x++)
            {
                TD_Tile ls_tile; 

                if(ls_loaded_tiles_index < arg_loaded_tiles.Count)
                {
                    if (arg_loaded_tiles[ls_loaded_tiles_index].x == i_x &&
                    arg_loaded_tiles[ls_loaded_tiles_index].y == i_y)
                    {
                        ls_tile = map_tiles_[i_y, i_x] = Instantiate(prefab_);
                        ls_tile.transform.position = new Vector3(i_x - ls_offset.x, 0, i_y - ls_offset.y);
                        ls_tile.Content = tile_factory_.Get((TileContentType)(arg_loaded_tiles[ls_loaded_tiles_index].z));

                        if (TileContentType.DESTINATION == ls_tile.Content.Type)
                        {
                            destination_ = ls_tile;
                            ls_tile.is_blocking_ = false;
                            ls_tile.Parent = null;
                            ls_tile.g = int.MaxValue;
                            ls_tile.IndexX = i_x;
                            ls_tile.IndexY = i_y;
                            ls_loaded_tiles_index++;
                            continue;
                        }
                            destination_ = ls_tile;

                        ls_tile.is_blocking_ = true;
                        ls_tile.Parent = null;
                        ls_tile.g = int.MaxValue;
                        ls_tile.IndexX = i_x;
                        ls_tile.IndexY = i_y;
                        ls_loaded_tiles_index++;
                        continue;
                    }
                }
                
                ls_tile = map_tiles_[i_y, i_x] = Instantiate(prefab_);
                ls_tile.transform.position = new Vector3(i_x - ls_offset.x, 0, i_y - ls_offset.y);
                ls_tile.Content = tile_factory_.Get(TileContentType.EMPTY);
                ls_tile.Parent = null;
                ls_tile.g = int.MaxValue;
                ls_tile.IndexX = i_x;
                ls_tile.IndexY = i_y;
            }
        }
    }
    public TD_Tile GetTile(Ray arg_ray)
    {
        if (Physics.Raycast(arg_ray, out RaycastHit hit))
        {
            int ls_x = (int)(hit.point.x + map_size_.x * (prefab_.transform.localScale.x / 2));
            int ls_y = (int)(hit.point.z + map_size_.y * (prefab_.transform.localScale.y / 2));
            if(ls_x >= 0 && ls_x < map_tiles_.GetLength(1) &&
                ls_y >= 0 && ls_y < map_tiles_.GetLength(0))
            {
                return map_tiles_[ls_y, ls_x];
            }
        }
        return null;
    }
    public void ToggleWall(TD_Tile arg_tile)
    {
        if(arg_tile.Content.Type == TileContentType.WALL)
        {
            //set blocking of tile too
            arg_tile.Content = tile_factory_.Get(TileContentType.EMPTY);
            arg_tile.is_blocking_ = false;
            return;
        }
        //ditto
        arg_tile.Content = tile_factory_.Get(TileContentType.WALL);
        arg_tile.is_blocking_ = true;
    }
    public void SetDestination(TD_Tile arg_tile)
    {
        if(null != destination_)
            destination_.Content = tile_factory_.Get(TileContentType.EMPTY);

        destination_ = arg_tile;
        arg_tile.Content = tile_factory_.Get(TileContentType.DESTINATION);
    }

}
