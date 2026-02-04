using UnityEngine;

public class NextPiecePreview : MonoBehaviour
{
    public GroupSpawner spawner;
    public Transform previewAnchor;

    GameObject previewObject;

    public void UpdatePreview()
    {
        

        if (spawner == null || previewAnchor == null)
        {
            
            return;
        }

        if (previewObject != null)
            Destroy(previewObject);

        GameObject prefab = spawner.GetNextPrefab();
        

        previewObject = Instantiate(prefab);
        previewObject.transform.position = previewAnchor.position;
        previewObject.transform.localScale = Vector3.one * 1.2f;

        foreach (Transform block in previewObject.transform)
            block.tag = "Untagged";
    }
}
