using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        m_MeshVertices = this.GetComponent<MeshFilter>().mesh.vertices;

        for (int i = 0; i < m_MeshVertices.Length; i++)
        {
            m_MeshVertices[i] = m_MeshVertices[i].normalized;
        }

        int[] lsArrTriangles = this.GetComponent<MeshFilter>().mesh.triangles;

        for (int i = 0; i < (lsArrTriangles.Length - 3); i += 3)
        {
            m_Triangles.Add(new Triangle(m_MeshVertices[lsArrTriangles[i]], m_MeshVertices[lsArrTriangles[i + 1]], m_MeshVertices[lsArrTriangles[i + 2]]));

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void OrientClockwise(List<Triangle> argTriangles) {
        for(int i = 0; i < argTriangles.Count; i++)
        {
            //make all tris clockwise
            if (!IsClockwise(argTriangles[i])) ChangeOrientation(argTriangles[i]);
        }
    }
    public static bool IsClockwise(Triangle argTri)
    {
        //a curve is clockwise if you get a positive value when going start to finish vice versa given that there is no intersection
        float lsDeterminant = argTri.v1.x * argTri.v2.y + argTri.v3.x * argTri.v1.y + argTri.v2.x * argTri.v3.y - argTri.v1.x * argTri.v3.y - argTri.v3.x * argTri.v2.y - argTri.v2.x * argTri.v1.y;

        if (lsDeterminant > 0f) return false;

        return true;
    }
    public static void ChangeOrientation(Triangle argTriangle)
    {

    }
}

[System.Serializable] public class Triangle
{
    [SerializeField] public Vector3 v1, v2, v3;

    public Triangle(Vector3 argV1, Vector3 argV2, Vector3 argV3)
    {
        v1 = argV1;
        v2 = argV2;
        v3 = argV3;
    }
}