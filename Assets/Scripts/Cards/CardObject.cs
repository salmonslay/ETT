using UnityEngine;

public class CardObject : MonoBehaviour
{
    public string dest;
    private Transform destObj;
    private float speed = 1;

    //Card props
    public Card Card;

    private void Start()
    {
    }

    private void Update()
    {
        if (destObj == null && !GameObject.Find(dest)) return; 
        else destObj = GameObject.Find(dest).transform;

        transform.rotation = Quaternion.Slerp(transform.rotation, destObj.rotation, speed * 1.6f * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, destObj.position, speed * 11.7f * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(destObj.lossyScale.x, destObj.lossyScale.x* 1.4494f, 0.0001f), speed * 4 * Time.deltaTime);
        if (transform.position == destObj.position) speed = 15;
        else speed = UnityEngine.Random.Range(0.9f, 1.1f);
    }
}