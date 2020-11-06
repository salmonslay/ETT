using UnityEngine;

public class PlayBoard : MonoBehaviour
{
    private static Color current;
    private static Color Default = new Color(0.5283019f, 0.03737985f, 0.3395049f);
    
    private static Color Blue = new Color(0.03921568f, 0.3722454f, 0.5372549f);
    private static Color Green = new Color(0.03921568f, 0.5372549f, 0.04602172f);
    private static Color Red = new Color(0.5377358f, 0.03804734f, 0.08242156f);
    private static Color Yellow = new Color(0.879f, 0.7629532f, 0.01868992f);

    private static Color[] Colors = new Color[] { Blue, Green, Red, Yellow };

    public static void SetColor(CardProperties.Color c)
    {
        current = Colors[(int)c];
    }
    public static void ResetColor()
    {
        current = Default;
    }
    private void Start()
    {
        ResetColor();
    }
    private void Update()
    {
        if(GetComponent<Renderer>().material.color != current)
        GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, current, Mathf.PingPong(Time.time, 12));
    }
}