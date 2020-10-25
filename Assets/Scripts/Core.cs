using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Core : MonoBehaviour
{
    public Card[] Cards;
    public static Core Instance;

    private void Start()
    {
        Stopwatch st = new Stopwatch();
        st.Start();
        Instance = this;
        foreach(Card Card in Cards)
        {
            Card.InitCard((GameObject)Instantiate(Resources.Load($"Prefabs/Card")));
            Card.Object.transform.position = new Vector3(UnityEngine.Random.Range(0, 100), UnityEngine.Random.Range(0, 100), UnityEngine.Random.Range(0, 100));
        }
        st.Stop();
        Debug.Log($"Core/Start: Cards initiated. Took {st.ElapsedMilliseconds}ms to execute.");
    }
}

