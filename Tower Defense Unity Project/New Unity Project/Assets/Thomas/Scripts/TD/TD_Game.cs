using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Game : MonoBehaviour
{
    public TD_Enemy prefab_enemy_;
    public List<TD_Enemy> list_enemies_;
    [SerializeField] public int player_lives_ = 10;
    const int MAP_RANGE_MIN_ = 2;
    const int MAP_RANGE_MAX_ = 25;
    const int ENEMIES_RANGE_MIN_ = 1;
    const int ENEMIES_RANGE_MAX_ = 25;
    const int TOWER_COST_ = 50;
    const int OIL_COST_ = 25;
    public int money_ = 100;
    private int enemies_spawned_ = 0;
    private List<TD_Tile> list_path_default_;
    private List<TD_Tile> list_path_hovering_;
    private int enemies_to_spawn_ = 0;
    private bool round_has_started_ = false;
    [Range(MAP_RANGE_MIN_, MAP_RANGE_MAX_)] public int map_width_ = MAP_RANGE_MIN_;
    [Range(MAP_RANGE_MIN_, MAP_RANGE_MAX_)] public int map_height_ = MAP_RANGE_MIN_;
    [SerializeField] TD_Map map_ => this.GetComponent<TD_Map>();
    [SerializeField] TD_A_Star pathfinding_ => this.GetComponent<TD_A_Star>();
    [SerializeField] TD_Tile_Factory tile_factory_;
    [SerializeField] TD_Enemy_Factory enemy_factory_;

    [SerializeField] TD_Map_Manager map_manager_ => this.GetComponent<TD_Map_Manager>();
    Ray ray_ => Camera.main.ScreenPointToRay(Input.mousePosition);
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(money_);
        map_.Init(tile_factory_, map_manager_.Open());
        list_enemies_ = new List<TD_Enemy>();
        //implementing new spawnpoints will just be Range of spawn points
        list_path_default_ = pathfinding_.Resolve(map_.spawn_points_[0].transform.position, EnemyType.DEFAULT);
        list_path_hovering_ = pathfinding_.Resolve(map_.spawn_points_[0].transform.position, EnemyType.HOVERING);
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(player_lives_ <= 0)
        {
            Debug.Log("GAME OVER");
            Application.Quit();
        }
        TD_Tile ls_tile = map_.GetTile(ray_);
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (ls_tile != null)
        //    {
        //        map_.SetDestination(ls_tile);
        //    }
        //}
        //else if (Input.GetMouseButtonDown(1))
        //{
        //    if (ls_tile != null)
        //    {
        //        map_.ToggleWall(ls_tile);
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (money_ >= TOWER_COST_)
            {
                if (map_.ToggleTower(ls_tile))
                    money_ -= TOWER_COST_;
            }
        }
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    ls_tile.Content = tile_factory_.Get(TileContentType.SPAWN);
        //}
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if(money_ >= OIL_COST_)
            {
                if (map_.ToggleOil(ls_tile))
                    money_ -= OIL_COST_;
            }
        }
        //else if (Input.GetKeyDown(TD_Map_Manager.SAVE_KEY_))
        //{
        //    map_manager_.Save();
        //}
        //start game
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            list_path_default_ = pathfinding_.Resolve(map_.spawn_points_[0].transform.position, EnemyType.DEFAULT);
            list_path_hovering_ = pathfinding_.Resolve(map_.spawn_points_[0].transform.position, EnemyType.HOVERING);
            //delay can be changed as difficulty increases
            InvokeRepeating("SpawnEnemy", 0.0f, 0.25f);
            round_has_started_ = true;
        }
        if(round_has_started_ && list_enemies_.Count <= 0)
        {
            round_has_started_ = false;
            //this can also incrementally increase for difficulty
            enemies_to_spawn_++;
            Debug.Log(money_);
        }

    }
    public void SpawnEnemy()
    {
        if (enemies_spawned_ >= enemies_to_spawn_)
        {
            CancelInvoke("SpawnEnemy");
            enemies_spawned_ = 0;
            return;
        }

        enemies_spawned_++;
        TD_Enemy temp = Instantiate(prefab_enemy_, new Vector3(map_.spawn_points_[0].transform.position.x, 0.0f, map_.spawn_points_[0].transform.position.z), Quaternion.identity);
        int chosen_type = Random.Range(1, 4);
        temp.Type = enemy_factory_.Get((EnemyType)(chosen_type));
        switch (chosen_type)
        {
            case (int)(EnemyType.DEFAULT):
            {
                temp.list_path_ = list_path_default_;
                break;
            }
            case (int)(EnemyType.FLYING):
            {
                //flying enemies will fly to dest
                temp.list_path_.Add(map_.destination_);
                break;
            }
            case (int)(EnemyType.HOVERING):
            {
                temp.list_path_ = list_path_hovering_;
                break;
            }
        }
        list_enemies_.Add(temp);
    }
    public void KillEnemy(TD_Enemy arg_enemy)
    {
        int currency_gained = (int)arg_enemy.Type.Type;
        money_ += 10 * currency_gained;
        list_enemies_.Remove(arg_enemy);
        list_enemies_.TrimExcess();
        Destroy(arg_enemy.transform.root.gameObject);
    }
    public void RemoveLife()
    {
        if (player_lives_ > 1)
        {
            player_lives_--;
            return;
        }
        //end game stuff

    }
}
