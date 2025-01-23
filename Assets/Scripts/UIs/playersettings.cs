using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playersettings : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Close()
    {
        audiomanager.Instance.menusfx.Play();
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        // Update Mode를 Unscaled Time으로 변경
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        animator.ResetTrigger("Close");
        if (UIManager.Instance.UIList[0] != null) UIManager.Instance.UIList[0].gameObject.SetActive(false);
    }
}
