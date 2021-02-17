using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Vector3[] m_MeshVertices;
    [SerializeField] private List<Triangle> m_Triangles;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        m_MeshVertices              = this.GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i < m_MeshVertices.Length; i++)
        {
            m_MeshVertices[i]       = m_MeshVertices[i].normalized;
        }

        int[] lsArrTriangles        = this.GetComponent<MeshFilter>().mesh.triangles;
        for (int i = 0; i < lsArrTriangles.Length; i += 3)
        {
            Triangle lsTriangle = new Triangle(m_MeshVertices[i], m_MeshVertices[i + 1], m_MeshVertices[i + 2]);
            m_Triangles.Add(lsTriangle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void OrientClockwise(int[] argTriangles) {
        for(int i = 0; i < argTriangles.Length; i++)
        {

        }
    }
}
public class Triangle
{
    Vector3 v1, v2, v3;

    public Triangle(Vector3 argV1, Vector3 argV2, Vector3 argV3)
    {
        v1 = argV1;
        v2 = argV2;
        v3 = argV3;
    }
}