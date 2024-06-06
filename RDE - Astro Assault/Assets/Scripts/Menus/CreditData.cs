using System.Collections.Generic;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// A ScriptableObject that stores data for credit entries. Each entry contains a title and a list of names associated with that title
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "Credit Data", menuName = "Credits/Credit Entry", order = 1)]
    public class CreditData : ScriptableObject
    {
        public string title;
        public List<string> names;
    }
}