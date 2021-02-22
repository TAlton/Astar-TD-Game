using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tom
{
   [System.Serializable] public class HalfEdge
    {
        public Vertex Vert;
        public Triangle Tri;
        public HalfEdge NextEdge;
        public HalfEdge PrevEdge;
        public HalfEdge OppEdge;

        public HalfEdge(Vertex argVert)
        {
            this.Vert = argVert;
        }
    }
}