using UnityEngine;
using UnityEngine.Audio;

public enum EAudioMixerType { Master, BGM, SFX }

public class audiomanager : MonoBehaviour
{
    public float mstvolume;
    public float bgmvolume;
    public float sfxvolume;
    // 각 오디오 소스들
    public AudioSource mainmenubgm;
    public AudioSource ingamebgm;
    public AudioSource bossstagebgm;
    public AudioSource npcattack;
    public AudioSource menusfx;
    public AudioSource bossfire;

    public static audiomanager Instance;
    [SerializeField] private AudioMixer audioMixer;

    private bool[] isMute = new bool[3];
    private float[] audioVolumes = new float[3];
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetAudioVolume(EAudioMixerType audioMixerType, float volume)
    {
        // 오디오 믹서의 값은 -80 ~ 0까지이기 때문에 0.0001 ~ 1의 Log10 * 20을 한다.
        audioMixer.SetFloat(audioMixerType.ToString(), Mathf.Log10(volume) * 20);
        // 볼륨 연동용
        switch (audioMixerType.ToString())
        {
            case "Master":
                mstvolume = volume;
                break;
            case "BGM":
                bgmvolume = volume;
                break;
            case "SFX":
                sfxvolume = volume;
                break;
            default:
                Debug.LogError("Out of Vol");
                break;
        }
    }

    public void SetAudioMute(EAudioMixerType audioMixerType)
    {
        int type = (int)audioMixerType;
        if (!isMute[type]) // 뮤트 
        {
            isMute[type] = true;
            audioMixer.GetFloat(audioMixerType.ToString(), out float curVolume);
            audioVolumes[type] = curVolume;
            SetAudioVolume(audioMixerType, 0.001f);
        }
        else
        {
            isMute[type] = false;
            SetAudioVolume(audioMixerType, audioVolumes[type]);
        }
    }

    public void PlayMenuSFX()
    {
        menusfx.Play();
    }

    public void StopMainMenuBGM()
    {
        mainmenubgm.Stop();
    }
}
