using System.Collections.Generic;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// A ScriptableObject that defines the structure for storing patch notes data
    /// It includes details such as version, date, and categorized lists of updates
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "Patch Note Data", menuName = "Patch Notes/Patch Note Entry", order = 1)]
    public class PatchNoteData : ScriptableObject
    {
        public string patchVersion;
        public int patchYear;
        public int patchMonth;
        public int patchDay;
        public List<string> generalUpdates;
        public List<string> newFeatures;
        public List<string> bugFixes;
        public List<string> knownIssues;
    }
}