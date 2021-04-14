using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu]
public class TD_Enemy_Factory : ScriptableObject
{
    [SerializeField] TD_Enemy_Type prefab_default_enemy_;
    [SerializeField] TD_Enemy_Type prefab_flying_enemy_;
    [SerializeField] TD_Enemy_Type prefab_hovering_enemy_;
    Scene content_scene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Reclaim(TD_Enemy_Type arg_content)
    {
        Destroy(arg_content.gameObject);
    }
    private TD_Enemy_Type Get(TD_Enemy_Type arg_content)
    {
        TD_Enemy_Type ls_instance = Instantiate(arg_content);
        ls_instance.OriginFactory = this;
        MoveToFactoryScene(ls_instance.gameObject);
        return ls_instance;
    }
    public TD_Enemy_Type Get(EnemyType arg_type)
    {
        switch (arg_type)
        {
            case EnemyType.DEFAULT: return Get(prefab_default_enemy_);
            case EnemyType.HOVERING: return Get(prefab_hovering_enemy_);
            case EnemyType.FLYING: return Get(prefab_flying_enemy_);
        }
        return null;
    }
    void MoveToFactoryScene(GameObject arg_obj)
    {
        if (!content_scene.isLoaded) 
        {
            if (Application.isEditor)
            {
                content_scene = SceneManager.GetSceneByName(name);
                if (!content_scene.isLoaded)
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
