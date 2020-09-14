using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public void Shot(float velocity, GameObjectManager gameObjectManager)
    {
        StartCoroutine(OnShot(velocity, gameObjectManager));
    }
    private IEnumerator OnShot(float velocity, GameObjectManager gameObjectManager)
    {
        float distance = Vector3.Distance(gameObjectManager.transform.position, transform.position);
        float shotTime = distance / velocity;
        float time = 0.0f;
        Vector3 startPosition = transform.position;

        while(gameObjectManager && time < shotTime)
        {
            time += Time.deltaTime * velocity;
            transform.position = Vector3.up + Vector3.Lerp(startPosition, gameObjectManager.transform.position, time / shotTime);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
