using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PlayerSkill
{
    public float[] colTime;
    public int[] mp;
}



public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Player player;

    public PlayerSkill[] nearSkill;
    public PlayerSkill[] farSkill;
    public PlayerSkill[] magicSkill;

    public int[] nearSkillLevel = new int[4] { 1, 1, 1, 1 };
    public int[] farSkillLevel = new int[4] { 1, 1, 1, 1 };
    public int[] magicSkillLevel = new int[4] { 1, 1, 1, 1 };

    public float[] currentNearSkillColTime;
    public float[] currentFarSkillColTime;
    public float[] currentEnemySkillColTime;

    public bool[] useNearSkill;
    public bool[] useFarSkill;
    public bool[] useEnemySkill;

    public Image[] nearSkillImgae;
    public Image[] farSkillImgae;
    public Image[] magicSkillImgae;

    public Image[] rockNearSkillImage;
    public Image[] rockFarSkillImage;
    public Image[] rockMagicSkillImage;

    public bool[] rockNearSkill = new bool[4] { true, true, true, true };
    public bool[] rockFarSkill = new bool[] { true, true, true, true };
    public bool[] rockMagicSkill = new bool[] { true, true, true, true };

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            Player player;
            if (playerObj.TryGetComponent<Player>(out player))
            {
                if (player.root)
                {
                    this.player = player;
                }
            }
        }
        Skill();
        for (int i = 0; i < rockNearSkillImage.Length; i++)
        {
            if (!rockNearSkill[i])
            {
                rockNearSkillImage[i].gameObject.SetActive(false);
            }
            if (!rockFarSkill[i])
            {
                rockFarSkillImage[i].gameObject.SetActive(false);
            }
            if (!rockMagicSkill[i])
            {
                rockMagicSkillImage[i].gameObject.SetActive(false);
            }
        }
    }

    public void Skill()
    {
        switch (player.stats)
        {
            case playerStats.near:
                player.NearSkill();
                break;
            case playerStats.far:
                break;
            case playerStats.magic:
                break;
        }


        for (int i = 0; i < useNearSkill.Length; i++)
        {
            if (useNearSkill[i])
            {
                currentNearSkillColTime[i] += Time.deltaTime;
                nearSkillImgae[i].fillAmount = 1-(currentNearSkillColTime[i] / nearSkill[i].colTime[nearSkillLevel[i]]);
                if (currentNearSkillColTime[i] > nearSkill[i].colTime[nearSkillLevel[i]])
                {
                    currentNearSkillColTime[i] = 0;
                    useNearSkill[i] = false;
                }
            }
            else
            {
                nearSkillImgae[i].fillAmount = 0;
            }
        }

    }

    
}
