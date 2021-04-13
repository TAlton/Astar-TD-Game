using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu]
public class TD_Tile_Factory : ScriptableObject
{
    [SerializeField] TD_Tile_Content prefab_dest_;
    [SerializeField] TD_Tile_Content prefab_empty_;
    [SerializeField] TD_Tile_Content prefab_wall_;
    [SerializeField] TD_Tile_Content prefab_tower_;
    [SerializeField] TD_Tile_Content prefab_spawn_point_;
    Scene content_scene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Reclaim(TD_Tile_Content arg_content)
    {
        Destroy(arg_content.gameObject);
    }
    private TD_Tile_Content Get (TD_Tile_Content arg_content)
    {
        TD_Tile_Content ls_instance = Instantiate(arg_content);
        ls_instance.OriginFactory = this;
        MoveToFactoryScene(ls_instance.gameObject);
        return ls_instance;
    }
    public TD_Tile_Content Get(TileContentType arg_type)
    {
        switch(arg_type) {
            case TileContentType.DESTINATION:   return Get(prefab_dest_);
            case TileContentType.EMPTY:         return Get(prefab_empty_);
            case TileContentType.WALL:          return Get(prefab_wall_);
            case TileContentType.TOWER:         return Get(prefab_tower_);
            case TileContentType.SPAWN:         return Get(prefab_spawn_point_);
        }
        return null;
    }
    void MoveToFactoryScene(GameObject arg_obj)
    {
        if(!content_scene.isLoaded)
        {
            if(Application.isEditor)
            {
                content_scene = SceneManager.GetSceneByName(name);
                if(!content_scene.isLoaded)
                {
                    content_scene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                content_scene = SceneManager.CreateScene(name);
            }
        }
        SceneManager.MoveGameObjectToScene(arg_obj, content_scene);
    }

}
