using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool IsCombo = animator.GetBool("IsCombo");
        bool comboKey = Input.GetKey("c");
        bool IsAttack = animator.GetBool("IsAttack");
        bool AttackKey = Input.GetKey("a");
        // if player presses "a"
        if (!IsAttack && AttackKey)
        {
            // then set the IsAttack boolean to be true
            animator.SetBool("IsAttack", true);
        }

        // if player does not press "c"
        if (IsAttack && !AttackKey)
        {
            // then set the IsCombo boolean to be false
            animator.SetBool("IsAttack", false);
        }

        if (!IsCombo && comboKey)
        {
            // then set the IsCombo boolean to be true
            animator.SetBool("IsCombo", true);
        }

        // if player does not press "c"
        if (IsCombo && !comboKey)
        {
            // then set the IsCombo boolean to be false
            animator.SetBool("IsCombo", false);
        }
;
    }
}
