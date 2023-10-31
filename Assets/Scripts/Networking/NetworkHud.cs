using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkHud : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public Button startButton;
    public TextMeshProUGUI connLabel;

    public bool IsHosting = false;

    public void ToggleButton(Button button, bool state)
    {
        button.gameObject.SetActive(state);
    }

    public void ShowStart() {
        ToggleButton(hostButton, false);
        ToggleButton(clientButton, false);
        ToggleButton(startButton, true);
    }

    public void WaitHost() {
        ToggleButton(hostButton, false);
        ToggleButton(clientButton, false);

        ToggleButton(startButton, true);
        startButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Wait...";
        startButton.enabled = false;
    }
}
