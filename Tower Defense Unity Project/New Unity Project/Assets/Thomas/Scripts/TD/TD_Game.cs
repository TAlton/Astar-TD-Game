using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Game : MonoBehaviour
{
    const int MAP_RANGE_MIN_ = 2;
    const int MAP_RANGE_MAX_ = 25;
    const int ENEMIES_RANGE_MIN_ = 1;
    const int ENEMIES_RANGE_MAX_ = 25;
    [Range(MAP_RANGE_MIN_, MAP_RANGE_MAX_)] public int map_width_ = MAP_RANGE_MIN_;
    [Range(MAP_RANGE_MIN_, MAP_RANGE_MAX_)] public int map_height_ = MAP_RANGE_MIN_;
    [Range(ENEMIES_RANGE_MIN_, ENEMIES_RANGE_MAX_)] public int enemies_to_spawn_ = ENEMIES_RANGE_MIN_;
    [SerializeField] TD_Map map_ => this.GetComponent<TD_Map>();
    [SerializeField] TD_A_Star pathfinding_ => this.GetComponent<TD_A_Star>();
    [SerializeField] TD_Tile_Factory tile_factory_;
    [SerializeField] TD_Map_Manager map_manager_ => this.GetComponent<TD_Map_Manager>();
    Ray ray_ => Camera.main.ScreenPointToRay(Input.mousePosition);
    List<TD_Enemy_Type> list_enemies_;
    // Start is called before the first frame update
    void Start()
    {
        //map_ = this.GetComponent<TD_Map>();
        //map_manager_ = this.GetComponent<TD_Map_Manager>();
        //pathfinding_ = this.GetComponent<TD_A_Star>();
        map_.Init(tile_factory_, map_manager_.Open());
        list_enemies_ = new List<TD_Enemy_Type>();
        //map_.Init(map_width_, map_height_, tile_factory_);
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TD_Tile ls_tile = map_.GetTile(ray_);
        if (Input.GetMouseButtonDown(0))
        {
            if(ls_tile != null)
            {
                map_.SetDestination(ls_tile);
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            if(ls_tile != null)
            {
                map_.ToggleWall(ls_tile);
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (ls_tile.Content.Type == TileContentType.WALL)
                {

                }
                ls_tile.Content = tile_factory_.Get(TileContentType.TOWER);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                ls_tile.Content = tile_factory_.Get(TileContentType.SPAWN);
            }
        }
        if (Input.GetKeyDown(TD_Map_Manager.SAVE_KEY_))
            map_manager_.Save();
        if (Input.GetKeyDown(KeyCode.Q))
            pathfinding_.Resolve(map_.spawn_points_[0].transform.position);

    }
}
