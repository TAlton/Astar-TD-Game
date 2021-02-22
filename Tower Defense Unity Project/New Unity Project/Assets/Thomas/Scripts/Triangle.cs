using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tom
{
    [System.Serializable] public class Triangle
    {
        public Vertex v1, v2, v3;
        public HalfEdge Half_Edge;

        public Triangle(Vertex argV1, Vertex argV2, Vertex argV3)
        {
            this.v1 = argV1;
            this.v2 = argV2;
            this.v3 = argV3;
        }
        public Triangle(Vector3 argV1, Vector3 argV2, Vector3 argV3)
        {
            this.v1 = new Vertex(argV1);
            this.v2 = new Vertex(argV2);
            this.v3 = new Vertex(argV3);
        }
        public Triangle(HalfEdge argHalfEdge)
        {
            this.Half_Edge = argHalfEdge;
        }
        public void FlipOrientation()
        {
            Vertex lsVert = this.v1;
            this.v1 = this.v2;
            this.v2 = lsVert;

        }
    }
}