using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public List<MeshFilter> meshFilters;
    public List<MeshRenderer> meshRenderers;
    [SerializeField] private List<Material> materials;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;

    [ContextMenu("Combine Meshes")]
    private void CombineMeshes()
    {
        materials = new List<Material>();
        for (int i = 0; i < meshRenderers.Count; i++)
        {

            for (int j = 0; j < meshRenderers[i].sharedMaterials.Length; j++)
            {
                Material mat = meshRenderers[i].sharedMaterials[j];
                if (!materials.Contains(mat))
                {
                    materials.Add(mat);
                }
            }
        }
        var combine = new CombineInstance[meshFilters.Count];

        for(int i = 0; i < meshFilters.Count; i++)
        {
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        meshRenderer.sharedMaterials = materials.ToArray();
        meshFilter.mesh = mesh;
    }

}
