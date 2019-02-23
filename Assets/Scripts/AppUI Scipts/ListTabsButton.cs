using UnityEngine;

public class ListTabsButton : MonoBehaviour
{
    private Animator buttonAnimator;

    void Start()
    {
        buttonAnimator = this.GetComponent<Animator>();
    }

    public void HoverButton()
    {
        if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("PP Pressed"))
        {
            // do nothing because it's clicked
        }

        else
        {
            buttonAnimator.Play("PP Hover");
        }
    }

    public void NormalizeButton()
    {
        if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("PP Pressed"))
        {
            // do nothing because it's clicked
        }

        else
        {
            buttonAnimator.Play("PP Pressed to Normal");
        }
    }
}
