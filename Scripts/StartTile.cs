using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class StartTile : Tile
{
    public int startTileIndex = 0;

    /// <summary>
    /// Called when a player lands on or passes the Start tile.
    /// Awards a random bonus between $100, $150, or $200 only if they have completed at least one full lap.
    /// </summary>
    /// <param name="player">The player who reached the Start tile.</param>
    public override void OnLand(PlayerController player)
    {

        base.OnLand(player);
        if (player.currentTileIndex == startTileIndex && player.completedLaps > 0)
        {
            GrantStartBonus(player);
        }
    }

    /// <summary>
    /// Called externally when the player crosses tile index 0 during movement (not just on land).
    /// This should be triggered from PlayerController.
    /// </summary>
    public void OnPassStart(PlayerController player)
    {
        if (player.completedLaps > 0)
        {
            GrantStartBonus(player);
        }
    }

    /// <summary>
    /// Grants a random bonus ($100, $150, or $200) to the player and updates their profit.
    /// </summary>
    private void GrantStartBonus(PlayerController player)
    {
        int[] bonuses = { 100, 150, 200 };
        int bonus = bonuses[Random.Range(0, bonuses.Length)];

        player.money += bonus;
        player.rentEarned += bonus;

        Debug.Log($"{player.playerName} received a start tile bonus of ${bonus}!");
        UIManager.Instance.UpdatePlayerUI();
    }
}
