using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Tower_Attack : MonoBehaviour
{
    [Range(0.5f, 25.0f)] public float range_;
    [SerializeField] private Collider[] collided_objects_;
    [SerializeField] private float fire_rate_;
    [SerializeField] private float damage_;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //InvokeRepeating("GetEnemiesInRange", 0f, 0.5f);
    }
    void GetEnemiesInRange()
    {
        collided_objects_ = Physics.OverlapSphere(this.transform.position, range_, (1 << LayerMask.NameToLayer("Enemies")));
    }
}
