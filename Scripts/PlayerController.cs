using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int rentEarned = 0;
    public int bonusEarned = 0;
    public int startingMoney = 1500;
    public int currentTileIndex = 0;
    public int money = 1500;
    public bool isInJail = false;
    public int jailTurnsLeft = 0;
    public string playerName = "Player 1";

    /// <summary>
    /// Tracks how many full laps (board rounds) the player has completed.
    /// </summary>
    public int completedLaps = 0;

    /// <summary>
    /// Tracks how many total turns the player has taken.
    /// Used for determining end of game.
    /// </summary>
    public int turnsPlayed = 0;

    /// <summary>
    /// Initiates movement of the player by a specified number of steps. 
    /// Movement is ignored if the player is in jail.
    /// </summary>
    /// <param name="steps">Number of tiles to move forward.</param>
    public void Move(int steps)
    {
        if (isInJail)
        {
            Debug.Log($"{playerName} is in jail. Cannot move.");
            return;
        }

        Debug.Log($"{playerName} starts moving {steps} steps from tile {currentTileIndex}");
        GameManager.Instance.isPlayerMoving = true;
        StartCoroutine(MoveStepByStep(steps));
    }

    /// <summary>
    /// Coroutine that moves the player tile-by-tile for the specified number of steps.
    /// Detects passing Start tile and increments completed laps.
    /// Triggers tile landing events after movement.
    /// </summary>
    /// <param name="steps">Number of tiles to move forward.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    private IEnumerator MoveStepByStep(int steps)
    {
        GameManager.Instance.isPlayerMoving = true;
        int totalTiles = GameManager.Instance.boardTiles.Length;

        for (int i = 0; i < steps; i++)
        {
            int nextTileIndex = (currentTileIndex + 1) % totalTiles;

            // Detect passing Start tile (tile index 0)
            if (nextTileIndex == 0)
            {
                completedLaps++;

                // Trigger Start tile bonus when passing it
                Tile startTile = GameManager.Instance.boardTiles[0];
                if (startTile is StartTile start)
                {
                    start.OnPassStart(this);
                }
            }

            Tile landedTile = GameManager.Instance.boardTiles[nextTileIndex];
            Vector3 tileCenter = landedTile.GetCenterPosition();

            Debug.Log($"{playerName} moving to tile index {nextTileIndex}: {landedTile.name}");

            yield return StartCoroutine(MoveSmooth(tileCenter));
            currentTileIndex = nextTileIndex;
        }

        Tile tile = GameManager.Instance.boardTiles[currentTileIndex];

        if (tile is PropertyTile propertyTile)
        {
            propertyTile.OnLand(this);
        }
        else
        {
            tile.OnLand(this);
        }
        GameManager.Instance.isPlayerMoving = false;
        Debug.Log($"Player landed on tile: {tile.name}");

        turnsPlayed++;
        if (turnsPlayed >= 30)
        {
            GameManager.Instance.CheckGameEndCondition();
        }
    }

    /// <summary>
    /// Coroutine that smoothly moves the player to a specified destination.
    /// </summary>
    /// <param name="destination">The position to move the player to.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    private IEnumerator MoveSmooth(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 5f);
            yield return null;
        }

        transform.position = destination;
    }

    /// <summary>
    /// Moves the player and waits until the movement is fully completed. 
    /// Used by other scripts (e.g., DiceController) to synchronize with player movement.
    /// </summary>
    /// <param name="steps">Number of steps to move.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    public IEnumerator MoveAndWait(int steps)
    {
        if (isInJail)
        {
            Debug.Log($"{playerName} is in jail. Cannot move.");
            yield break;
        }

        yield return StartCoroutine(MoveStepByStep(steps));
    }

    /// <summary>
    /// Adds a specified bonus to the player's money and tracks it only as profit.
    /// Called from StartTile when passing or landing after a lap.
    /// </summary>
    /// <param name="bonus">Amount to add as bonus.</param>
    public void AddStartBonusToProfit(int bonus)
    {
        money += bonus;
        bonusEarned += bonus;
        Debug.Log($"{playerName} received ${bonus} bonus. Total bonuses: ${bonusEarned}");
    }
}
