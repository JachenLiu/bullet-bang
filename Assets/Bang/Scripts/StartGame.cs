using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang;

public class StartGame : MonoBehaviour
{
    private static GameManager m_gameManager;
    public GameObject Table;
    public GameObject Card;
    public GameObject Player;
    public void Awake()
    {
        m_gameManager = new GameManager();
        Instantiate(Table, new Vector3(0,0,0), Quaternion.identity);
        Instantiate(Card, new Vector3(0, .1f, 0), Quaternion.Euler(90,0,0)).GetComponent<MeshRenderer>();
        Instantiate(Card, new Vector3(0, .1f, 0), Quaternion.Euler(90, 0, 0));
        Instantiate(Card, new Vector3(0, .1f, 0), Quaternion.Euler(90, 0, 0));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_gameManager.StartGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.LogWarning("Escape pressed");
        }
    }
}
