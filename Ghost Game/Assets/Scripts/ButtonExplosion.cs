using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExplosion : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] LayerMask lm;

    [SerializeField] GameObject canvasButton;

    [SerializeField] bool inRangeOfButtonExplosion;

    [SerializeField] GameObject button;

    [SerializeField] Explosion explosion;
    void Start()
    {
        canvasButton.SetActive(false);
    }


    void Update()
    {        
        inRangeOfButtonExplosion = Physics.CheckSphere(transform.position, radius, lm);

        if (inRangeOfButtonExplosion)
        {
            if (Input.GetKeyDown(KeyCode.F) && explosion != null)
            {

                StartCoroutine(ButtonPressed());
            }

            if (explosion != null)
            {
                canvasButton.SetActive(true);
            }
            else
            {
                canvasButton.SetActive(false);
            }
        }
        else
        {
            canvasButton.SetActive(false);
        }

    }
    IEnumerator ButtonPressed()
    {
        Explosion old = explosion;
        explosion = null;
        button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - .5f, button.transform.position.z);
        old.Explode();
        yield return new WaitForSeconds(.5f);
        button.transform.position = new Vector3(button.transform.position.x + .1f, button.transform.position.y + .5f, button.transform.position.z);
        yield return new WaitForSeconds(4.5f);
        Destroy(old.gameObject);
    }

    private void OnDrawGizmos()
    {
        Color col = Color.red;
        col.a = .1f;
        Gizmos.color = col;
        Gizmos.DrawSphere(transform.position, radius);

    }
}
