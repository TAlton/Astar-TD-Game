using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Enemy : MonoBehaviour
{
    [SerializeField] TD_Enemy_Type enemy_type_;
    public List<TD_Tile> list_path_;
    private int current_path_index = 0;
    private float progress_ = 0.0f;
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
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        list_path_ = new List<TD_Tile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (current_path_index >= list_path_.Count || list_path_[0] == null)
            return;
        if (progress_ >= 1.0f)
        {
            progress_ = 0.0f + Time.deltaTime;
            current_path_index++;
        }
        this.gameObject.transform.position = Vector3.Lerp(this.transform.position, list_path_[current_path_index].transform.position, (progress_));
        enemy_type_.gameObject.transform.position = this.transform.position;
        progress_ += Time.deltaTime;
    }
}
