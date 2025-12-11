using UnityEngine;
using UnityEditor;

public class TileAutoFixer : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Fix All Tiles")]
    public static void FixAllTiles()
    {
        var tiles = FindObjectsOfType<Tile>();

        foreach (var tile in tiles)
        {
            GameObject tileObj = tile.gameObject;

            // Check for PropertyTile
            PropertyTile propTile = tileObj.GetComponent<PropertyTile>();
            if (propTile != null)
            {
                // Remove base Tile if PropertyTile exists
                DestroyImmediate(tile);
                Debug.Log($"[TileFixer] Fixed PropertyTile: {tileObj.name}");
            }
            else
            {
                Debug.Log($"[TileFixer] Tile {tileObj.name} is NOT a PropertyTile — keeping base Tile.");
            }

            // Ensure CenterPoint exists
            Transform cp = tileObj.transform.Find("CenterPoint");
            if (cp == null)
            {
                GameObject centerPoint = new GameObject("CenterPoint");
                centerPoint.transform.parent = tileObj.transform;
                centerPoint.transform.localPosition = Vector3.zero;
                Debug.LogWarning($"[TileFixer] CenterPoint added to: {tileObj.name}");
            }
        }

        Debug.Log("✅ Tile Fix Complete. Check scene.");
    }
#endif
}
