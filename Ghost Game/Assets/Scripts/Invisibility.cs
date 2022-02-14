using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Invisibility : MonoBehaviour
{
    [Header("Serialize Fields")]
    [SerializeField] Text textInvisCount;
    [SerializeField] Text timerInvisText;
    [SerializeField] SkinnedMeshRenderer material;
    [SerializeField] Image imageCooldown;
    [SerializeField] Image backImage;

    [SerializeField] Material mainMaterial;
    [SerializeField] Material invisMaterial;

    [Header("Variables")]
    [SerializeField] int invisTimes = 1;
    [SerializeField] float timeInvisible;
    [SerializeField] float timerInvis;
    [SerializeField] bool inCooldown;
    public bool invisible;

    void Start()
    {
        timerInvis = timeInvisible;  
    }

    void Update()
    {
        timerInvisText.text = timerInvis.ToString("F1");
        textInvisCount.text = invisTimes.ToString();

        if(SceneManager.GetActiveScene().buildIndex != 1 && !FindObjectOfType<GameManager>().gameIsOver)
            Invisible();
        
    }

    void Invisible()
    {
        if (!invisible && Input.GetKeyDown(KeyCode.E) && !inCooldown && invisTimes > 0)
        {
            timerInvisText.gameObject.SetActive(true);
            backImage.gameObject.SetActive(true);

            invisible = true;
            material.material = invisMaterial;

            inCooldown = true;
            invisTimes--;
        }
        if (invisible)
        {       

            if (timerInvis > 0)
            {
                timerInvis -= Time.deltaTime;
            }
            else
            {
                material.material = mainMaterial;

                timerInvisText.gameObject.SetActive(false);
                backImage.gameObject.SetActive(false);
                timerInvis = timeInvisible;

                inCooldown = false;

                invisible = false;   
            }

        }
        else
        {
            material.material = mainMaterial;
        }
        if (invisTimes == 0)
        {
            imageCooldown.gameObject.SetActive(true);
            textInvisCount.gameObject.SetActive(false);
        }
    }
   
}
