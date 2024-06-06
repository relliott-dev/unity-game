using UnityEngine;
using System.Collections;

namespace K2Games
{
    public class SetToZero : MonoBehaviour
    {
        #region Methods
        void Start()
        {
            transform.localPosition = Vector3.zero;
        }
        #endregion
    }
}
