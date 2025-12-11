using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailTile : Tile
{
    public override void OnLand(PlayerController player)
    {
        Debug.Log($"{player.playerName} landed on Go To Jail tile!");
        GameManager.Instance.SendPlayerToJail(player);
    }
}
