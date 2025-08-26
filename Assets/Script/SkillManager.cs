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
    public GameObject[] isSkillLevelUpImage;

    public int[] passiveSkillLevel = new int[5];
    public int passiveSkillPoint = 0;

    public Text passiveSkillText;
    public GameObject passiveSKillUpImage;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
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

        passiveSkillText.text = $"공용패시브 스킬 : 크리티컬확률증가 Lv.{passiveSkillLevel[0]}, 공격속도증가 Lv.{passiveSkillLevel[1]}, 체력증가 Lv.{passiveSkillLevel[2]}," +
            $" 이동속도증가 Lv.{passiveSkillLevel[3]}, 공격력증가 Lv.{passiveSkillLevel[4]}";


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
            isSkillLevelUpImage[0].SetActive(true);

        }
        else
        {
            for (int i = 0; i < nearSkillUpgradeButton.Length; i++)
            {
                nearSkillUpgradeButton[i].gameObject.SetActive(false);
            }
            isSkillLevelUpImage[0].SetActive(false);
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
            isSkillLevelUpImage[1].SetActive(true);

        }
        else
        {
            for (int i = 0; i < farSkillUpgradeButton.Length; i++)
            {
                farSkillUpgradeButton[i].gameObject.SetActive(false);
            }
            isSkillLevelUpImage[1].SetActive(false);
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
            isSkillLevelUpImage[2].SetActive(true);

        }
        else
        {
            for (int i = 0; i < magicSkillUpgradeButton.Length; i++)
            {
                magicSkillUpgradeButton[i].gameObject.SetActive(false);
            }
            isSkillLevelUpImage[2].SetActive(false);
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

        if(passiveSkillPoint > 0)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (passiveSkillLevel[0] == 5) return;
                passiveSkillLevel[0]++;
                passiveSkillPoint--;
                foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    Player player;
                    if (playerObj.TryGetComponent<Player>(out player))
                    {
                        switch (passiveSkillLevel[0])
                        {
                            case 1:
                                player.criticalDamagePersent += player.criticalDamagePersent * 0.1f;
                                break;
                            case 2:
                                player.criticalDamagePersent += player.criticalDamagePersent * 0.2f;
                                break;
                            case 3:
                                player.criticalDamagePersent += player.criticalDamagePersent * 0.3f;
                                break;
                            case 4:
                                player.criticalDamagePersent += player.criticalDamagePersent * 0.4f;
                                break;
                            case 5:
                                player.criticalDamagePersent += player.criticalDamagePersent * 0.5f;
                                break;
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (passiveSkillLevel[1] == 5) return;
                passiveSkillLevel[1]++;
                passiveSkillPoint--;
                foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    Player player;
                    if (playerObj.TryGetComponent<Player>(out player))
                    {
                        switch (passiveSkillLevel[1])
                        {
                            case 1:
                                player.attackSpeed = player.orizinAttackSpeed / (1f + 0.5f);
                                break;
                            case 2:
                                player.attackSpeed = player.orizinAttackSpeed / (1f + 1f);
                                break;
                            case 3:
                                player.attackSpeed = player.orizinAttackSpeed / (1f + 1.5f);
                                break;
                            case 4:
                                player.attackSpeed = player.orizinAttackSpeed / (1f + 2f);
                                break;
                            case 5:
                                player.attackSpeed = player.orizinAttackSpeed / (1f + 3f);
                                break;
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (passiveSkillLevel[2] == 5) return;
                passiveSkillLevel[2]++;
                passiveSkillPoint--;
                foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    Player player;
                    if (playerObj.TryGetComponent<Player>(out player))
                    {
                        switch (passiveSkillLevel[2])
                        {
                            case 1:
                                player.maxHp = player.bastMaxHp + 400;
                                player.hp += 400;
                                if (player.hp > player.maxHp)
                                    player.hp = player.maxHp;
                                break;
                            case 2:
                                player.maxHp = player.bastMaxHp + 400;
                                player.hp += 400;
                                if (player.hp > player.maxHp)
                                    player.hp = player.maxHp;
                                break;
                            case 3:
                                player.maxHp = player.bastMaxHp + 600;
                                player.hp += 600;
                                if (player.hp > player.maxHp)
                                    player.hp = player.maxHp;
                                break;
                            case 4:
                                player.maxHp = player.bastMaxHp + 800;
                                player.hp += 800;
                                if (player.hp > player.maxHp)
                                    player.hp = player.maxHp;
                                break;
                            case 5:
                                player.maxHp = player.bastMaxHp + 1000;
                                player.hp += 1000;
                                if (player.hp > player.maxHp)
                                    player.hp = player.maxHp;
                                break;
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (passiveSkillLevel[3] == 5) return;
                passiveSkillLevel[3]++;
                passiveSkillPoint--;
                foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    Player player;
                    if (playerObj.TryGetComponent<Player>(out player))
                    {
                        switch (passiveSkillLevel[3])
                        {
                            case 1:
                                player.speed = player.orizinSpeed * (1f + 0.5f);
                                break;
                            case 2:
                                player.speed = player.orizinSpeed * (1f + 1f);
                                break;
                            case 3:
                                player.speed = player.orizinSpeed * (1f + 1.2f);
                                break;
                            case 4:
                                player.speed = player.orizinSpeed * (1f + 1.5f);
                                break;
                            case 5:
                                player.speed = player.orizinSpeed * (1f + 2f);
                                break;
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (passiveSkillLevel[4] == 5) return;
                passiveSkillLevel[4]++;
                passiveSkillPoint--;
                foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    Player player;
                    if (playerObj.TryGetComponent<Player>(out player))
                    {
                        switch (passiveSkillLevel[3])
                        {
                            case 1:
                                player.attackDamage = player.orizinAttackDamage * (1f + 0.2f);
                                break;
                            case 2:
                                player.attackDamage = player.orizinAttackDamage * (1f + 0.4f);
                                break;
                            case 3:
                                player.attackDamage = player.orizinAttackDamage * (1f + 0.6f);
                                break;
                            case 4:
                                player.attackDamage = player.orizinAttackDamage * (1f + 0.8f);
                                break;
                            case 5:
                                player.attackDamage = player.orizinAttackDamage * (1f + 1f);
                                break;
                        }
                    }
                }
            }
            passiveSKillUpImage.SetActive(true);
        }
        else
        {
            passiveSKillUpImage.SetActive(false);
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
