// UMD IMDM290 
// Instructor: Myungin Lee
// All the same Lerp but using audio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactive : MonoBehaviour
{
    GameObject[] spheres;
    public Mesh flower;
    static int numSphere = 10;
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;
    float spectrum;
    float spectrum2;
    float timer;
    float squareScale = 0.125f;
    float random = 0f;
    float green = 0.4f;
    float blue = 0.5f;
    float orange = 0.06f;
    float cubeCol;
    float backCol;
    float part2 = 24;
    float part3 = 48;
    float part4 = 97;
    float part5 = 120;

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[numSphere];
        endPosition = new Vector3[numSphere];
        // Define target positions. Start = random, End = heart 
        for (int i = 0; i < numSphere; i++)
        {
            // Random start positions
            float r = 5f;
            startPosition[i] = new Vector3(r * random, r * random, r * random);

            // Circular end position
            r = 3f;
            endPosition[i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numSphere), r * Mathf.Cos(i * 2 * Mathf.PI / numSphere));
        }
        // Let there be spheres..
        for (int i = 0; i < numSphere; i++)
        {
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Position
            initPos[i] = startPosition[i];
            spheres[i].transform.position = initPos[i];
            spheres[i].transform.localRotation = Quaternion.EulerAngles(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
            spheres[i].transform.localScale = new Vector3(Random.Range(0.3f, 0.5f), Random.Range(0.3f, 0.5f), Random.Range(0.3f, 0.5f));
            // Color
            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
            MeshFilter cubeMesh = spheres[i].GetComponent<MeshFilter>();
            cubeMesh.mesh = flower;
        }
    }

    // Update is called once per frame
    void Update()
    {
        random = Random.Range(0, spectrum);
        timer += Time.deltaTime;

        if (timer <= part2)
        {
            cubeCol = green;
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
            spectrum = AudioSpectrum.vocal3;
            Debug.Log("part3");
        }
        else if (timer <= part5)
        {
            backCol = orange;
            spectrum = AudioSpectrum.overall4;
            Debug.Log("part4");
        }

        float spectrum2 = AudioSpectrum.bass;
            // ***Here, we use audio Amplitude, where else do you want to use?
            // Measure Time 
            // Time.deltaTime = The interval in seconds from the last frame to the current one
            // but what if time flows according to the music's amplitude?
            //time += Time.deltaTime * AudioSpectrum.audioAmp;
            time += Time.deltaTime * spectrum;
        Debug.Log(spectrum);
        Debug.Log(spectrum2 + "Spec 2");
        // what to update over time?
        for (int i = 0; i < numSphere; i++)
        {
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)

            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            t = i * 2 * Mathf.PI / numSphere;
            spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            float scale = squareScale + (spectrum);
            spheres[i].transform.localScale = new Vector3(scale, squareScale, scale);
            spheres[i].transform.Rotate(spectrum, spectrum, spectrum);

            // Color Update over time
            Renderer cubeRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(cubeCol, spectrum, 1f); // Full saturation and brightness
            cubeRenderer.material.color = color;
        }

        Camera camera = Camera.main;
        camera.backgroundColor = Color.HSVToRGB(backCol, spectrum2, 1f);
    }
}
