using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject Enemy;
    public int count;
    public bool stepOn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        { stepOn = true; print("playerin"); }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        { stepOn = true; print("playerin"); }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        { stepOn = false; print("playerout"); }
    }

    private void Start()
    {
        StartCoroutine(SpawnCheck());
    }

    public void summon()
    {
        GameObject copy = Instantiate(Enemy, transform.position, transform.rotation);
        copy.GetComponent<AIBase>().portal = this;
        copy.GetComponent<AIBase>().SetAct();
    }

    IEnumerator SpawnCheck()
    {
        if (count < 3)
        {
            summon();
            count++;
        }
        yield return new WaitForSeconds(10.0f);
        StartCoroutine(SpawnCheck());
    }

    
}
