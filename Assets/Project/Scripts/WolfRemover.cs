using System.Collections;
using UnityEngine;

public class WolfRemover : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(RemoveAfterDelay(1f));
    }

    private IEnumerator RemoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}