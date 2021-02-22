using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tom
{
   [System.Serializable] public class HalfEdge
    {
        [SerializeField] public Vertex Vert;
        [SerializeField] public Triangle Tri;
        [SerializeField] public HalfEdge NextEdge;
        [SerializeField] public HalfEdge PrevEdge;
        [SerializeField] public HalfEdge OppEdge;

        public HalfEdge(Vertex argVert)
        {
            this.Vert = argVert;
        }
    }
}