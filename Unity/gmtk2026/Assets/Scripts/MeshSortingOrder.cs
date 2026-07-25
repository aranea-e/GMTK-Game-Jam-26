using UnityEngine;

[ExecuteInEditMode] // Allows you to see changes instantly in the Scene view
[RequireComponent(typeof(MeshRenderer))]
public class MeshSortingOrder : MonoBehaviour
{
    [SerializeField] private string sortingLayerName = "Default";
    [SerializeField] private int sortingOrder = 0;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        ApplySorting();
    }

    private void OnValidate()
    {
        // Updates the visibility immediately when changed in the Inspector
        ApplySorting();
    }

    private void ApplySorting()
    {
        if (meshRenderer != null)
        {
            meshRenderer.sortingLayerName = sortingLayerName;
            meshRenderer.sortingOrder = sortingOrder;
        }
    }
}
