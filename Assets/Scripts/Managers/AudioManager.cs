using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer mixer;
    public AudioSource SFXSource;   //音效播放    
    public AudioMixerGroup SFXMixerGroup;


    [System.Serializable]
    #region Sound类储存单个音频信息
    public class Sound
    {

        [Header("音频素材")]
        public AudioClip clip;
        [Header("音频Channel")]
        public AudioMixerGroup channel;
        [Header("是否为音效")]
        public bool isSFX;                       //不是音效则是音乐
        [Header("是否开局播放")]
        public bool PlayonAwake;
        public Sound(AudioClip audio_clip, bool is_SFX, bool playon_awake = false)
        {
            clip = audio_clip;
            isSFX = is_SFX;
            //if (is_SFX)
            //{
            //    channel = Mixer.outputAudioMixerGroup.
            //}
            PlayonAwake = playon_awake;
        }
    }
    #endregion

    #region Sounds列表
    public List<Sound> sounds;     // 可以手动添加，也可以自动添加
    #endregion

    private Dictionary<string, Sound> audioDic;

    #region Unity函数
    public Dictionary<string, int> SFXLimter = new Dictionary<string, int>();  // 音效限制器 防止同一时间播放过多音效
    override protected void Awake()
    {
        base.Awake();
        audioDic = new Dictionary<string, Sound>();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        foreach (var sound in sounds)         //将列表加入字典
        {
            if (!sound.isSFX)                //若不是音效则为音乐 为其添加GameObject audiosource及audiomixer
            {

                GameObject obj = new GameObject(sound.clip.name);
                obj.transform.SetParent(gameObject.transform);
                AudioSource source = obj.AddComponent<AudioSource>();
                source.clip = sound.clip;
                source.outputAudioMixerGroup = sound.channel;
                source.playOnAwake = sound.PlayonAwake;
                source.loop = true;
                audioDic.Add(sound.clip.name, sound);
            }
            else
            {
                audioDic.Add(sound.clip.name, sound);
            }
        }
        PlayAudio("无意义");
    }
    private void FixedUpdate()
    {
        RefreshSFXLimter();
    }

    #endregion

    #region 播放音频
    // 禁止一下子播放很多个音效
    private int refreshSFXLimterFrame = 5;  
    private int refreshSFXLimterFramecounter = 0;
    public void RefreshSFXLimter()
    {
        refreshSFXLimterFramecounter += 1;
        if (refreshSFXLimterFramecounter > refreshSFXLimterFrame)
        {
            refreshSFXLimterFramecounter = 0;
        }
        else
        {
            return;
        }
        var keys = new List<string>();
        foreach (var item in SFXLimter.Keys)
        {
            keys.Add(item.ToString());
        }
        foreach (var item in keys)
        {
            if (SFXLimter[item] > 0)
            {
                SFXLimter[item] -= 1;
            }
        }
    }
    public void PlayAudio(string name, float delay = 0)   //播放
    {
        StartCoroutine(_PlayAudio(name, delay));
    }

    private string backupAudioPath = "Audio/Sound/";
    IEnumerator _PlayAudio(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!audioDic.ContainsKey(name))
        {
            try
            {
                var clip = Resources.Load<AudioClip>(backupAudioPath + name);
                var s = new Sound(clip, true);
                s.channel = SFXMixerGroup;
                audioDic[name] = s;
                Logger.Log("【加载音频】 " + name);
            }
            catch (System.Exception)
            {
                Logger.Warning("【加载音频失败】 " + name);
            }
        }
        if (audioDic[name].isSFX)
        {
            // 禁止短时间播放同一个音效三次
            if (!SFXLimter.ContainsKey(name))
            {
                SFXLimter[name] = 0;
            }
            if (SFXLimter[name] < 2)
            {
                Logger.Debug("【播放音频】 " + name);
                SFXSource.outputAudioMixerGroup = audioDic[name].channel;
                SFXSource.playOnAwake = audioDic[name].PlayonAwake;
                SFXSource.PlayOneShot(audioDic[name].clip);
                SFXLimter[name] += 1;
            }
        }
        else
        {
            GameObject.Find(audioDic[name].clip.name).GetComponent<AudioSource>().Play();
        }
    }

    #endregion

    #region 停止与暂停音频
    public void StopAudio(string name, float delay = 0)   //暂停
    {
        StartCoroutine(_StopAudio(name, delay));
    }
    IEnumerator _StopAudio(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!audioDic.ContainsKey(name))
        {
            Debug.Log($"名为{name}音频不存在");
        }
        else if (audioDic[name].isSFX)
        {
            SFXSource.clip = audioDic[name].clip;
            SFXSource.outputAudioMixerGroup = audioDic[name].channel;
            SFXSource.Stop();
        }
        else
        {
            GameObject.Find(audioDic[name].clip.name).GetComponent<AudioSource>().Stop();
        }
    }
    public void PauseAudio(string name, float delay = 0)   //暂停
    {
        StartCoroutine(_PauseAudio(name, delay));
    }
    IEnumerator _PauseAudio(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!audioDic.ContainsKey(name))
        {
            Debug.Log($"名为{name}音频不存在");
        }
        else if (audioDic[name].isSFX)
        {
            SFXSource.clip = audioDic[name].clip;
            SFXSource.outputAudioMixerGroup = audioDic[name].channel;
            SFXSource.Pause();
        }
        else
        {
            GameObject.Find(audioDic[name].clip.name).GetComponent<AudioSource>().Pause();
        }
    }
    #endregion

    #region 设置音量
    public void BGMVolumeDown(string name, float value)
    {
        StartCoroutine(Down(name, value));
    }
    IEnumerator Down(string name,float value)
    {
        if (!audioDic.ContainsKey(name))
        {
            Debug.Log($"名为{name}音频不存在");
            
        }
        else
        {
            var v = 0f;
            mixer.GetFloat("BGMvolume", out v);
            if (audioDic[name].isSFX)
            {
                mixer.SetFloat("BGMvolume", v + value);
                yield return new WaitForSeconds(audioDic[name].clip.length);
                mixer.SetFloat("BGMvolume", v);
            }
            else
            {
                mixer.SetFloat("BGMvolume", value + v);

            }
            
        }
    }

    public void SetBGMValue(float v)
    {
        float trueV = Mathf.Pow(v, 0.1f) * 72 - 80;
        AudioManager.Instance.mixer.SetFloat("BGMvolume", trueV);
    }
    public void SetSFXValue(float v)
    {
        float trueV = Mathf.Pow(v, 0.1f) * 77 - 80;
        AudioManager.Instance.mixer.SetFloat("SFXvolume", trueV);
    }
    public float GetBGMValue()
    {
        var v = 0f;
        mixer.GetFloat("BGMvolume", out v);
        float outV = Mathf.Pow((v + 80) / 72, -0.1f);
        return outV;
    }
    public float GetSFXValue()
    {
        var v = 0f;
        mixer.GetFloat("SFXvolume", out v);
        float outV = Mathf.Pow((v + 80) / 77, -0.1f);
        return outV;
    }
    #endregion




}
