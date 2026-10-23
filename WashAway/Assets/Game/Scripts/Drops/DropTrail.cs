using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrail : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;

    private Mesh mesh;

    private GUILayoutOption[] options;

    private void OnValidate()
    {
        if(mesh == null)
        {
            mesh = new Mesh();
            //meshFilter.sharedMesh = mesh;
        }

        RebuildMesh();
    }

    public void RebuildMesh()
    {
        Vector3[] vertices = new Vector3[4];

        vertices[0].x = 0.5f * -width;
        vertices[0].y = -height;

        vertices[1].x = 0.5f * width;
        vertices[1].y = -height;

        vertices[2].x = 0.5f * -width;
        vertices[2].y = 0;

        vertices[3].x = 0.5f * width;
        vertices[3].y = 0;

        int[] triangles = { 2, 1, 0, 2, 3, 1 };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2((vertices[i].x + 0.5f * width) / width, (vertices[i].y + height) / height);
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        // Prevents meshFilter.shardedMesh generating a warning.
        Invoke("ApplyMesh", 0);
    }

    private void ApplyMesh()
    {
        meshFilter.sharedMesh = mesh;
    }
}
