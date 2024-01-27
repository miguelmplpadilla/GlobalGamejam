using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public Animator animator;
    bool destroying;

    void Update()
    {
        if (!destroying)
        {
            if (Input.anyKey)
            {
                Invoke(nameof(DestroyObj), 2f);
                animator.SetTrigger("Fade");
            }
        }
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

    
}
