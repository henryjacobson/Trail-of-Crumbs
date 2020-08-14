using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// copied from https://answers.unity.com/questions/476810/flip-a-mesh-inside-out.html
public class InvertibleMesh : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.triangles = mesh.triangles.Reverse().ToArray();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.triangles = mesh.triangles.Reverse().ToArray();
        }
    }
}
