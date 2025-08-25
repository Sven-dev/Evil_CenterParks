using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GuestSystem
{
    [System.Serializable]
    public class Date
    {
        [SerializeField] private int Day;
        [SerializeField] private int Month;
        [SerializeField] private int Year;

        /// <summary>
        /// Generates a random date between 1970-01-01 and 2005-12-31.
        /// </summary>
        public static Date Random()
        {
            int year = UnityEngine.Random.Range(1970, 2006);
            int month = UnityEngine.Random.Range(1, 13);

            int day;
            //February (28 days)
            if (month == 2)
            {
                day = UnityEngine.Random.Range(1, 29);
            }
            //30-day months (4, 6, 9, 11)
            else if (month == 4 || month == 6 || month == 9 || month == 11) 
            {
                day = UnityEngine.Random.Range(1, 31);
            }
            //31-day months (1, 3, 5, 7, 8, 10, 12)
            else
            {
                day = UnityEngine.Random.Range(1, 32);                
            }

            return new Date(year, month, day);
        }

        /// <summary>
        /// Generates a random date between 1970-01-01 and 2005-12-31.
        /// </summary>
        /// <param name="minimum">The date the generated date has to be bigger than.</param>
        /// <returns></returns>
        public static Date Random(Date minimum)
        {
            Date newDate = Random();
            while (newDate.Get() < minimum.Get())
            {
                newDate = Random();
            }

            return newDate;
        }

        public Date(int year, int month, int day)
        {
            Day = day;
            Month = month;
            Year = year;
        }

        public DateTime Get()
        {
            return new DateTime(Year, Month, Day);
        }

        public override string ToString()
        {
            return Day.ToString("D2") + "/" + Month.ToString("D2") + "/" + Year.ToString("D4");
        }
    }
}