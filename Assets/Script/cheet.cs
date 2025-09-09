using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class cheet : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameManager.Instance.got = !GameManager.Instance.got;
            if (GameManager.Instance.got)
            {
                GameManager.Instance.messageUI.Add("무적모드", Color.blue, true);
            }
            else
            {
                GameManager.Instance.messageUI.Add("무적모드", Color.blue, true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            foreach (var player in GameManager.Instance.players)
            {
                if (player != null)
                {
                    player.attackDamage += 100;
                }
            }
            GameManager.Instance.messageUI.Add("모든 캐릭터의 공격력 100 증가", Color.blue, true);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            foreach (var player in GameManager.Instance.players)
            {
                if (player != null)
                {
                    player.hp = player.maxHp;
                }
            }
            GameManager.Instance.messageUI.Add("모든 캐릭터의 체력 회복", Color.blue, true);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            foreach (var player in GameManager.Instance.players)
            {
                if (player != null)
                {
                    switch (player.stats)
                    {
                        case playerStats.near:
                            player.maxHp += 20;
                            player.hp += player.maxHp / 4;
                            player.attackDamage += 10;
                            GameManager.Instance.messageUI.Add("전사: 체력 +20, 공격력 +10", Color.green, true);
                            if (player.level % 2 == 1)
                            {
                                SkillManager.instance.nearSkillUpgrade++;
                                GameManager.Instance.messageUI.Add("전사: ctrl + 스킬키를 눌러 스킬 레벨업", Color.green, true);
                            }
                            break;
                        case playerStats.far:
                            player.maxHp += 15;
                            player.hp += player.maxHp / 4;
                            player.attackDamage += 8;
                            GameManager.Instance.messageUI.Add("아처: 체력 +15, 공격력 +8", Color.green, true);
                            if (player.level % 2 == 1)
                            {
                                SkillManager.instance.farSkillUpgrade++;
                                GameManager.Instance.messageUI.Add("아처: ctrl + 스킬키를 눌러 스킬 레벨업", Color.green, true);
                            }
                            break;
                        case playerStats.magic:
                            player.maxHp += 10;
                            player.hp += player.maxHp / 4;
                            player.attackDamage += 10;
                            GameManager.Instance.messageUI.Add("마법사: 체력 +10, 공격력 +10", Color.green, true);
                            if (player.level % 2 == 1)
                            {
                                SkillManager.instance.magicSkillUpgrade++;
                                GameManager.Instance.messageUI.Add("마법사: ctrl + 스킬키를 눌러 스킬 레벨업", Color.green, true);
                            }
                            break;
                    }
                }
            }
            GameManager.Instance.messageUI.Add("모든 캐릭터의 1레벨업", Color.blue, true);
        }else if(Input.GetKeyDown(KeyCode.F5))
        {
            GameManager.Instance.BossSpecialSkill1.Use();
            GameManager.Instance.messageUI.Add("화면 가리기 기술 시전", Color.blue, true)
        }
    }
}
