using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private CollectableType type;

    [SerializeField]
    private float speed;

    public CollectableType Type => type;

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            Collect();
        }
    }

    public void Collect()
    {
        gameObject.SetActive(false);
    }
}
