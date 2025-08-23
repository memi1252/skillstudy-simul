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
    public float[] currentMagicSkillColTime;

    public bool[] useNearSkill;
    public bool[] useFarSkill;
    public bool[] useMagicSkill;

    public Image[] nearSkillImgae;
    public Image[] farSkillImgae;
    public Image[] magicSkillImgae;

    public Image[] rockNearSkillImage;
    public Image[] rockFarSkillImage;
    public Image[] rockMagicSkillImage;

    public bool[] rockNearSkill = new bool[4] { true, true, true, true };
    public bool[] rockFarSkill = new bool[] { true, true, true, true };
    public bool[] rockMagicSkill = new bool[] { true, true, true, true };

    public int nearSkillUpgrade;
    public int farSkillUpgrade;
    public int magicSkillUpgrade;

    public Button[] nearSkillUpgradeButton;
    public Button[] farSkillUpgradeButton;
    public Button[] magicSkillUpgradeButton;

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

        if (nearSkillUpgrade > 0)
        {
            for (int i = 0; i < nearSkillLevel.Length; i++)
            {
                if (nearSkillLevel[i] < 5)
                {
                    nearSkillUpgradeButton[i].gameObject.SetActive(true);
                }
                else
                {
                    nearSkillUpgradeButton[i].gameObject.SetActive(false);
                }
            }

        }
        else
        {
            for(int i = 0;i < nearSkillUpgradeButton.Length; i++)
            {
                nearSkillUpgradeButton[i].gameObject.SetActive(false);
            }
        }

        if (farSkillUpgrade > 0)
        {
            for (int i = 0; i < farSkillLevel.Length; i++)
            {
                if (farSkillLevel[i] < 5)
                {
                    farSkillUpgradeButton[i].gameObject.SetActive(true);
                }
                else
                {
                    farSkillUpgradeButton[i].gameObject.SetActive(false);
                }
            }

        }
        else
        {
            for (int i = 0; i < farSkillUpgradeButton.Length; i++)
            {
                farSkillUpgradeButton[i].gameObject.SetActive(false);
            }
        }

        if (magicSkillUpgrade > 0)
        {
            for (int i = 0; i < magicSkillLevel.Length; i++)
            {
                if (magicSkillLevel[i] < 5)
                {
                    magicSkillUpgradeButton[i].gameObject.SetActive(true);
                }
                else
                {
                    magicSkillUpgradeButton[i].gameObject.SetActive(false);
                }
            }

        }
        else
        {
            for (int i = 0; i < magicSkillUpgradeButton.Length; i++)
            {
                magicSkillUpgradeButton[i].gameObject.SetActive(false);
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

    public void SkillUpgrade(int count)
    {
        switch (player.stats)
        {
            case playerStats.near:
                nearSkillLevel[count-1]++;
                nearSkillUpgrade--;
                break;
            case playerStats.far:
                farSkillLevel[count-1]++;
                farSkillUpgrade--;
                break;
            case playerStats.magic: 
                magicSkillLevel[count-1]++;
                magicSkillUpgrade--;
                break;
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
                player.FarSkill();
                break;
            case playerStats.magic:
                player.MagicSkill();
                break;
        }


        for (int i = 0; i < useNearSkill.Length; i++)
        {
            if (useNearSkill[i])
            {
                currentNearSkillColTime[i] += Time.deltaTime;
                nearSkillImgae[i].fillAmount = 1-(currentNearSkillColTime[i] / nearSkill[i].colTime[nearSkillLevel[i]-1]);
                if (currentNearSkillColTime[i] > nearSkill[i].colTime[nearSkillLevel[i]-1])
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

        for (int i = 0; i < useFarSkill.Length; i++)
        {
            if (useFarSkill[i])
            {
                currentFarSkillColTime[i] += Time.deltaTime;
                farSkillImgae[i].fillAmount = 1 - (currentFarSkillColTime[i] / farSkill[i].colTime[farSkillLevel[i] - 1]);
                if (currentFarSkillColTime[i] > farSkill[i].colTime[farSkillLevel[i] - 1])
                {
                    currentFarSkillColTime[i] = 0;
                    useFarSkill[i] = false;
                }
            }
            else
            {
                farSkillImgae[i].fillAmount = 0;
            }
        }

        for (int i = 0; i < useMagicSkill.Length; i++)
        {
            if (useMagicSkill[i])
            {
                currentMagicSkillColTime[i] += Time.deltaTime;
                magicSkillImgae[i].fillAmount = 1 - (currentMagicSkillColTime[i] / magicSkill[i].colTime[magicSkillLevel[i] - 1]);
                if (currentMagicSkillColTime[i] > magicSkill[i].colTime[magicSkillLevel[i] - 1])
                {
                    currentMagicSkillColTime[i] = 0;
                    useMagicSkill[i] = false;
                }
            }
            else
            {
                magicSkillImgae[i].fillAmount = 0;
            }
        }

    }

    
}
