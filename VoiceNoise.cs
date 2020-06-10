using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VoiceNoise : MonoBehaviour
{
    private Material _material;
    [SerializeField]
    private AudioSource audioSource;
    float[] _WaveData = new float[128000];//16秒分の波形データ（音源サンプルレート8,000Hz）
    float _maxVolume; //音量の最大値
    float[] _curWaveData = new float[1024];
    float _noiseIntensityByVolume;

    // Start is called before the first frame update
    void Start()
    {
        _material = transform.GetChild(1).GetComponent<Renderer>().material;
        audioSource.clip.GetData(_WaveData, 0); //曲0秒〜16秒の波形データ
        _maxVolume = _WaveData.Select(x => x * x).Max();
    }

    // Update is called once per frame
    void Update()
    {
        //音源から現在の音量取得
        //GetOutputData()で音源の波形データを配列個数分取得。（周波数成分に分けていない総合的な波形データ）
        audioSource.GetOutputData(_curWaveData, 0);
        var volume = _curWaveData.Select(x => x * x).Sum();
        _noiseIntensityByVolume = volume / _maxVolume * 0.2f;
        _material.SetFloat("_NoiseAmount", _noiseIntensityByVolume);
    }
}
