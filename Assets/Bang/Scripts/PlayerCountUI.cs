using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace Bang
{
    public class PlayerCountUI : MonoBehaviour
    {
        public GameObject textInputPrefab;
        public GameObject playerCountPanelPrefab;
        public Material tableMaterial;

        private void Start()
        {
           if(GameManager.Instance != null)
            {
                //SetCanvasMaterial();
                GameObject canvas = Instantiate(textInputPrefab, GameManager.Instance.transform);
                canvas.transform.position = new Vector3(0f, 0f, 0f);
                canvas.transform.rotation = Quaternion.identity;
                canvas.transform.localScale = Vector3.one;

                // Instantiate the player count panel as a child of the canvas
                GameObject playerCountPanel = Instantiate(playerCountPanelPrefab, canvas.transform);

                // Get the input field component from the player count panel
                InputField playerCountInput = playerCountPanel.GetComponentInChildren<InputField>();

                // Set up the listener for the input field
                //playerCountInput.onEndEdit.AddListener(delegate { OnPlayerCountEntered(playerCountInput.text); });
            }
        }

        //public void OnPlayerCountEntered(string playerCountText)
        //{
        //    int playerCount;
        //    if (int.TryParse(playerCountText, out playerCount))
        //    {
        //        GameManager.Instance.SetPlayerCount(playerCount);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Invalid player count input!");
        //    }
        //}
        //void SetCanvasMaterial()
        //{
        //    CanvasRenderer canvasRenderer = GetComponent<CanvasRenderer>();
        //    if (canvasRenderer != null && tableMaterial != null)
        //    {
        //    }
        //}
    }
}