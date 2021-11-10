using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUtils
{
  public static void ApplyTransformations(GameObject go, Matrix4x4 t, Vector3[] orig)
  {
    Mesh m = go.GetComponent<MeshFilter>().mesh;
    Vector3[] transformed = new Vector3[orig.Length];

    for (int i = 0; i < transformed.Length; i++)
    {
      Vector4 temp = new Vector4(orig[i].x, orig[i].y, orig[i].z, 1);
      transformed[i] = t * temp;
    }

    m.vertices = transformed;
    m.RecalculateNormals();
  }

  public static void ExtractVertices(GameObject go, List<Vector3[]> orig, string name = null)
  {
    if (name != null) go.name = name;

    Mesh m = go.GetComponent<MeshFilter>().mesh;
    Vector3[] o = new Vector3[m.vertices.Length];

    for (int i = 0; i < m.vertices.Length; i++)
    {
      o[i] = new Vector3(m.vertices[i].x, m.vertices[i].y, m.vertices[i].z);
    }

    orig.Add(o);
  }
}
