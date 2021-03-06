﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceAnimator : MonoBehaviour
{
    List<Animator> _animators;
    public float Interval = 0.07f;
    public float WaitEnd = 2f;

    void Start()
    {
        _animators = new List<Animator>(GetComponentsInChildren<Animator>());

        StartCoroutine(DoAnimation());
    }

    IEnumerator DoAnimation()
    {
        while (true)
        {
            foreach (var animator in _animators)
            {
                animator.SetTrigger("AnimationTrigger");
                yield return new WaitForSeconds(Interval);
            }
            yield return new WaitForSeconds(WaitEnd);
        }
    
    }
}