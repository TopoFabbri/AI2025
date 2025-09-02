using System;
using Input;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;

        private float seconds;
        
        private void ToggleTimer()
        {
            timeText.enabled = !timeText.enabled;
        }

        private void Update()
        {
            seconds += Time.deltaTime;
            
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            timeText.text = time.ToString("hh\\:mm");
        }

        private void OnEnable()
        {
            InputListener.ToggleTimer += ToggleTimer;
        }

        private void OnDisable()
        {
            InputListener.ToggleTimer -= ToggleTimer;
        }
    }
}