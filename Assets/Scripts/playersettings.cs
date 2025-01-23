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
        GameManager.Instance.ui_list[10].gameObject.SetActive(false);
    }
}
