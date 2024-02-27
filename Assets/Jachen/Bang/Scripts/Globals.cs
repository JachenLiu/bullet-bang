using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Globals : MonoBehaviour
{
    //[SerializeField]
    //private Dictionary<string, CardData> _Cards;

    public static Globals Instance { get; private set; }
    private void Awake()
    {
        if ((UnityEngine.Object)Globals.Instance == (UnityEngine.Object)this)
            Globals.Instance = this;
        else if ((UnityEngine.Object)Globals.Instance != (UnityEngine.Object)this)
            UnityEngine.Object.Destroy((UnityEngine.Object)this.gameObject);
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object)this.gameObject);

    }
}
