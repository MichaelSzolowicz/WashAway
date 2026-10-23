using UnityEngine;

public class DropTrail : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;
    [SerializeField, Range(0.0f, 1.0f)] private float animationTime = 1.0f;

    private Mesh mesh;
    private MaterialPropertyBlock propertyBlock;
    private new Renderer renderer;

    private void OnValidate()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.sortingLayerName = "Trails";
        // This can be moved to an editor script if you need to optimize by
        // only running these functions when the corresponding values are modified.
        RebuildMesh();
        BuildPropertyBlock();
    }

    private void BuildPropertyBlock()
    {
        if (propertyBlock == null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }
        if(renderer == null)
        {
            renderer = GetComponent<Renderer>();
        }

        propertyBlock.SetFloat("_AnimationTime", animationTime);
        renderer.SetPropertyBlock(propertyBlock);
    }

    public void RebuildMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
        }

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
