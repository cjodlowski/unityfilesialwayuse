using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Color highlightColor;
    public GameObject outlines;
    private Color[] origColors;
    private MeshRenderer mr;
    private bool canInteract = false;
    public float reachDistance = 5f;
    private Transform tf;

    

    protected GameObject player;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mr = GetComponentInChildren<MeshRenderer>();
        Debug.Log(mr.materials);
        origColors = new Color[mr.materials.Length];
        for(int ii = 0; ii <mr.materials.Length; ++ii)
        {
            origColors[ii] = mr.materials[ii].color;
        }

        tf = GetComponent<Transform>();
        outlines.SetActive(false);
    }

    public void Update()
    {
        if(canInteract)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Interact();
                for (int ii = 0; ii < mr.materials.Length; ++ii)
                {
                    mr.materials[ii].color = origColors[ii];
                }

                outlines.SetActive(false);
            }
        }
        Debug.Log("########## Player: " + player);
        if(Vector3.Distance(tf.position, player.transform.position) > reachDistance)
        {
            for (int ii = 0; ii < mr.materials.Length; ++ii)
            {
                mr.materials[ii].color = origColors[ii];
            }
            outlines.SetActive(false);
        }

        

    }

    public void OnMouseEnter()
    {
        Debug.Log("Mouse Entered");

        if(Vector3.Distance(tf.position, player.transform.position) < reachDistance)
        {
            Debug.Log("Should be highlighted");
            for (int ii = 0; ii < mr.materials.Length; ++ii)
            {
                mr.materials[ii].color = highlightColor;
            }
            outlines.SetActive(true);
            canInteract = true;
        }

    }

    public void OnMouseExit()
    {
        Debug.Log("Mouse Exited");
        for (int ii = 0; ii < mr.materials.Length; ++ii)
        {
            mr.materials[ii].color = origColors[ii];
        }
        outlines.SetActive(false);
        canInteract = false;
    }
    public abstract void Interact();
}
