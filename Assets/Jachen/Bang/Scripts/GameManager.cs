using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Bang
{
    public class GameManager : MonoBehaviour
    {
        //CardDatabase Deck = new CardDatabase();
        //private GameServer Server;
        //private GameClient Client;
        //GameState gameState;
        public Transform globalBlack;
        public CameraManager cameraManager;
        public string gameVersion = "0.0.0";
        public bool gameIsOnFocus = true;
        public static GameManager Instance { get; private set; }
        private void OnApplicationFocus(bool hasFocus)
        {
            this.gameIsOnFocus = hasFocus;
        }
        private void Awake()
        {
            if ((UnityEngine.Object)GameManager.Instance == (UnityEngine.Object)null)
                GameManager.Instance = this;
            else if ((UnityEngine.Object)GameManager.Instance != (UnityEngine.Object)this)
                UnityEngine.Object.Destroy((UnityEngine.Object)this.gameObject);

            this.globalBlack.gameObject.SetActive(true);
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US", false);
        }
        private void Start()
        {
            //Application.targetFrameRate = Globals.Instance.NormalFPS;
            this.gameVersion = ((TextAsset)UnityEngine.Resources.Load("runtime-version")).text;
            this.SetCamera();
            //this.Resize();
            //Globals.Instance.CreateGameContent();
        }
        public void StartGame()
        {
            Debug.Log("Game started");

            //Server = new GameServer();
            //gameState = new GameState();



            //Table setup
            //Create Game Server

            //Create Game Clients

        }
        public void SetCamera()
        {
            Camera[] allCameras = Camera.allCameras;

            for (int index = 0; index < allCameras.Length; ++index)
            {
                if (allCameras[index].gameObject.tag != "MainCamera")
                {
                    allCameras[index].gameObject.SetActive(false);
                }
                else
                {
                    this.cameraManager = allCameras[index].GetComponent<CameraManager>();
                    //this.cameraMain = allCameras[index];
                }
            }
        }

        public void SendServerEventToClients(/* event */)
        {

        }

        public void SendClientEventToServer(/* event */)
        {

        }
        public void ResetGameState()
        {

        }
       

    }
}