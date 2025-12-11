using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerSetupUI : MonoBehaviour
{

    public GameObject nameInputPanel;
    public TMP_InputField player1InputField;
    public TMP_InputField player2InputField;

    /// <summary>
    /// Displays the player name input panel when called.
    /// Typically triggered by a Start or Setup button.
    /// </summary>
    public void ShowNameInputPanel()
    {
        nameInputPanel.SetActive(true);
    }

    /// <summary>
    /// Called when the player clicks the "Continue" button.
    /// Sets the player names using GameSessionManager and loads the main gameplay scene.
    /// </summary>
    public void OnContinueClicked()
    {
        //retrieve and trim input values to remove extra spaces
        string name1 = player1InputField.text.Trim();
        string name2 = player2InputField.text.Trim();

        //save player names for use in the next scene
        GameSessionManager.Instance.SetPlayerNames(name1, name2);

        //load the main gameplay scene (assumes build index 1)
        SceneManager.LoadScene(1);
    }
}
