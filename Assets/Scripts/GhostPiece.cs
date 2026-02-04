using UnityEngine;

public class GhostPiece : MonoBehaviour
{
    public Material ghostBaseMaterial;

    GameObject ghostObject;
    CubeArray grid;
    Movement movement;

    void Start()
    {
        grid = GetComponent<CubeArray>();
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        if (movement.actualGroup == null)
            return;

        UpdateGhost();
    }

    void UpdateGhost()
    {
        if (ghostObject != null)
            Destroy(ghostObject);

        ghostObject = Instantiate(movement.actualGroup);
        Destroy(ghostObject.GetComponent<Rotation>());

        foreach (Transform block in ghostObject.transform)
        {
            block.tag = "Untagged";
            ApplyGhostMaterial(block);
        }

        while (true)
        {
            ghostObject.transform.position += Vector3.down;

            if (!grid.IsValidPosition(ghostObject.transform))
            {
                ghostObject.transform.position += Vector3.up;
                break;
            }
        }
    }

    void ApplyGhostMaterial(Transform block)
    {
        Renderer r = block.GetComponent<Renderer>();
        if (r == null) return;

        Material mat = new Material(ghostBaseMaterial);
        mat.color = new Color(0.7f, 0.7f, 0.7f, 0.35f);
        r.material = mat;
    }
}
