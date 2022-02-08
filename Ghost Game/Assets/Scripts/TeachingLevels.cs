using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeachingLevels : MonoBehaviour
{
    [TextArea] public string[] MyString;
    public Text MyText;
    [SerializeField] GameObject textPanel;
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

}
