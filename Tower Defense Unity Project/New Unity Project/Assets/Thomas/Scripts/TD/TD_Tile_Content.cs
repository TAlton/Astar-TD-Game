using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileContentType { EMPTY = 0, DESTINATION, WALL, TOWER, SPAWN, OIL}

public class TD_Tile_Content : MonoBehaviour
{
    [SerializeField] TileContentType type_ = TileContentType.EMPTY;
    TD_Tile_Factory origin_;
    public TileContentType Type => type_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public TD_Tile_Factory OriginFactory
    {
        get => origin_;
        set
        {
            origin_ = value;
        }
    }
    public void Recycle()
    {
        origin_.Reclaim(this);
    }
}
