
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;


public class ArtworkController : MonoBehaviour // DOM: try and use descriptive names, so the name should always try and describe what ever its for as accurately as possible.
{
    public NodeController[] controllerScript;
    public GameObject myPrefab;
    public int width;
    private bool lights;
    public int height;
    public Color nodeColor;
    private Quaternion rot;
    private Quaternion rot2;
    private float x_offset = 2.0f;
    private float z_offset = 2.0f;
    public float testing;
    public float[] alpha;
    public int k = 0;
    public float speed;
    float val;
    int arrayLength;
    Vector3 scale;
    //Scripts

    //GameObjects
    public GameObject[] led;


    //Position
    public Vector3[] position;

    // Renderers
    public Renderer[] ledRenderer;


    //public Color testColor;
    public Color[] ledcolor;


    struct AlphaJob : IJobParallelFor    // DOM: try and use descriptive names, so the name should always try and describe what ever its for as accurately as possible. in this case i would call it AlphaJob or so.
    {                                  // also, in c# you should always try and name classes and structs beginning with a capital letter. so this should be called TheJob rather than theJob.

        public NativeArray<float> alphaValues;
        public float newAlphaValue;
        public float theTime;



        public void Execute(int i)
        {

            newAlphaValue = Mathf.Sin(2 * Mathf.PI / 2 * theTime) + 1 / 2;
            alphaValues[i] = newAlphaValue;


        }
    }


    void Start()
    {
       
        //testing = controllerScript.valueWhatever;
        speed = 2.0f;
        arrayLength = (width + 1) * (height + 1);
        controllerScript = new NodeController[arrayLength];
        nodeColor = new Color(1, 0, 0);


        float len = controllerScript.Length;
       

        //gameObjects
        led = new GameObject[arrayLength];

        //positions of Game Objects
        position = new Vector3[arrayLength];
        alpha = new float[arrayLength];
        rot2 = Quaternion.Euler(0, 0, 0);


        for (int j = 0; j < height - 1; j = j + 2)
        {


            for (int i = 0; i <= width; i++)
            {

                if ((i % 2 == 0) & (j % 4 == 0))
                {

                    if (k != 18)
                    {

                        controllerScript[k] = InstantiatePrefab(k, new Vector3(i * x_offset, 0, j * z_offset), -60, transform);

                        k = k + 1;

                    }

                    controllerScript[k] = InstantiatePrefab(k, new Vector3(((i - 1) + 0.5F) * x_offset, 0, (j + 1F) * z_offset), 0, transform);

                    k = k + 1;

                }

                if ((i % 2 == 0) & (j % 4 != 0))
                {
                    if (k != 111)
                    {

                        controllerScript[k] = InstantiatePrefab(k, new Vector3(i * x_offset * 1F, 0, j * z_offset), 60, transform);
                        k = k + 1;

                    }


                    if (j != height - 2)
                    {
                        controllerScript[k] = InstantiatePrefab(k, new Vector3(((i) + 0.5F) * x_offset, 0, (j + 1F) * z_offset), 0, transform);

                        k = k + 1;
                    }
                }


                if ((i % 2 != 0) & (j % 4 == 0))
                {
                    controllerScript[k] = InstantiatePrefab(k, new Vector3(i * x_offset * 1F, 0, j * z_offset), 60, transform);

                    k = k + 1;
                }


                if ((i % 2 != 0) & (j % 4 != 0))
                {
                    controllerScript[k] = InstantiatePrefab(k, new Vector3(i * x_offset * 1F, 0, j * z_offset), -60, transform);

                    k = k + 1;
                }

            }

        }



    }

    //
    private NodeController InstantiatePrefab(int id, Vector3 position, float angle, Transform parent)
    {

       
            GameObject led = Instantiate(myPrefab, position, Quaternion.Euler(0, angle, 0));
            led.AddComponent<NodeController>();
            led.name = "Led" + id;
            //Debug.Log("name" + led.name);
            led.transform.parent = parent;

            NodeController nodeController = led.GetComponent<NodeController>();
            nodeController.id = id + 1;

            nodeController.ledcolor = nodeColor;
           

            nodeController.SetChildren();

            return nodeController;
           

    }
    private void Update()
    {

        NativeArray<float> alphaValues = new NativeArray<float>(arrayLength, Allocator.TempJob); // DOM: since you're using this every frame, i would create this as a permanent buffer in the start and delete it again in the destroy method. less overhead.


        var theJob = new AlphaJob()
        {

            theTime = Time.time,
            alphaValues = alphaValues



        };

        JobHandle jobHandle = theJob.Schedule(arrayLength, 1); // DOM: again use alphaValues.Length
        jobHandle.Complete();


        for (int i = 0; i < controllerScript.Length; i++)
        {
            //this worked
            //controllerScript[i].SetLightValues(theJob.alphaValues[i], theJob.alphaValues[i], theJob.alphaValues[i]);
            controllerScript[i].SetLightValues(1f, 1f, 1f);
        }



        alphaValues.Dispose();
    }



}





