// UMD IMDM290 
// Instructor: Myungin Lee
// All the same Lerp but using audio

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioReactive : MonoBehaviour
{
    GameObject[] flowers;
    public Mesh flowerMesh;
    static int numFlowers = 10;
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1

    GameObject[] flowerShape;
    Vector3[] flowerShapeStart;
    Vector3[] flowerShapeEnd;
    public Mesh flowerShapeMesh;

    int numFlowerShape = 40;
    int petals = 24;
    float flowerRadius = 8f;

    //peak swirl effect
    GameObject[] swirl;
    int swirlObjs = 20;
    Vector3[] swirlStart;
    Vector3[] swirlEnd;
    float swirlTime = 0f;

    float t;
    float spectrum;
    float spectrum2;
    float timer;
    float FlowerScale = 0.125f;
    float random = 0f;
    float blue = 0.5f;
    float backCol;
    float part2 = 24;
    float part3 = 48;
    float part4 = 97;
    float part5 = 120;

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        flowers = new GameObject[numFlowers];
        initPos = new Vector3[numFlowers]; // Start positions
        startPosition = new Vector3[numFlowers];
        endPosition = new Vector3[numFlowers];
        // Define target positions. Start = random, End = heart 
        for (int i = 0; i < numFlowers; i++)
        {
            // Random start positions
            float r = 5f;
            startPosition[i] = new Vector3(r * random, r * random, r * random);

            // Circular end position
            r = 3f;
            endPosition[i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numFlowers), r * Mathf.Cos(i * 2 * Mathf.PI / numFlowers));
        }
        // Let there be spheres..
        for (int i = 0; i < numFlowers; i++)
        {
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            flowers[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Position
            initPos[i] = startPosition[i];
            flowers[i].transform.position = initPos[i];
            flowers[i].transform.localRotation = Quaternion.Euler(270f, 0f, 0f); ;
            flowers[i].transform.localScale = new Vector3(Random.Range(0.3f, 0.5f), Random.Range(0.3f, 0.5f), Random.Range(0.3f, 0.5f));
            // Color
            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = flowers[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numFlowers; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
            MeshFilter cubeMesh = flowers[i].GetComponent<MeshFilter>();
            cubeMesh.mesh = flowerMesh;
        }

        flowerShape = new GameObject[numFlowerShape];
        flowerShapeStart = new Vector3[numFlowerShape];
        flowerShapeEnd = new Vector3[numFlowerShape];

        for (int i = 0; i < numFlowerShape; i++)
        {
            float theta = i * 2 * Mathf.PI / numFlowerShape;

            float r = flowerRadius * Mathf.Sin(petals * theta);

            float x = r * Mathf.Cos(theta);
            float y = r * Mathf.Sin(theta);

            flowerShapeEnd[i] = new Vector3(x, y, 0f);
            flowerShapeStart[i] = new Vector3(0f, 0f, 20f);

            flowerShape[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flowerShape[i].transform.position = flowerShapeStart[i];
            flowerShape[i].transform.localRotation = Quaternion.Euler(270f, 0f, 0f); ;
            flowerShape[i].transform.localScale = Vector3.one * 0.05f;

            Renderer rend = flowerShape[i].GetComponent<Renderer>();
            rend.material.color = Color.magenta;
            MeshFilter flowerShapeFilter = flowerShape[i].GetComponent<MeshFilter>();
            flowerShapeFilter.mesh = flowerShapeMesh;
        }

        //swirl p3?
        swirl = new GameObject[swirlObjs];
        swirlStart = new Vector3[swirlObjs];
        swirlEnd = new Vector3[swirlObjs];

        float radiusS = 1f;
        float radiusL = 20f;

        for (int j = 0; j < swirlObjs; j++)
        {
            //change start pos to a circle in center of screen
            float angle = j * Mathf.PI * 2f / swirlObjs;

            swirlStart[j] = new Vector3(Mathf.Cos(angle) * radiusS, 
                                        Mathf.Sin(angle) * radiusS, 
                                        0f);

            // Circular end position
            swirlEnd[j] = new Vector3(Mathf.Cos(angle) * radiusL, 
                                    Mathf.Sin(angle) * radiusL, 
                                    0f);

            swirl[j] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            swirl[j].transform.position = swirlStart[j];
            swirl[j].transform.localScale = Vector3.zero; //hidden until triggered

            Renderer r = swirl[j].GetComponent<Renderer>();
            r.material.color = Color.cyan;
            //MeshFilter cubeMesh = swirl[j].GetComponent<MeshFilter>();
            //cubeMesh.mesh = flower;
        }
    }

    // Update is called once per frame
    void Update()
    {
        random = Random.Range(0, spectrum);
        timer += Time.deltaTime;

        if (timer <= part2)
        {
            backCol = blue;
            spectrum = AudioSpectrum.drum1;
            Debug.Log("part1");

        }
        else if (timer <= part3)
        {
            spectrum = AudioSpectrum.drum2;
            Debug.Log("part2");
        }
        else if (timer <= part4)
        {
            UpdateSwirl(AudioSpectrum.vocal3); //at peak, change to swirl pattern
            spectrum = AudioSpectrum.vocal3;
            //spectrum = AudioSpectrum.vocal3;
            Debug.Log("part3");
        }
        else if (timer <= part5)
        {
            //if (timer >= part3) UpdateSwirl(AudioSpectrum.vocal3); //at peak, change to swirl pattern
            backCol = 0f;
            spectrum = AudioSpectrum.overall4;
            Debug.Log("part4");
        }
        else
        {
            HideSwirl(); //hide swirl objs
        }

        spectrum = Mathf.Lerp(spectrum, spectrum, Time.deltaTime * .25f);
        

        float spectrum2 = AudioSpectrum.bass;
        spectrum2 = Mathf.Lerp(spectrum2, spectrum2, Time.deltaTime * 0.25f);
        // ***Here, we use audio Amplitude, where else do you want to use?
        // Measure Time 
        // Time.deltaTime = The interval in seconds from the last frame to the current one
        // but what if time flows according to the music's amplitude?
        //time += Time.deltaTime * AudioSpectrum.audioAmp;
        time += Time.deltaTime * spectrum;
        Debug.Log(spectrum);
        Debug.Log(spectrum2 + "Spec 2");
        // what to update over time?
        for (int i = 0; i < numFlowers; i++)
        {
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)

            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            t = i * 2 * Mathf.PI / numFlowers;
            flowers[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            float scale = FlowerScale + (spectrum);
            flowers[i].transform.localScale = new Vector3(scale, FlowerScale, scale);
            flowers[i].transform.Rotate(0f, spectrum, 0f);

            // Color Update over time
            Renderer cubeRenderer = flowers[i].GetComponent<Renderer>();
            float hue = (float)i / numFlowers; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(0.2f + (0.8f - 0.2f) * (Mathf.Clamp01(spectrum2)), 1f, 1f); // Full saturation and brightness
            cubeRenderer.material.color = color;
        }

        float flowerLerp = Mathf.Sin(time * 0.5f) * 0.5f + spectrum2;

        for (int i = 0; i < numFlowerShape; i++)
        {
            flowerShape[i].transform.position =
                Vector3.Lerp(flowerShapeStart[i], flowerShapeEnd[i], flowerLerp);

            float scale = FlowerScale + (spectrum2);
            flowerShape[i].transform.localScale = new Vector3(scale, FlowerScale, scale);

            flowerShape[i].transform.Rotate(0f, spectrum2, 0f);
        }

        Camera camera = Camera.main;
        camera.backgroundColor = Color.HSVToRGB(0.55f, 0.75f, 1f);
    }

    //manage pattern change
    void UpdateSwirl(float spectr)
    {
        swirlTime += Time.deltaTime * spectr;

        for (int i = 0; i < swirlObjs; i++)
        {
            float delay = i * 0.04f; //delay ripple travel
            float raw = (swirlTime-delay) % 1f;
            if (raw < 0) raw += 1f;

            float lerp = raw;

            swirl[i].transform.position = Vector3.Lerp(swirlStart[i], 
                                                    swirlEnd[i], 
                                                    lerp);

            //pulse obj scale
            float scale = 0.3f + spectr * 0.75f;
            swirl[i].transform.localScale = Vector3.one * scale;
        }
    }
    //hide the objects when not used
    void HideSwirl()
    {
        for (int i = 0; i < swirlObjs; i++)
        {
            swirl[i].transform.localScale = Vector3.Lerp(swirl[i].transform.localScale,
                                                            Vector3.zero,
                                                            Time.deltaTime * 5f);
        }
    }
}
