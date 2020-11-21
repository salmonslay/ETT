using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class CardObject : MonoBehaviour
{
    public string dest = "NONE";
    private Transform destObj;
    private float speed = 1;
    public bool moveInStack = false;

    //Stack offsets
    private Vector3 stackPos;
    private Vector3 stackSize;
    private Quaternion stackRotation;

    public bool menuFlip = false;

    //Card props
    public Card Card;

    private void Start()
    {
        stackPos = new Vector3(Random.Range(0.3f, -0.3f), 15, Random.Range(0.3f, -0.3f));
        stackSize = new Vector3(-4.178819f, -6.052824f, 0.01f);
        stackRotation = Quaternion.Euler(90, Random.Range(-30, 0), 0);
    }

    private void Update()
    {
        if (dest == "NONE") return;
        else if (dest == "STACK")
        {
            speed = Random.Range(1.5f, 2.5f);
            transform.position = Vector3.MoveTowards(transform.position, stackPos, speed * 11.7f * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, stackRotation, speed * 1.6f * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, stackSize, speed * 4 * Time.deltaTime);
            if (transform.position == stackPos)
            {
                transform.rotation = stackRotation;
                transform.localScale = stackSize;
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                gameObject.GetComponent<Rigidbody>().mass = 1.5f;
                Destroy(GetComponent<Rigidbody>(), 7);
                dest = "NONE";
            }
        }
        else if(dest == "LEFT")
        {   
            if (SceneManager.GetActiveScene().name == "main" && transform.position.x < -44.56) transform.position = new Vector3(52.35f, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 10f, destObj.position.y, destObj.position.z), 2f * Time.deltaTime);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-180, menuFlip ? 0 : -180, -180), 2 * Time.deltaTime);
            
        }
        else //move to card pos
        {
            if(GameObject.Find(dest)) destObj = GameObject.Find(dest).transform;
            

            transform.position = Vector3.MoveTowards(transform.position, destObj.position, speed * 11.7f * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, destObj.rotation, speed * (SceneManager.GetActiveScene().name == "main" ? 0.6f : 1.6f) * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(destObj.lossyScale.x, destObj.lossyScale.x * 1.4494f, 0.0001f), speed * 4 * Time.deltaTime);
            if (transform.position == destObj.position) speed = 15;
            else if (moveInStack) speed = 0.35f;
            else speed = Random.Range(0.9f, 1.1f);
        }
    }
}