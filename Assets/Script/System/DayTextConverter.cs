﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL
{
    public class DayTextConverter
    {
        // 1 日     60 秒
        // 1 時間   6 秒
        // 10 分    1 秒

        private static float SecondToDay = 20;                              //60
        private static float MinutesToDay = SecondToDay / 10.0f;            //6
        private static int SecondToMinutes = (int)(60.0 / MinutesToDay);   //10

        public static DateTime Convert(float time) {
            DateTime startDate = new DateTime(2020, 4, 1, 0, 0, 0);
            DateTime currentDate = startDate + ConvertSpan(time);

            return currentDate;
        }

        public static TimeSpan ConvertSpan(float time) {
            DateTime startDate = new DateTime(2020, 4, 1, 0, 0, 0);

            int elapsedDay = (int)(time / SecondToDay);                 // (int)( 113 / 30 ) = 3日

            float timeInDay = time - elapsedDay * SecondToDay;          // 113 - 3 * 30 = 23 
            int elapsedHour = (int)(timeInDay / MinutesToDay);          // (int)( 23 / 3 ) = 7 時間

            float timeInHour = timeInDay - elapsedHour * MinutesToDay;  // 23 - 3 * 7 = 2
            int elapsedMinutes = (int)(timeInHour * SecondToMinutes);  // (int)( 2 * 20 ) = 40 分


            int finalDay = elapsedDay;
            int finalHour = elapsedHour + 10;
            int finalMinutes = (int)(elapsedMinutes / SecondToMinutes) * SecondToMinutes;

            TimeSpan span = new TimeSpan(finalDay, finalHour, finalMinutes, 0);
            return span;
        }

        public static string ConvertString(float time) {
            var date = Convert(time);
            return string.Format("{0,2}月{1,2}日 {2,2}時{3,2}分", date.Month, date.Day, date.Hour, date.Minute);
        }

        public static float OneDayProgress (float time) {
            int elapsedDay = (int)(time / SecondToDay);                 // (int)( 113 / 30 ) = 3日
            return (time - elapsedDay * SecondToDay) / SecondToDay;
        }
    }   
}