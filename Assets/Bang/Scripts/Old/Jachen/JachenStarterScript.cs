using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JachenGame.GameSystem;

/// <summary>
/// This class is connected to Unity. 
/// </summary>
public class JachenStarterScript : MonoBehaviour
{
    private FakeGameManager m_fakeGameManager;

    // Called at the very beginning of the game.
    public void Awake()
    {
        m_fakeGameManager = new FakeGameManager();
    }

    // Called every frame.
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            m_fakeGameManager.StartGame();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.LogWarning("You just pressed S");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.LogError("You just pressed D");
        }
    }
}
