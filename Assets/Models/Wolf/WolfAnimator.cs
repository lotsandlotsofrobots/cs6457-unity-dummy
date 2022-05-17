using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimator : MonoBehaviour
{
    Animator animator;

    public bool Walking { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Walking", false);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Walking", Walking);
    }
}
