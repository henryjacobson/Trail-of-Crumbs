using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// copied from https://answers.unity.com/questions/476810/flip-a-mesh-inside-out.html
public class InverseMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}
