using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class PropertyTile : Tile
{

    public string propertyName;
    public int price = 200;
    public int rent = 50;
    public PlayerController owner;
    public TextMeshProUGUI ownershipText;
    public float rentPercentage = 0.1f;

    /// <summary>
    /// Called when a player lands on this property tile. Handles rent payment or purchase option.
    /// </summary>
    /// <param name="player">The player who landed on this tile.</param>
    public override void OnLand(PlayerController player)
    {
        if (owner == null)
        {
            UIManager.Instance.ShowPurchaseOption(this, player);
        }
        else if (owner != player)
        {
            int rent = Mathf.RoundToInt(price * rentPercentage);
            player.money -= rent;
            owner.money += rent;
            owner.rentEarned += rent;

            Debug.Log($"{player.playerName} paid ${rent} rent to {owner.playerName}");
            UIManager.Instance.UpdatePlayerUI();
            UIManager.Instance.ShowRentMessage($"{player.playerName} paid ${rent} rent to {owner.playerName}");
        }
    }

    /// <summary>
    /// Allows the specified player to purchase this property, deducting money and assigning ownership.
    /// </summary>
    /// <param name="player">The player buying the property.</param>
    public void Buy(PlayerController player)
    {
        player.money -= price;
        owner = player;
        UIManager.Instance.UpdatePlayerUI();

        if (ownershipText != null)
        {
            ownershipText.text = $"{player.playerName} Owns";
            ownershipText.gameObject.SetActive(true);
        }

        Debug.Log($"{player.playerName} bought {propertyName} for ${price}");
    }
}
