using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Tile : MonoBehaviour
{
    public bool is_blocking_ = false;
    public TD_Tile Parent { get; set; }
    public int IndexX { get; set; }
    public int IndexY { get; set; }
    public int IndexZ { get; set; }
    public int Col { get; set; }
    public double g { get; set; }
    public double h { get; set; }
    double f;
    public double GetFCost() { fCost(); return f; }
    public void fCost() { this.f = g + h; }
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
