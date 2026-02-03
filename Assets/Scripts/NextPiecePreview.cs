using UnityEngine;

public class NextPiecePreview : MonoBehaviour
{
    public GroupSpawner spawner;
    public Transform previewAnchor;

    GameObject previewObject;

    public void UpdatePreview()
    {
        Debug.Log("UpdatePreview CALLED");

        if (spawner == null || previewAnchor == null)
        {
            Debug.LogError("Spawner or Anchor NOT assigned");
            return;
        }

        if (previewObject != null)
            Destroy(previewObject);

        GameObject prefab = spawner.GetNextPrefab();
        Debug.Log("Next prefab: " + prefab.name);

        previewObject = Instantiate(prefab);
        previewObject.transform.position = previewAnchor.position;
        previewObject.transform.localScale = Vector3.one * 1.2f;

        foreach (Transform block in previewObject.transform)
            block.tag = "Untagged";
    }
}
