using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField]
    Animator PanelActionAnimator;
    [SerializeField]
    Text Message;

    [SerializeField]
    GameObject ActionPanel;

    [SerializeField]
    GameObject GameOverPanel;

    [SerializeField]
    Text winText;
    private string textToNotify;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        UnitsManager.ReadyToMakeAction += EnableActionPanel;
        UnitsManager.ActionHappned += Notification;
        UnitsManager.GameOver += GameOver;
    }
    public void DisableActionPanel()
    {
        ActionPanel.SetActive(false);
    }
    public void OnClickSkip()
    {
        UnitsManager.Instance.TurnBase();
    }
    public void OnClickReplay()
    {
        SceneManager.LoadScene(0);
    }
    public void OnClickClose()
    { Application.Quit(); }
    void Notification(string action)
    {
        textToNotify = action;
        ShowPanel();
    }
    void GameOver(UnitType type)
    {
        GameOverPanel.SetActive(true);
        if (type == UnitType.Player)
        { winText.text = "Pirats Win the Game "; }
        else
        { winText.text = "Winner Winner checken dinner"; }
    }
    void ShowPanel()
    {
        Message.text = textToNotify;
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
    private void OnDestroy()
    {
        UnitsManager.ReadyToMakeAction -= EnableActionPanel;
        UnitsManager.ActionHappned -= Notification;
        UnitsManager.GameOver -= GameOver;
    }
}
