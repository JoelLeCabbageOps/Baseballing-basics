using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject youWinPanel, youLosePanel, inGameCanvas;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inGameCanvas.SetActive(true);
            youWinPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
