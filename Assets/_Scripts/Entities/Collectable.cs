using Shadout.Controllers;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ContenderBase contender = other.GetComponent<ContenderBase>();
        if (contender != null)
        {
            Collect(contender);
        }
    }

    protected virtual void Collect(ContenderBase contender)
    {
        gameObject.SetActive(false);
    }
}
