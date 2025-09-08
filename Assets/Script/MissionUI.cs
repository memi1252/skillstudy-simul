using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    public Text stageText;
    public Text misstionText;
    public Text timerText;

    public Stage stage1;
    public Stage stage2;
    public Stage stage3;

    private void Update()
    {
        if(stageText != null)
        {
            stageText.text = $"현재 스테이지 : {GameManager.Instance.stage}";
        }
        if(misstionText != null)
        {
            switch (GameManager.Instance.stage)
            {
                case 1:
                    if (stage1.bossSpanw)
                    {
                        misstionText.text = $"미션:\n식인식물 처치";
                    }
                    else
                    {
                        misstionText.text = $"미션:\n1단계 몬스터 처치 ({GameManager.Instance.Stage1Level1EnemyCount}/{stage1.level1EnemyMax})\n" +
                            $"2단계 몬스터 처치 ({GameManager.Instance.Stage1Level2EnemyCount}/{stage1.level2EnemyMax})";
                    }
                    timerText.text = $"남은 시간 : {(int)((stage1.timerMax-stage1.currentTimer)/60):D2}:{(int)((stage1.timerMax - stage1.currentTimer)%60):D2}";                        
                    break;
                case 2:
                    if (stage2.bossSpanw)
                    {
                        misstionText.text = $"미션:\n골램 처치";
                    }
                    else
                    {
                        misstionText.text = $"미션:\n2단계 몬스터 처치 ({GameManager.Instance.Stage2Level2EnemyCount}/{stage2.level2EnemyMax})\n" +
                            $"3단계 몬스터 처치 ({GameManager.Instance.Stage2Level3EnemyCount}/{stage2.level3EnemyMax})\n" +
                            $"오브젝트 파괴 ({GameManager.Instance.Stage2DestoyObjectCount}/{stage2.destroyObjctMax})";
                    }
                    timerText.text = $"남은 시간 : {(int)((stage2.timerMax - stage1.currentTimer) / 60):D2}:{(int)((stage2.timerMax - stage1.currentTimer) % 60):D2}";
                    break;
                case 3:
                    misstionText.text = $"미션:\n드래곤을 제한시간내에 처치";
                    timerText.text = $"남은 시간 : {(int)((stage3.timerMax - stage1.currentTimer) / 60):D2}:{(int)((stage3.timerMax - stage1.currentTimer) % 60):D2}";
                    break;
            }
        }
    }
}
