using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Tile : MonoBehaviour
{
    [SerializeField] public TD_Tile_Content content_;
    public TD_Tile_Content Content
    {
        get => content_;
        set
        {
            if (content_ != null)
            {
                content_.Recycle();
            }
            content_ = value;
            content_.transform.localPosition = transform.localPosition;
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
