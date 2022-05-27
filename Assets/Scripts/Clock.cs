using System;
using System.Net;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts
{
    public class Clock : MonoBehaviour
    {
        [SerializeField] private bool _getTimeFromServers;
        [SerializeField] private string[] _ntpServers;

        /*[Range(0, 11)] [SerializeField]*/ private int hours;

        //[Range(0, 59)] [SerializeField] private int minutes;

        //[Range(0, 59)] [SerializeField] private int seconds;

        [SerializeField] private ClockHand _hourHand;
        [SerializeField] private ClockHand _minuteHand;
        [SerializeField] private ClockHand _secondHand;
        
        private DateTime dateTime;

        private float time;

        // private int Hours
        // {
        //     get => hours;
        //     set
        //     {
        //         hours = value;
        //         UpdateTime();
        //     }
        // }

        void Start()
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            if (_getTimeFromServers)
                UpdateTimeFromServer();
            else
                UpdateTimeFromLocale();

            hours = dateTime.Hour;
            time = Time.GetTimeInSeconds(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        private void UpdateTimeFromServer()
        {
            if (ServerTime.GetTime(_ntpServers, out dateTime))
                Debug.Log("Successfully getting time");
            else
                Debug.LogError("Unsuccessfully getting time");
        }

        private void UpdateTimeFromLocale()
        {
            dateTime = DateTime.Now;
        }

        private void Update()
        {
            time += Time.deltaTime;

            var seconds = time % 60;
            var minutes = time % Mathf.Pow(60, 2) / 60;
            var hours = time / Mathf.Pow(60, 2);

            _secondHand.RotateHand(seconds * 6);
            _minuteHand.RotateHand(minutes * 6);
            _hourHand.RotateHand(hours * 30);
            
            if(this.hours != Mathf.FloorToInt(hours))
                UpdateTime();
        }
    }
}