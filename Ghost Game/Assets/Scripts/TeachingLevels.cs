using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class TeachingLevels : MonoBehaviour
{
    [TextArea] public string[] MyString;
    public Text MyText;
    [SerializeField] GameObject textPanel;
    [SerializeField] CinemachineFreeLook cam2;
    [SerializeField] CinemachineFreeLook cam3;
    [SerializeField] GameObject spotLight;
    [SerializeField] int camChangedNumber;
    bool endTyping;
    bool continuePressed;
    int y = 0;

    private void Start()
    {
        endTyping = true;
        FindObjectOfType<GameManager>().isDialog = true;

    }
    void Update()
    {
        Typing();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            continuePressed = true;
        }
        CameraSwap();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            textPanel.SetActive(false);
            FindObjectOfType<GameManager>().isDialog = false;

        }
    }
    IEnumerator Type(string MyString)
    {
        endTyping = false;
        MyText.text = "";
        for (int j = 0; j < MyString.Length; j++)
        {
            char c = MyString[j];
            MyText.text += c;
            yield return new WaitForSeconds(.05f);
        }
        endTyping = true;
    }
    void Typing()
    {    
        if (continuePressed)
        {
            continuePressed = false;
            if(y < MyString.Length && endTyping)
            {              
                StartCoroutine(Type(MyString[y]));
                y++;
            }
            if(y == MyString.Length && endTyping)
            {
                textPanel.gameObject.SetActive(false);
                FindObjectOfType<GameManager>().isDialog = false;
            }
        }
    }
    void CameraSwap()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (y == camChangedNumber )
            {
                cam2.Priority = 12;
            }
            if (y == camChangedNumber + 1)
            {
                cam3.Priority = 12;
                cam2.Priority = 9;
            }
            if (y == camChangedNumber + 2)
            {
                cam3.Priority = 9;
            }
        }
        else
        {
            if (y == camChangedNumber)
            {
                if (spotLight != null)
                    spotLight.SetActive(true);
                cam2.Priority = 11;
            }
            if (y == camChangedNumber + 1)
            {
                if (spotLight != null)
                    spotLight.SetActive(false);
                cam2.Priority = 9;
            }

        }

    }

}
