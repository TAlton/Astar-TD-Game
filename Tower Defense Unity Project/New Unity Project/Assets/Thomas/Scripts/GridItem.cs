using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Thomas.Scripts
{
    [System.Serializable] public class GridItem
    {
        public GridItem Parent { get; set; }
        public int IndexX { get; set; }
        public int IndexY { get; set; }
        public int IndexZ { get; set; }
        public int Col { get; set; }
        public double g{ get; set; }
        public double h{ get; set; }
        double f;
        public double GetFCost() { fCost();  return f; }
        [SerializeField] public GameObject Tile { get; set; }
        public bool Blocked { get; set; }
        public bool Occupied { get; set; }
        public bool Active { get; set; }
        public GridItem(GameObject argGO, bool argActive = true)
        {
            this.Tile = argGO;
            Blocked = false;
            Occupied = false;
            Active = true;

        }
        public void fCost() { this.f = g + h; }
    }
}