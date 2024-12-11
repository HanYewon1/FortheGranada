using UnityEngine;
using UnityEngine.UI;

public class allslider : MonoBehaviour
{
    // 특정 부모 아래에서 "Check"라는 이름의 자식을 검색
    Transform parent;
    Transform child;
    public Image check;
    public Slider allsld;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform;
        child = FindChildRecursive(parent, "Check");
        if (child != null) check = child.GetComponent<Image>();
        allsld = GetComponent<Slider>();
    }

    private void OnEnabled()
    {
        if (allsld != null) allsld.value = audiomanager.Instance.mstvolume;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeVolume(allsld.value);
    }

    public void Mute()
    {
        audiomanager.Instance.SetAudioMute(EAudioMixerType.Master);
        check.gameObject.SetActive(!check.gameObject.activeSelf);
    }

    public void ChangeVolume(float volume)
    {
        audiomanager.Instance.SetAudioVolume(EAudioMixerType.Master, volume);
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
