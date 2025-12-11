using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiceController : MonoBehaviour
{
    public Image diceImage;
    public Sprite[] diceFaces;
    private bool rolling = false;
    public bool isPlayer1Dice;
    private bool pawnMoving = false;
    public void RollDice()
    {
        //prevent multiple rolls during movement
        if (rolling || GameManager.Instance.isPlayerMoving)
        {
            Debug.Log("Cannot roll - still moving or rolling.");
            return;
        }
        //check if it's this player's turn
        bool isCorrectTurn = (isPlayer1Dice && GameManager.Instance.activePlayer == GameManager.Instance.player1)
                          || (!isPlayer1Dice && GameManager.Instance.activePlayer == GameManager.Instance.player2);

        if (!isCorrectTurn)
        {
            Debug.Log("Not your turn!");
            return;
        }
        StartCoroutine(Roll());
    }


    private IEnumerator Roll()
    {
        rolling = true;
        int result = 0;

        for (int i = 0; i < 10; i++)
        {
            result = Random.Range(1, 7);
            diceImage.sprite = diceFaces[result - 1];
            yield return new WaitForSeconds(0.1f);
        }

        rolling = false;

        //Start pawn movement and block further rolls
        pawnMoving = true;

        yield return GameManager.Instance.activePlayer.MoveAndWait(result);

        pawnMoving = false;
        TurnManager.Instance.OnDiceRolled(result);
    }

}