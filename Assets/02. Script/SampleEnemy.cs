using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemy : MonoBehaviour
{
    private float direction = 1;
    public float speed = 3f;

    private void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        transform.Translate(new Vector2(direction * speed * Time.deltaTime, 0));
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            direction *= -1;
        }
    }
}