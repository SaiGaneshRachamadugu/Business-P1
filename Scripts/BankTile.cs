using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankTile : Tile
{
    public int feeAmount = 100;

    public override void OnLand(PlayerController player)
    {
        base.OnLand(player);
        player.money -= feeAmount;
        UIManager.Instance.UpdatePlayerUI();
        Debug.Log($"{player.name} paid bank fee of ${feeAmount}");
    }
}
