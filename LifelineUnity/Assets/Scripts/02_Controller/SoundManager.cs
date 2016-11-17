using UnityEngine;
public class SoundManager : MonoBehaviour
{
	//==============================================================================================
    // Fields
	
    public AudioClip m_leftAudio;	// 左侧发送消息声音
    public AudioClip m_rightAudio;	// 右侧发送消息声音
    private AudioSource m_audioSource;
	// private bool m_soundIsActive = true;

    //==============================================================================================
    // Methods
	
    void Awake()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
		// m_setSoundButton.onClick.AddListener(()=> SetSound());
		// m_soundButtonImage = m_setSoundButton.GetComponent<Image>();
		// m_soundButtonImage.sprite = m_soundButtonImgs[0];
    }
	/// <summary> 发送消息时，播放声音 </summary>
    public void PlayMusic(AudioClip clip)
    {
        m_audioSource.PlayOneShot(clip, 1.0f);
    }
	/// <summary> 关闭/打开声音 </summary>
	// private void SetSound()
	// {
	// 	if (m_soundIsActive)
	// 	{
	// 		m_audioSource.volume = 0.0f;
	// 		m_soundIsActive = false;
	// 		// m_soundButtonImage.sprite = m_soundButtonImgs[1];
	// 	}
	// 	else
	// 	{
	// 		m_audioSource.volume = 1.0f;
	// 		m_soundIsActive = true;
	// 		// m_soundButtonImage.sprite = m_soundButtonImgs[0];
	// 	}
	// }
}
