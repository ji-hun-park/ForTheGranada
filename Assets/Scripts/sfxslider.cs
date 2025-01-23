using UnityEngine;
using UnityEngine.UI;

public class sfxslider : MonoBehaviour
{
    // 특정 부모 아래에서 "Check"라는 이름의 자식을 검색
    Transform parent;
    Transform child;
    public Image check;
    public Slider sfxsld;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform;
        child = FindChildRecursive(parent, "Check");
        if (child != null) check = child.GetComponent<Image>();
        sfxsld = GetComponent<Slider>();
        sfxsld.value = audiomanager.Instance.sfxvolume;
        ChangeVolume(sfxsld.value);
    }

    // Update is called once per frame
    void Update()
    {
        if (!audiomanager.Instance.isMute[2]) ChangeVolume(sfxsld.value);
        else { ChangeVolume(0.001f); sfxsld.value = 0.001f; }
    }

    public void Mute()
    {
        sfxsld.value = 0.001f;
        audiomanager.Instance.SetAudioMute(EAudioMixerType.SFX);
        check.gameObject.SetActive(!check.gameObject.activeSelf);
    }

    public void ChangeVolume(float volume)
    {
        audiomanager.Instance.SetAudioVolume(EAudioMixerType.SFX, volume);
        audiomanager.Instance.sfxvolume = volume;
    }

    Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            // 재귀 호출로 자식의 자식도 탐색
            Transform found = FindChildRecursive(child, childName);
            if (found != null)
                return found;
        }
        return null; // 찾지 못한 경우 null 반환
    }
}
