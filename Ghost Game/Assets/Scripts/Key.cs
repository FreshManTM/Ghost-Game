using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    [SerializeField] GameObject canvasButton;
    [SerializeField] AudioSource pickUpSound;
    [SerializeField] Image keyImage;
    bool inRange;

    private void Start()
    {
            keyImage.gameObject.SetActive(true);

    }
    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.F))
        {
            pickUpSound.Play();
            FindObjectOfType<GameManager>().hasKey = true;
            keyImage.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !FindObjectOfType<Invisibility>().invisible)
        {
            canvasButton.SetActive(true);
            inRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canvasButton.SetActive(false);
            inRange = false;

        }
    }
}
