using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Purchase UI")]
    public GameObject purchasePanel;
    public TextMeshProUGUI propertyNameText;
    public TextMeshProUGUI priceText;
    private PropertyTile currentTile;
    private PlayerController currentPlayer;
    public TextMeshProUGUI warningText;

    [Header("Player Info UI")]
    public TextMeshProUGUI player1InfoText;
    public TextMeshProUGUI player2InfoText;
    public TextMeshProUGUI player1RentText;
    public TextMeshProUGUI player1ProfitText;
    public TextMeshProUGUI player2RentText;
    public TextMeshProUGUI player2ProfitText;
    public TextMeshProUGUI rentMessageText;

    [Header("End Game UI")]
    public GameObject endGamePanel;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI runnerUpText;
    public TextMeshProUGUI summaryText;
    public ParticleSystem confetti;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPurchaseOption(PropertyTile tile, PlayerController player)
    {
        purchasePanel.SetActive(true);
        currentTile = tile;
        currentPlayer = player;
        propertyNameText.text = tile.name;
        priceText.text = "$" + tile.price;
    }

    public void OnConfirmPurchase()
    {
        if (currentPlayer.money < currentTile.price)
        {
            UIManager.Instance.ShowWarning("Not enough currency to buy this property!");
            purchasePanel.SetActive(false);
            return;
        }

        currentTile.Buy(currentPlayer);
        purchasePanel.SetActive(false);
    }

    public void OnDeclinePurchase()
    {
        purchasePanel.SetActive(false);
    }

    public void ShowWarning(string message)
    {
        warningText.text = message;
    }

    public void HideWarning()
    {
        warningText.text = "";
    }

    public void ShowRentMessage(string message)
    {
        rentMessageText.text = message;
        rentMessageText.gameObject.SetActive(true);
        StartCoroutine(HideRentMessage());
    }

    private IEnumerator HideRentMessage()
    {
        yield return new WaitForSeconds(3f);
        rentMessageText.gameObject.SetActive(false);
    }

    public void CloseApp()
    {
        Application.Quit();
        confetti.Stop();
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(0);
        confetti.Stop();
    }

    public void UpdatePlayerUI()
    {
        var player1 = GameManager.Instance.player1;
        var player2 = GameManager.Instance.player2;

        int player1Profit = player1.rentEarned + player1.bonusEarned;
        int player2Profit = player2.rentEarned + player2.bonusEarned;

        player1InfoText.text = $"{player1.playerName}: ${player1.money}";
        player2InfoText.text = $"{player2.playerName}: ${player2.money}";

        player1RentText.text = $"Rent Earned: ${player1.rentEarned}";
        player2RentText.text = $"Rent Earned: ${player2.rentEarned}";

        player1ProfitText.text = $"Profit: ${player1Profit}";
        player2ProfitText.text = $"Profit: ${player2Profit}";
    }

    public void ShowEndGamePopup(string winnerName, int winnerMoney, string runnerUpName, int runnerUpMoney)
    {
        endGamePanel.SetActive(true);
        winnerText.text = $"Winner: {winnerName} \nFinal Amount: ${winnerMoney}";
        runnerUpText.text = $"Runner-Up: {runnerUpName} \nFinal Amount: ${runnerUpMoney}";

        string summary = GenerateGameSummary();
        summaryText.text = summary;
    }

    private string GenerateGameSummary()
    {

        confetti.Play();
        var player1 = GameManager.Instance.player1;
        var player2 = GameManager.Instance.player2;

        List<string> player1Props = new List<string>();
        List<string> player2Props = new List<string>();

        foreach (Tile tile in GameManager.Instance.boardTiles)
        {
            if (tile is PropertyTile property)
            {
                if (property.owner == player1)
                    player1Props.Add(property.propertyName);
                else if (property.owner == player2)
                    player2Props.Add(property.propertyName);
            }
        }

        string p1Props = player1Props.Count > 0 ? string.Join(", ", player1Props) : "None";
        string p2Props = player2Props.Count > 0 ? string.Join(", ", player2Props) : "None";

        string summary = $"\n{player1.playerName} Properties: {p1Props}\n" +
                         $"{player2.playerName} Properties: {p2Props}";

        return summary;
    }
}
