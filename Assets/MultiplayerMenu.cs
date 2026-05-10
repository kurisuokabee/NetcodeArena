using UnityEngine;
using Unity.Netcode;
using TMPro;

public class MultiplayerMenu : MonoBehaviour
{
    [SerializeField] GameObject hostButton;
    [SerializeField] GameObject clientButton;
    [SerializeField] GameObject serverButton;

    [SerializeField] TextMeshProUGUI infoText;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();

        HideButtons();

    }

    

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();

        HideButtons();

    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();

        HideButtons();
        
    }

    void UpdateInfoText()
    {
        ulong playerId = NetworkManager.Singleton.LocalClientId;

        int playerCount = NetworkManager.Singleton.ConnectedClientsList.Count;

        if (NetworkManager.Singleton.IsHost)
        {
            infoText.text =
                "HOST - Player #" + playerId +
                "\nPlayers Connected: " + playerCount;
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            infoText.text =
                "CLIENT - Player #" + playerId +
                "\nPlayers Connected: " + playerCount;
        }
        else if (NetworkManager.Singleton.IsServer)
        {
            infoText.text =
                "SERVER" +
                "\nPlayers Connected: " + playerCount;
        }
    }

    void Update()
    {
        if (NetworkManager.Singleton.IsListening)
        {
            UpdateInfoText();
        }
    }

    private void HideButtons()
    {
        hostButton.SetActive(false);
        clientButton.SetActive(false);
        serverButton.SetActive(false);
    }
}