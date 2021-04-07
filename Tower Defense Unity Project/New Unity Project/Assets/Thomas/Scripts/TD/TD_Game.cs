using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Game : MonoBehaviour
{
    const int RANGE_MIN_ = 2;
    const int RANGE_MAX_ = 25;
    [Range(RANGE_MIN_, RANGE_MAX_)] public int map_width_ = RANGE_MIN_;
    [Range(RANGE_MIN_, RANGE_MAX_)] public int map_height_ = RANGE_MIN_;
    [SerializeField] TD_Map map_;
    [SerializeField] TD_Tile_Factory tile_factory_;
    [SerializeField] TD_Map_Manager map_manager_;
    Ray ray_ => Camera.main.ScreenPointToRay(Input.mousePosition);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        map_ = this.GetComponent<TD_Map>();
        map_manager_ = this.GetComponent<TD_Map_Manager>();
        map_.Init(map_width_, map_height_, tile_factory_);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TD_Tile ls_tile = map_.GetTile(ray_);
            if(ls_tile != null)
            {
                ls_tile.Content = tile_factory_.Get(TileContentType.DESTINATION);
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            TD_Tile  ls_tile = map_.GetTile(ray_);
            if(ls_tile != null)
            {
                map_.ToggleWall(ls_tile);
            }
        }
        if (Input.GetKeyDown(TD_Map_Manager.SAVE_KEY_))
            map_manager_.Open();

    }
}
