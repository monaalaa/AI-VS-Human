using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Animator PanelActionAnimator;
    [SerializeField]
    Text Message;

    [SerializeField]
    GameObject SkipPanel;

    [SerializeField]
    GameObject GameOverPanel;

    [SerializeField]
    Text winText;
    private string textToNotify;

    private void Start()
    {
        UnitsManager.ReadyToMakeAction += EnableActionPanel;
        UnitsManager.ActionHappned += Notification;
        UnitsManager.GameOver += GameOver;
    }
    public void DisableActionPanel()
    {
        SkipPanel.SetActive(false);
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
        DisableActionPanel();
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
        SkipPanel.SetActive(true);
    }
    private void OnDestroy()
    {
        UnitsManager.ReadyToMakeAction -= EnableActionPanel;
        UnitsManager.ActionHappned -= Notification;
        UnitsManager.GameOver -= GameOver;
    }
}
