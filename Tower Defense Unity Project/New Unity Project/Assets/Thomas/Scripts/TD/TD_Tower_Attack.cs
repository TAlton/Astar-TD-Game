using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Tower_Attack : MonoBehaviour
{
    //easy to implement new tower types based on prefabs
    [Range(0.5f, 25.0f)] public float range_;
    [SerializeField] private Collider[] collided_objects_;
    [SerializeField] private float fire_rate_;
    [SerializeField] private float damage_;

    
    // Start is called before the first frame update
    void Start()
    {
        //delay checks for performance
        InvokeRepeating("GetEnemiesInRange", 0f, 0.1f);
        InvokeRepeating("Fire", 0.0f, fire_rate_);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void GetEnemiesInRange()
    {
        collided_objects_ = Physics.OverlapSphere(this.transform.position, range_, (1 << LayerMask.NameToLayer("Enemies")));
    }
    void Fire()
    {
        if (collided_objects_.Length <= 0)
            return;
        if (collided_objects_[0] == null)
            return;
        TD_Enemy target_ = collided_objects_[0].gameObject.GetComponentInParent<TD_Enemy>();

        if (!target_.isDead())
        {
            target_.Damage(damage_);
            Debug.Log(target_.GetHealth());
        }
    }
}
