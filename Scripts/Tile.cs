using UnityEngine;

public class Tile : MonoBehaviour
{
    public Transform centerPoint;

    void Awake()
    {
        centerPoint = transform.Find("CenterPoint");
        if (centerPoint == null)
            Debug.LogError("CenterPoint not found on tile: " + name);
    }

    public Vector3 GetCenterPosition()
    {
        return centerPoint.position;
    }

    // Mark this as virtual
    public virtual void OnLand(PlayerController player)
    {
        Debug.Log($"[Tile.cs] {player.playerName} landed on tile: {name} ({this.GetType().Name})");
    }
}
