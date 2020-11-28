using UnityEngine;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetInt("volume", 1);
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (AudioListener.volume == 1)
            {
                AudioListener.volume = 0;
                PlayerPrefs.SetInt("volume", 0);
            }
            else
            {
                AudioListener.volume = 1;
                PlayerPrefs.SetInt("volume", 1);
            }
        }
    }
}