using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { DEFAULT = 1, HOVERING, FLYING }
public class TD_Enemy_Type : MonoBehaviour
{
    [SerializeField] EnemyType type_ = EnemyType.DEFAULT;
    TD_Enemy_Factory origin_;
    
    public EnemyType Type => type_;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TD_Enemy_Factory OriginFactory
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
