using UnityEngine;
using UnityEngine.UI;
using Wit.BaiduAip.Speech;

public class AsrDemo : MonoBehaviour
{
    public string APIKey = "";
    public string SecretKey = "";
    public UnityEngine.UI.Button StartButton;
    public UnityEngine.UI.Button StopButton;
    public Text DescriptionText;

    private AudioClip _clipRecord;
    private Asr _asr;

    // Microphone is not supported in Webgl
#if !UNITY_WEBGL

    void Start()
    {
        _asr = new Asr(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());

        StartButton.gameObject.SetActive(true);
        StopButton.gameObject.SetActive(false);
        DescriptionText.text = DescriptionText.text.Replace(" a ", " a");

        StartButton.onClick.AddListener(OnClickStartButton);
        StopButton.onClick.AddListener(OnClickStopButton);
    }

    private void OnClickStartButton()
    {
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(true);
        DescriptionText.text = "正在录音...";

        _clipRecord = Microphone.Start(null, false, 30, 16000);
    }

    private void OnClickStopButton()
    {
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        DescriptionText.text = "等待返回...";
        Microphone.End(null);
        Debug.Log("[WitBaiduAip demo]end record");
        var data = Asr.ConvertAudioClipToPCM16(_clipRecord);
        StartCoroutine(_asr.Recognize(data, s =>
        {
            DescriptionText.text = s.result != null && s.result.Length > 0 ? s.result[0] : "未识别到声音";

            StartButton.gameObject.SetActive(true);
        }));
    }
#endif
}