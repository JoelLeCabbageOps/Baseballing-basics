using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject youWinPanel, youLosePanel;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            youWinPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
