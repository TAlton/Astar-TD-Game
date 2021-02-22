using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tom
{
    [System.Serializable] public class Edge
    {
        public Vertex v1;
        public Vertex v2;

        public Edge(Vertex argV1, Vertex argV2)
        {
            this.v1 = argV1;
            this.v2 = argV2;
        }
        public Edge(Vector3 argV1, Vector3 argV2)
        {
            this.v1 = new Vertex(argV1);
            this.v2 = new Vertex(argV2);
        }
        public Vector2 GetXZ(Vertex argVert)
        {
            return new Vector2(argVert.Position.x, argVert.Position.y);
        }
        public void FlipEdge()
        {
            Vertex lsVert = this.v1;
            this.v1 = this.v2;
            this.v2 = lsVert;
        }
    }
}