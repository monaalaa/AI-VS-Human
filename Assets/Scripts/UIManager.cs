using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Animator PanelActionAnimator;
    public Text Message;

    public GameObject ActionPanel;

    private string textToNotify;

    internal string TextToNotify
    {
        get => textToNotify;
        set
        {
            textToNotify = value;
            ShowPanel();
        }
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        UnitsManager.ReadyToAction += EnableActionPanel;
    }
    void ShowPanel()
    {
        Message.text = TextToNotify;
        StartCoroutine(PlayAnimation());
    }
    IEnumerator PlayAnimation()
    {
        if (!PanelActionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Show"))
        {
            PanelActionAnimator.SetTrigger("ShowPanel");
            yield return new WaitForSeconds(2f);
            PanelActionAnimator.SetTrigger("HidePanel");
        }
    }

    void EnableActionPanel()
    {
        ActionPanel.SetActive(true);
    }

    public void OnClickSkip()
    {
        UnitsManager.Instance.TurnBase();
    }
}
