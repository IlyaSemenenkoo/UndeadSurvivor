using System;
using TMPro;
using UnityEngine;

public class TimeBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField]private WaveManager _waveManager;

    private void OnEnable()
    {
        _waveManager.OnTimeChanged += SetTime;
    }

    private void SetTime(int time)
    {
        _timeText.text = time.ToString();
    }

    private void OnDisable()
    {
        _waveManager.OnTimeChanged -= SetTime;
    }
}
