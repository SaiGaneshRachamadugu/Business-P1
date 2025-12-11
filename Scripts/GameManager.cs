using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController player1;
    public PlayerController player2;
    public PlayerController activePlayer;
    public Tile[] boardTiles;
    public Tile jailTile;
    public bool isPlayerMoving = false;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        //set player names from GameSessionManager if provided
        if (GameSessionManager.Instance != null)
        {
            player1.playerName = GameSessionManager.Instance.player1Name;
            player2.playerName = GameSessionManager.Instance.player2Name;
        }

        //place both players on START tile (Tile 0)
        Vector3 startPos = boardTiles[0].GetCenterPosition();
        player1.currentTileIndex = 0;
        player1.transform.position = startPos;

        player2.currentTileIndex = 0;
        player2.transform.position = startPos;

        //set first turn
        activePlayer = player1;
        TurnManager.Instance.StartTurn(activePlayer);

        //update UI with initial player info
        UIManager.Instance.UpdatePlayerUI();
    }

    /// <summary>
    /// Ends the current player's turn and switches to the next player.
    /// </summary>
    public void EndTurn()
    {
        activePlayer = (activePlayer == player1) ? player2 : player1;
        TurnManager.Instance.StartTurn(activePlayer);
    }

    /// <summary>
    /// Sends the specified player to jail, placing them on the jail tile and setting jail duration.
    /// </summary>
    /// <param name="player">The player to send to jail.</param>
    public void SendPlayerToJail(PlayerController player)
    {
        player.isInJail = true;
        player.jailTurnsLeft = 3;
        Vector3 jailPos = jailTile.GetCenterPosition();
        player.currentTileIndex = GetTileIndex(jailTile);
        player.transform.position = jailPos;

        Debug.Log($"{player.playerName} sent to jail for 3 turns.");
    }

    /// <summary>
    /// Retrieves the index of a specific tile within the boardTiles array.
    /// </summary>
    /// <param name="tile">The tile to find the index of.</param>
    /// <returns>The index of the tile, or -1 if not found.</returns>
    public int GetTileIndex(Tile tile)
    {
        for (int i = 0; i < boardTiles.Length; i++)
        {
            if (boardTiles[i] == tile)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Checks whether both players have completed 30 turns and ends the game if so.
    /// Determines the winner based on total money and profit.
    /// </summary>
    public void CheckGameEndCondition()
    {
        if (player1.turnsPlayed >= 30 && player2.turnsPlayed >= 30)
        {
            Debug.Log("Game Over - Calculating Results");

            int player1Total = player1.money + player1.bonusEarned;
            int player2Total = player2.money + player2.bonusEarned;

            PlayerController winner = (player1Total >= player2Total) ? player1 : player2;
            PlayerController runnerUp = (winner == player1) ? player2 : player1;

            UIManager.Instance.ShowEndGamePopup(winner.playerName, winner.money, runnerUp.playerName, runnerUp.money);
        }
    }
}
