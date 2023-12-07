using System;
using System.Collections;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] float lifeTime = 1f;

    private void Start()
    {
        StartCoroutine(DelayCoroutine(lifeTime, () => Destroy(gameObject)));
    }

    private IEnumerator DelayCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
