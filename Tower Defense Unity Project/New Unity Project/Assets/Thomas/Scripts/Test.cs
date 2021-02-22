using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tom;
public class Test : MonoBehaviour
{
    [SerializeField] private Vector3[] m_MeshVertices;
    [SerializeField] private List<Triangle> m_Triangles = new List<Triangle>();
    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {

        Triangulate();
       
    }

    // Update is called once per frame
    void Update()
    {
    }
    public static void OrientClockwise(List<Triangle> argTriangles) {
        for(int i = 0; i < argTriangles.Count; i++)
        {
            //make all tris clockwise
            if (!IsClockwise(argTriangles[i])) argTriangles[i].FlipOrientation();
        }
    }
    public static bool IsClockwise(Triangle argTri)
    {
        //a curve is clockwise if you get a positive value when going start to finish vice versa given that there is no intersection
        float lsDeterminant                 = argTri.v1.Position.x * argTri.v2.Position.y + 
                                                argTri.v3.Position.x * argTri.v1.Position.y + 
                                                argTri.v2.Position.x * argTri.v3.Position.y - 
                                                argTri.v1.Position.x * argTri.v3.Position.y - 
                                                argTri.v3.Position.x * argTri.v2.Position.y - 
                                                argTri.v2.Position.x * argTri.v1.Position.y;

        if (lsDeterminant > 0f) return false;

        return true;
    }
    public static bool IsClockwise(Vector2 argV1, Vector2 argV2, Vector2 argV3)
    {
        //a curve is clockwise if you get a positive value when going start to finish vice versa given that there is no intersection
        float lsDeterminant                 = argV1.x * argV1.y +
                                                argV3.x * argV1.y +
                                                argV1.x * argV3.y -
                                                argV1.x * argV3.y -
                                                argV3.x * argV1.y -
                                                argV1.x * argV1.y;

        if (lsDeterminant > 0f) return false;

        return true;
    }
    public static void ChangeOrientation(Triangle argTriangle)
    {

    }
    public static List<HalfEdge> TransformToHalfEdge(List<Triangle> argTris)
    {
        OrientClockwise(argTris);

        List<HalfEdge> lsHalfEdges      = new List<HalfEdge>(argTris.Count * 3);

        for(int i = 0; i < argTris.Count; i++)
        {
            Triangle lsTri              = argTris[i];

            HalfEdge lsHE1              = new HalfEdge(lsTri.v1);
            HalfEdge lsHE2              = new HalfEdge(lsTri.v2);
            HalfEdge lsHE3              = new HalfEdge(lsTri.v3);

            lsHE1.NextEdge              = lsHE2;
            lsHE2.NextEdge              = lsHE3;
            lsHE3.NextEdge              = lsHE1;
            lsHE1.PrevEdge              = lsHE3;
            lsHE2.PrevEdge              = lsHE1;
            lsHE3.PrevEdge              = lsHE2;
            lsHE1.Vert.Half_Edge        = lsHE2;
            lsHE2.Vert.Half_Edge        = lsHE3;
            lsHE3.Vert.Half_Edge        = lsHE1;
            lsTri.Half_Edge             = lsHE1;
            lsHE1.Tri                   = lsTri;
            lsHE2.Tri                   = lsTri;
            lsHE3.Tri                   = lsTri;

            lsHalfEdges.Add(lsHE1);
            lsHalfEdges.Add(lsHE2);
            lsHalfEdges.Add(lsHE3);

        }
        for(int i = 0; i < lsHalfEdges.Count; i++)
        {
            HalfEdge lsHE               = lsHalfEdges[i];
            Vertex VertTo               = lsHE.Vert;
            Vertex VertFrom             = lsHE.PrevEdge.Vert;

            for(int ii = 0; ii < lsHalfEdges.Count; ii++)
            {
                if (ii == i) continue;

                HalfEdge lsOppEdge      = lsHalfEdges[ii];

                if(VertFrom.Position == lsOppEdge.Vert.Position &&
                    VertTo.Position == lsOppEdge.PrevEdge.Vert.Position)
                {
                    lsHE.OppEdge        = lsOppEdge;
                    break;
                }
            }
        }

        return lsHalfEdges;

    }
    //argV4 is the point; 1,2,3 are the points of the triangle creating the circle
    //+ve == inside, -ve == outside, 0 == on the line
    public static float IsPointInCircle(Vector2 argV1, Vector2 argV2, Vector2 argV3, Vector2 argV4)
    {
        float lsA                       = argV1.x - argV4.x;
        float lsA1                      = argV2.x - argV4.x;
        float lsA2                      = argV3.x - argV4.x;
        float lsB                       = argV1.y - argV4.y;
        float lsB1                      = argV2.y - argV4.y;
        float lsB2                      = argV3.y - argV4.y;
        float lsC                       = lsA * lsA + lsB * lsB;
        float lsC1                      = lsA1 * lsA1 + lsB1 * lsB1;
        float lsC2                      = lsA2 * lsA2 + lsB2 * lsB2;

        return (lsA * lsB1 * lsC2) + (lsB * lsC1 * lsA2) + (lsC * lsA1 * lsB2) -
                    (lsA2 * lsB1 * lsC) - (lsB2 * lsC1 * lsA) - (lsC2 * lsA1 * lsB);
    }
    public static bool IsQuadConvex(Vector2 argV1, Vector2 argV2, Vector2 argV3, Vector2 argV4)
    {
        bool B123                       = IsClockwise(argV1, argV2, argV3);
        bool B124                       = IsClockwise(argV1, argV2, argV4);
        bool B234                       = IsClockwise(argV2, argV3, argV4);
        bool B314                       = IsClockwise(argV3, argV1, argV4);

        if (B123 && B124 && B234 & !B314) return true;
        if (B123 && B124 && !B234 & B314) return true;
        if (B123 && !B124 && B234 & B314) return true;
        if (!B123 && !B124 && !B234 & B314) return true;
        if (!B123 && !B124 && B234 & !B314) return true;
        if (!B123 && B124 && !B234 & !B314) return true;

        return false;
    }
    private void OnDrawGizmos()
    {
        
    }
    private static void FlipEdge(HalfEdge argFirstEdge)
    {
        HalfEdge lsSecondEdge           = argFirstEdge.NextEdge;
        HalfEdge lsThirdEdge            = argFirstEdge.PrevEdge;
        HalfEdge lsFourthEdge           = argFirstEdge.OppEdge;
        HalfEdge lsFifthEdge            = argFirstEdge.OppEdge.NextEdge;
        HalfEdge lsSixthEdge            = argFirstEdge.OppEdge.PrevEdge;

        Vertex lsFirstVert              = argFirstEdge.Vert;
        Vertex lsSecondVert             = lsSecondEdge.Vert;
        Vertex lsThirdVert              = lsThirdEdge.Vert;
        Vertex lsFourthVert             = lsFifthEdge.Vert;

        lsFirstVert.Half_Edge = lsSecondEdge;
        lsThirdVert.Half_Edge = lsFifthEdge;

        argFirstEdge.NextEdge = lsThirdEdge;
        argFirstEdge.PrevEdge = lsFifthEdge;

        lsSecondEdge.NextEdge = lsFourthEdge;
        lsSecondEdge.PrevEdge = lsSixthEdge;

        lsThirdEdge.NextEdge = lsFifthEdge;
        lsThirdEdge.PrevEdge = argFirstEdge;

        lsFourthEdge.NextEdge = lsSixthEdge;
        lsFourthEdge.PrevEdge = lsSecondEdge;

        lsFifthEdge.NextEdge = argFirstEdge;
        lsFifthEdge.PrevEdge = lsThirdEdge;

        lsSixthEdge.NextEdge = lsSecondEdge;
        lsSixthEdge.PrevEdge = lsFourthEdge;

        argFirstEdge.Vert = lsSecondVert;
        lsSecondEdge.Vert = lsSecondVert;
        lsThirdEdge.Vert = lsThirdVert;
        lsFourthEdge.Vert = lsFourthVert;
        lsFifthEdge.Vert = lsFourthVert;
        lsSixthEdge.Vert = lsFirstVert;

        Triangle lsFirstTri = argFirstEdge.Tri;
        Triangle lsSecondTri = lsFourthEdge.Tri;

        argFirstEdge.Tri = lsFirstTri;
        lsThirdEdge.Tri = lsFirstTri;
        lsFifthEdge.Tri = lsFirstTri;
        lsSecondEdge.Tri = lsSecondTri;
        lsFourthEdge.Tri = lsSecondTri;
        lsSixthEdge.Tri = lsSecondTri;

        lsFirstTri.v1 = lsSecondVert;
        lsFirstTri.v2 = lsThirdVert;
        lsFirstTri.v3 = lsFourthVert;
        lsSecondTri.v1 = lsSecondVert;
        lsSecondTri.v2 = lsFourthVert;
        lsSecondTri.v3 = lsFirstVert;

        lsFirstTri.Half_Edge = lsThirdEdge;
        lsSecondTri.Half_Edge = lsFourthEdge;
    }
    public List<Triangle> Triangulate()
    {

        m_MeshVertices = this.GetComponent<MeshFilter>().mesh.vertices;

        for (int i = 0; i < m_MeshVertices.Length; i++)
        {
            m_MeshVertices[i]       = m_MeshVertices[i].normalized;
        }

        int[] lsArrTriangles = this.GetComponent<MeshFilter>().mesh.triangles;

        for (int i = 0; i < (lsArrTriangles.Length - 3); i += 3)
        {
            m_Triangles.Add(new Triangle(m_MeshVertices[lsArrTriangles[i]], m_MeshVertices[lsArrTriangles[i + 1]], m_MeshVertices[lsArrTriangles[i + 2]]));

        }

        List<HalfEdge> lsEdges = TransformToHalfEdge(m_Triangles);
        int lsSafety = 0;
        int lsFlippedEdgeCount = 0;

        while(true)
        {
            lsSafety++;
            if (lsSafety > 100000) break; //find a way to generate a number larger than the predicted count

            bool lsHasFlipped       = false;

            for(int i = 0; i < lsEdges.Count; i++)
            {
                HalfEdge lsHalfEdge = lsEdges[i];

                if (null == lsHalfEdge.OppEdge) continue;

                Vertex lsA          = lsHalfEdge.Vert;
                Vertex lsB          = lsHalfEdge.NextEdge.Vert;
                Vertex lsC          = lsHalfEdge.PrevEdge.Vert;
                Vertex lsD          = lsHalfEdge.OppEdge.NextEdge.Vert;

                Vector2 lsAPos      = lsA.GetXZ();
                Vector2 lsBPos      = lsB.GetXZ();
                Vector2 lsCPos      = lsC.GetXZ();
                Vector2 lsDPos      = lsD.GetXZ();

                //is point outside circle
                if(IsPointInCircle(lsAPos, lsBPos, lsCPos, lsDPos) < 0f) 
                {
                    if(IsQuadConvex(lsAPos, lsBPos, lsCPos, lsDPos))
                    {
                        //if after flip the new tri isnt better dont flip
                        if (IsPointInCircle(lsBPos, lsCPos, lsDPos, lsAPos) < 0f) continue;

                        lsFlippedEdgeCount++;
                        lsHasFlipped = true;
                        FlipEdge(lsHalfEdge);
                        Debug.Log("flip");
                    }
                }
            }

            if (!lsHasFlipped) break;
        }
        return m_Triangles;
    }
}