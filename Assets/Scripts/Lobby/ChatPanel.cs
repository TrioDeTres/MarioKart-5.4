using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour
{
    public static ChatPanel singleton;

    public Scrollbar verticalScrollBar;
    public GameObject messageList;
    public GameObject messagePrefab;
    public InputField messageInput;

    public void Start()
    {
        singleton = this;
    }

    public void CreateMessage(string message, string playerNameWhoSentMessage)
    {
        GameObject messageObject = Instantiate(messagePrefab, messageList.transform, false) as GameObject;

        Text textMessageObject = messageObject.GetComponentInChildren<Text>();
        textMessageObject.text = playerNameWhoSentMessage + " says: " + message;

        messageInput.text = string.Empty;
        messageInput.Select();
        messageInput.ActivateInputField();

        StartCoroutine(VerticalScrollBarDelay());
    }

    private IEnumerator VerticalScrollBarDelay()
    {
        yield return new WaitForSeconds(0.075f);

        verticalScrollBar.value = -0.5f;
    }
}
