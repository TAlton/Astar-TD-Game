using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tom
{
    [System.Serializable] public class Vertex
    {
        [SerializeField] public Vector3 Position;
        [SerializeField] public HalfEdge Half_Edge;
        [SerializeField] public Triangle Triangle;
        [SerializeField] public Vertex PrevVert;
        [SerializeField] public Vertex NextVert;
        [SerializeField] public bool isConcave;
        [SerializeField] public bool isConvex;
        [SerializeField] public bool isEar;

        public Vertex(Vector3 argPos)
        {
            this.Position = argPos;
        }
        public Vector2 GetXZ()
        {
            return new Vector2(this.Position.x, this.Position.z);
        }
    }
}