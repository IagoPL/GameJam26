using UnityEngine;

public class S_HitEffect : MonoBehaviour
{
    
    public bool isCritical = false; 
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (anim == null) return;

        if (isCritical)
            anim.Play("HitCritical"); 
        else
            anim.Play("HitNormal"); 
    }

}
