using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Camera_Movement : MonoBehaviour
{
    [SerializeField] public KeyCode bind_forward_;
    [SerializeField] public KeyCode bind_backward_;
    [SerializeField] public KeyCode bind_left_;
    [SerializeField] public KeyCode bind_right_;
    [SerializeField] public KeyCode bind_turn_right_;
    [SerializeField] public KeyCode bind_turn_left_;
    [SerializeField] public float move_speed_ = 5f;
    [SerializeField] public float turn_speed_ = 90f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(bind_forward_)) this.transform.position += this.transform.up * move_speed_ * Time.deltaTime;
        if (Input.GetKey(bind_backward_)) this.transform.position += this.transform.up  * -move_speed_ * Time.deltaTime;
        if (Input.GetKey(bind_left_)) this.transform.position += this.transform.right * -move_speed_ * Time.deltaTime;
        if (Input.GetKey(bind_right_)) this.transform.position += this.transform.right * move_speed_ * Time.deltaTime;
        if (Input.GetKey(bind_turn_right_)) this.transform.Rotate(Vector3.forward, turn_speed_ * Time.deltaTime);
        if (Input.GetKey(bind_turn_left_)) this.transform.Rotate(Vector3.forward, -turn_speed_ * Time.deltaTime);
    }
}
