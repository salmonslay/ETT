using UnityEngine;

public class CardPhysics : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(GetComponent<Rigidbody>());
    }
}