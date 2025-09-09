using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameOverReason
{
    allDie,
    stage1TimeOut,
    stage2TimeOut,
    stage3TimeOut,
}

public class GameOverUI : MonoBehaviour
{
    public Text reasonText;
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

    public void GameOver(GameOverReason reason)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.Instance.cameraMove = false;
        switch (reason)
        {
            case GameOverReason.allDie:
                reasonText.text = $"모든 플레이어가 사망하였습니다.";
                break;
            case GameOverReason.stage1TimeOut:
                reasonText.text = $"1스테이지를 시간내에 클리어하지 못했습니다.";
                break;
            case GameOverReason.stage2TimeOut:
                reasonText.text = $"2스테이지를 시간내에 클리어하지 못했습니다.";
                break;
            case GameOverReason.stage3TimeOut:
                reasonText.text = $"3스테이지를 시간내에 클리어하지 못했습니다.";
                break;
        }
        playTimeText.text = $"{(int)(GameManager.Instance.currentTime/60):D2}:{(int)(GameManager.Instance?.currentTime%60):D2}";
        stageText.text = $"스테이지 {GameManager.Instance.stage}";
        bool rankUpdate = false;
        if(RankManager.instance.data.Count ==0)
        {
            RankManager.instance.Load();
        }
        foreach (var rank in RankManager.instance.data)
        {
            if(rank.score <= GameManager.Instance.score)
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
