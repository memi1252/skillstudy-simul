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
                GameManager.Instance.messageUI.Add("�������", Color.blue, true);
            }
            else
            {
                GameManager.Instance.messageUI.Add("�������", Color.blue, true);
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
            GameManager.Instance.messageUI.Add("��� ĳ������ ���ݷ� 100 ����", Color.blue, true);
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
            GameManager.Instance.messageUI.Add("��� ĳ������ ü�� ȸ��", Color.blue, true);
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
                            GameManager.Instance.messageUI.Add("����: ü�� +20, ���ݷ� +10", Color.green, true);
                            if (player.level % 2 == 1)
                            {
                                SkillManager.instance.nearSkillUpgrade++;
                                GameManager.Instance.messageUI.Add("����: ctrl + ��ųŰ�� ���� ��ų ������", Color.green, true);
                            }
                            break;
                        case playerStats.far:
                            player.maxHp += 15;
                            player.hp += player.maxHp / 4;
                            player.attackDamage += 8;
                            GameManager.Instance.messageUI.Add("��ó: ü�� +15, ���ݷ� +8", Color.green, true);
                            if (player.level % 2 == 1)
                            {
                                SkillManager.instance.farSkillUpgrade++;
                                GameManager.Instance.messageUI.Add("��ó: ctrl + ��ųŰ�� ���� ��ų ������", Color.green, true);
                            }
                            break;
                        case playerStats.magic:
                            player.maxHp += 10;
                            player.hp += player.maxHp / 4;
                            player.attackDamage += 10;
                            GameManager.Instance.messageUI.Add("������: ü�� +10, ���ݷ� +10", Color.green, true);
                            if (player.level % 2 == 1)
                            {
                                SkillManager.instance.magicSkillUpgrade++;
                                GameManager.Instance.messageUI.Add("������: ctrl + ��ųŰ�� ���� ��ų ������", Color.green, true);
                            }
                            break;
                    }
                }
            }
            GameManager.Instance.messageUI.Add("��� ĳ������ 1������", Color.blue, true);
        }else if(Input.GetKeyDown(KeyCode.F5))
        {
            GameManager.Instance.BossSpecialSkill1.Use();
            GameManager.Instance.messageUI.Add("ȭ�� ������ ��� ����", Color.blue, true);
        }
    }
}
