using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{

    public int id;
    public Color ledcolor;
    public Vector3 position;
    public Renderer[] childRenderer;




    public void SetChildren()
    {
        childRenderer = new Renderer[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.name = string.Format("ChildSide {0}_{1}", i + 1, id);
            childRenderer[i] = child.GetComponent<Renderer>();
            childRenderer[i].material.EnableKeyword("_EMISSION");
            //childRenderer[i].material.SetColor("_Color", Color.red);
        }
    }
    public void SetLightValues(float val1, float val2, float val3)
    {
        Color c = ledcolor;
        c.a = val1;
        //childRenderer[0].material.color = c;

        c = ledcolor;
        c.a = val2;
        //childRenderer[1].material.color = c;

        c = ledcolor;
        c.a = val3;
        //childRenderer[2].material.color = c;

        // here you set the light values to the actualy unity light sources, or the material, etc.
    }
}


