using UnityEngine;
using Random = UnityEngine.Random;

public class CardObject : MonoBehaviour
{
    public string dest = "NONE";
    private Transform destObj;
    private float speed = 1;

    //Stack offsets
    private Vector3 stackPos;
    private Vector3 stackSize;
    private Quaternion stackRotation;

    //Card props
    public Card Card;

    private void Start()
    {
        stackPos = new Vector3(Random.Range(0.3f, -0.3f), 15, Random.Range(0.3f, -0.3f));
        stackSize = new Vector3(-4.178819f, -6.052824f, 0.15f);
        stackRotation = Quaternion.Euler(90, Random.Range(-30, 0), 0);
    }

    private void Update()
    {
        if (dest == "NONE") return;
        if (dest == "STACK")
        {
            speed = Random.Range(1.5f, 2.5f);
            transform.position = Vector3.MoveTowards(transform.position, stackPos, speed * 11.7f * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, stackRotation, speed * 1.6f * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, stackSize, speed * 4 * Time.deltaTime);
            if (transform.position == stackPos)
            {
                gameObject.tag = "InStack";
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                Destroy(GetComponent<Rigidbody>(), 10);
                dest = "NONE";
            }
        }
        else
        {
            destObj = GameObject.Find(dest).transform;

            transform.position = Vector3.MoveTowards(transform.position, destObj.position, speed * 11.7f * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, destObj.rotation, speed * 1.6f * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(destObj.lossyScale.x, destObj.lossyScale.x * 1.4494f, 0.0001f), speed * 4 * Time.deltaTime);
            Debug.Log(destObj.rotation);
            if (transform.position == destObj.position) speed = 15;
            else speed = Random.Range(0.9f, 1.1f);
        }
    }
}