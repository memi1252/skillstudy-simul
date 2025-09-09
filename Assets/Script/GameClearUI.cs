using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearUI : MonoBehaviour
{
    public Text playTimeText;
    public Text stageText;

    public Button restartButton;
    public Button mainButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        mainButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
        playTimeText.text = $"{(int)(GameManager.Instance.currentTime / 60):D2}:{(int)(GameManager.Instance?.currentTime % 60):D2}";
        stageText.text = $"스테이지 {GameManager.Instance.stage}";
        bool rankUpdate = false;
        if (RankManager.instance.data.Count == 0)
        {
            RankManager.instance.Load();
        }
        foreach (var rank in RankManager.instance.data)
        {
            if (rank.score <= GameManager.Instance.score)
            {
                rankUpdate = true;
                break;
            }
        }

        if (rankUpdate)
        {
            RankManager.instance.rankAddUI.Show();
        }
    }

    


}
