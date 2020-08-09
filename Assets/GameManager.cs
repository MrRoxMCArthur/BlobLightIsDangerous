using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerController m_PlayerController;
    [SerializeField] private GameObject m_PlayerObject;
    [SerializeField] private GameObject m_Camera;
    [SerializeField] private GameObject m_GameOverImage;
    [SerializeField] private GameObject m_TheEndImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayerObject == null)
        {
            ((CameraFollow)m_Camera.GetComponent<CameraFollow>()).target = null;
            if (Input.GetButton("Jump"))
                SceneManager.LoadScene(0);
        }
        if (m_PlayerObject != null && m_PlayerObject.transform.localScale.x < 0.2f)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Destroy(m_PlayerObject);
        m_PlayerObject = null;
        m_GameOverImage.SetActive(true);
    }

    public void TheEnd()
    {
        Destroy(m_PlayerObject);
        m_PlayerObject = null;
        m_TheEndImage.SetActive(true);
    }
}
