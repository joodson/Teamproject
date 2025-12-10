using UnityEngine;

public class ForceTestAnim : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            anim.Play("Soccer Tackle");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            anim.Play("Goalkeeper Placing Ball");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            anim.Play("Scissor Kick");

        if (Input.GetKeyDown(KeyCode.Alpha4))
            anim.Play("Jog Forward");
    }
}
