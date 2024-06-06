using UnityEngine;
using System.Collections;

public class DebugObject : MonoBehaviour
{
    public static DebugObject Instance;

    void Start()
    {
        Instance = this;
    }

    public static void MoveTo(Vector3 position)
    {
        if(Instance != null)
            Instance.transform.position = position;
    }
}
