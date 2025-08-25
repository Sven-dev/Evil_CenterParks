using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuestSystem
{
    public class NameGenerator : MonoBehaviour
    {
        [SerializeField] private List<string> LastNames;

        public string GenerateName()
        {
            //First letter
            int rnd = Random.Range(65, 91);
            string name = ((char)rnd).ToString() + ". ";

            //Last name
            rnd = Random.Range(0, LastNames.Count);
            name += LastNames[rnd];

            return name;
        }
    }
}