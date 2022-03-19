using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDie : MonoBehaviour
{
    public float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DieByTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DieByTime()
    {
        if (lifeTime > 0)
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }
    }
}
