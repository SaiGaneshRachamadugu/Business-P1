using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSessionManager : MonoBehaviour
{
    
    public static GameSessionManager Instance;
    public string player1Name = "Player 1";
    public string player2Name = "Player 2";
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player2NameText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets player names, falling back to default if input is empty or null.
    /// Called typically from UI input fields.
    /// </summary>
    /// <param name="name1">Entered name for Player 1.</param>
    /// <param name="name2">Entered name for Player 2.</param>
    public void SetPlayerNames(string name1, string name2)
    {
        player1Name = string.IsNullOrEmpty(name1) ? "Player 1" : name1;
        player2Name = string.IsNullOrEmpty(name2) ? "Player 2" : name2;
    }

    /// <summary>
    /// Called automatically when a new scene is loaded.
    /// Assigns UI references for player name text fields in gameplay scene and updates them.
    /// </summary>
    /// <param name="scene">The scene that was loaded.</param>
    /// <param name="mode">The loading mode (Single or Additive).</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update player names only in the gameplay scene (build index 1)
        if (scene.buildIndex == 1)
        {
            player1NameText = GameObject.Find("Player1NameText")?.GetComponent<TextMeshProUGUI>();
            player2NameText = GameObject.Find("Player2NameText")?.GetComponent<TextMeshProUGUI>();

            if (player1NameText != null) player1NameText.text = player1Name;
            if (player2NameText != null) player2NameText.text = player2Name;
        }
    }
}
