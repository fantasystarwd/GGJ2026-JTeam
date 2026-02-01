using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _sceneBGM; // 拖入此場景要播放的音樂檔
    [SerializeField] private bool _loop = true;

    private void Start()
    {
        // 確保場景中已有 AudioManager 實例後再進行引用
        if (AudioManager.Instance != null)
        {
            if (_sceneBGM != null)
            {
                AudioManager.Instance.PlayMusic(_sceneBGM, _loop);
            }
            else
            {
                Debug.LogWarning("AudioPlayer: 尚未在 Inspector 中指定 _sceneBGM。");
            }
        }
        else
        {
            Debug.LogError("AudioPlayer: 找不到 AudioManager Instance！請確認初始場景中已產生 AudioManager。");
        }
    }

    public void ChangeSceneMusic(AudioClip newClip)
    {
        if (AudioManager.Instance != null && newClip != null)
        {
            AudioManager.Instance.PlayMusic(newClip, _loop);
        }
    }
}
