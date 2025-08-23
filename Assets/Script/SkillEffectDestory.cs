using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectDestory : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}
