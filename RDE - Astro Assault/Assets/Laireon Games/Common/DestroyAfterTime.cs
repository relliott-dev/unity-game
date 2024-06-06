using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{
    public float time;

    void Start()
    {
        StartCoroutine(DestroyMe());
    }

    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
