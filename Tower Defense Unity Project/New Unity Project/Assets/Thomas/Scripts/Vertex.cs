using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tom
{
    [System.Serializable] public class Vertex
    {
        public Vector3 Position;
        public HalfEdge Half_Edge;
        public Triangle Triangle;
        public Vertex PrevVert;
        public Vertex NextVert;
        public bool isConcave;
        public bool isConvex;
        public bool isEar;

        public Vertex(Vector3 argPos)
        {
            this.Position = argPos;
        }
        public Vector2 GetXZ()
        {
            return new Vector2(this.Position.x, this.Position.y);
        }
    }
}