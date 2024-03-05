using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class StartGame : MonoBehaviour
    {
        private static GameManager m_gameManager;
        public GameObject Table;
        public GameObject Card;
        //public GameObject Player;
        public void Start()
        {
            m_gameManager = GameManager.Instance;
            //Instantiate(Table, new Vector3(0,0,0), Quaternion.identity);
            //Instantiate(Card, new Vector3(0, .1f, 0), Quaternion.Euler(90,0,0)).GetComponent<MeshRenderer>();
            //Instantiate(Card, new Vector3(0, .1f, 0), Quaternion.Euler(90, 0, 0));
            //Instantiate(Card, new Vector3(0, .1f, 0), Quaternion.Euler(90, 0, 0));
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(m_gameManager != null)
                {
                    m_gameManager.StartGame();
                }
                else
                {
                    Debug.LogWarning("Gamemanager null");
                }

            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.LogWarning("Escape pressed");
            }
        }
    }

}
