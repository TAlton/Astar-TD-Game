using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Enemy : MonoBehaviour
{
    [SerializeField] TD_Enemy_Type enemy_type_;
    [SerializeField] TD_Game game_;
    public List<TD_Tile> list_path_;
    private int current_path_index = 0;
    private const float MOVE_SPEED_ = 5.0f;
    private float current_move_speed_ = MOVE_SPEED_;
    private float health_ = 100.0f;
    private bool is_dead = false;
    public TD_Enemy_Type Type
    {
        get => enemy_type_;
        set
        {
            if (enemy_type_ != null)
            {
                enemy_type_.Recycle();
            }
            enemy_type_ = value;
            enemy_type_.transform.localPosition = transform.localPosition;
            enemy_type_.transform.parent = transform;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        game_ = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TD_Game>();
        current_move_speed_ = MOVE_SPEED_;
    }

    // Update is called once per frame
    void Update()
    {
        if (current_path_index >= list_path_.Count || list_path_[0] == null)
            return;

        if(list_path_[current_path_index].Content.Type == TileContentType.OIL && enemy_type_.Type != EnemyType.HOVERING)
        {
            current_move_speed_ = MOVE_SPEED_ / 5.0f;
        } else
        {
            current_move_speed_ = MOVE_SPEED_;
        }

        if (this.transform.position != list_path_[list_path_.Count - 1].transform.position)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, list_path_[current_path_index].transform.position, current_move_speed_ * Time.deltaTime);

            if (this.gameObject.transform.position == list_path_[current_path_index].transform.position)
            {
                if(current_path_index == list_path_.Count - 1)
                {
                    game_.RemoveLife();
                    game_.DespawnEnemy(this);
                }
                current_path_index++;
            }

            
        }
        
    }
    public void Damage(float arg_damage)
    {
        this.health_ -= arg_damage;
        if (health_ <= 0.0f)
        {
            is_dead = true;
            game_.KillEnemy(this);
        }

    }
    public bool isDead() { return is_dead; }
    public float GetHealth() { return health_; }
}
