using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChanceTile : Tile
{
    /// <summary>
    /// Called when a player lands on this Chance tile.
    /// Triggers a chance event based on defined probabilities.
    /// </summary>
    /// <param name="player">The player who landed on the tile.</param>
    public override void OnLand(PlayerController player)
    {

        base.OnLand(player);
        int chance = Random.Range(1, 101);

        if (chance <= 90)
        {
            //90% chance Deduct 10-30% of player's money
            float lossPercentage = Random.Range(0.1f, 0.31f);
            int lossAmount = Mathf.RoundToInt(player.money * lossPercentage);
            player.money -= lossAmount;
            Debug.Log($"{player.playerName} lost ${lossAmount} ({Mathf.RoundToInt(lossPercentage * 100)}%) due to chance event.");

            //optional 25% chance within this 90% to be sent to jail as additional penalty
            if (Random.value <= 0.25f)
            {
                GameManager.Instance.SendPlayerToJail(player);
                Debug.Log($"{player.playerName} also sent to jail!");
            }
        }
        else
        {
            //10% chance Award 50% bonus of current money
            int bonus = Mathf.RoundToInt(player.money * 0.1f);
            player.money += bonus;
            Debug.Log($"{player.playerName} received a bonus of ${bonus} from chance event!");
        }

        UIManager.Instance.UpdatePlayerUI();
    }
}
