using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    GameObject TutorialPanel;
    [SerializeField]
    List<Sprite> tutorialSprites = new List<Sprite>();
    [SerializeField]
    Image sprite;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("FirstTime"))
        {
            TutorialPanel.SetActive(true);
            PlayerPrefs.SetInt("FirstTime", 1);
        }
    }

    public void OnClickNext()
    {
        count++;
        if (count < tutorialSprites.Count)
        {
            sprite.sprite = tutorialSprites[count];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
