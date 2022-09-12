using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float destroyTime = 1.5f;

    float currntTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (currntTime > destroyTime)
        {
            Destroy(gameObject);
        }

        currntTime += Time.deltaTime;
    }
}
