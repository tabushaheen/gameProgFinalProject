using UnityEngine;

public class EnemyAnimatorFix : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();

        // Hard reset everything that can "freeze" animation
        Time.timeScale = 1f;

        if (anim != null)
        {
            anim.enabled = true;
            anim.speed = 1f;

            // Ensure it updates with normal game time
            anim.updateMode = AnimatorUpdateMode.Normal;
            anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;

            // If you were messing with physics-driven anim, disable it:
            anim.applyRootMotion = false;

            Debug.Log($"[EnemyAnimatorFix] OK on {name} | speed={anim.speed} updateMode={anim.updateMode} cull={anim.cullingMode}");
        }
        else
        {
            Debug.LogWarning($"[EnemyAnimatorFix] No Animator on {name}");
        }
    }
}
