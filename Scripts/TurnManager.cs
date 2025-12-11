using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TurnManager : MonoBehaviour
{

    public static TurnManager Instance;
    [SerializeField]
    private TextMeshProUGUI turnIndicatorText;
    [SerializeField]
    private Button rollDiceButton;
    private int consecutiveSixes = 0;
    public GameObject dicePlayer1;
    public GameObject dicePlayer2;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Starts the turn for the given player. Enables the correct dice and handles jail logic.
    /// </summary>
    /// <param name="player">The player whose turn is starting.</param>
    public void StartTurn(PlayerController player)
    {
        turnIndicatorText.text = $"{player.playerName}'s Turn";

        if (player.isInJail)
        {
            player.jailTurnsLeft--;
            if (player.jailTurnsLeft <= 0)
            {
                player.isInJail = false;
                Debug.Log($"{player.playerName} released from jail!");

                //Player is now free, allow dice roll
                EnableDiceForActivePlayer();
            }
            else
            {
                Debug.Log($"{player.playerName} is in jail. Skipping turn.");
                StartCoroutine(SkipTurnAfterDelay());
            }
        }
        else
        {
            EnableDiceForActivePlayer();
        }

    }
    private void EnableDiceForActivePlayer()
    {
        // Only allow rolling if pawn is NOT moving
        rollDiceButton.interactable = !GameManager.Instance.isPlayerMoving;

        if (GameManager.Instance.activePlayer == GameManager.Instance.player1)
        {
            dicePlayer1.SetActive(true);
            dicePlayer2.SetActive(false);
        }
        else
        {
            dicePlayer1.SetActive(false);
            dicePlayer2.SetActive(true);
        }
    }



    private IEnumerator SkipTurnAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        EndTurn();
    }

    /// <summary>
    /// Called after a dice is rolled. Handles re-roll on six or switches turn otherwise.
    /// </summary>
    /// <param name="result">The result of the dice roll (1-6).</param>
    public void OnDiceRolled(int result)
    {
        if (result == 6)
        {
            consecutiveSixes++;
            Debug.Log($"Player rolled 6! Roll again ({consecutiveSixes}x)");

            rollDiceButton.interactable = true;
        }
        else
        {
            rollDiceButton.interactable = false;
            GameManager.Instance.EndTurn();
        }
    }

    /// <summary>
    /// Ends the current player's turn and passes control to the next player.
    /// </summary>
    public void EndTurn()
    {
        rollDiceButton.interactable = false;
        GameManager.Instance.EndTurn();
    }
}
