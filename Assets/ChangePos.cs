using UnityEngine;

public class ChangePos : MonoBehaviour
{
    // 2D arrays: [segmentIndex, sphereIndex]
    // Row = time segment (0-10, 10-20, 20-30)
    // Col = sphere id
    // Example: startPositions[1, 25] -> sphere 25 start position in 10-20s segment
    Vector3[,] startPositions;
    Vector3[,] endPositions;

    void Start()
    {
        timeFlags = new float[] { 0f, 10f, 20f, 30f };
        segmentCount = timeFlags.Length - 1;
        spheres = new GameObject[numSphere];
        startPositions = new Vector3[segmentCount, numSphere];
        endPositions = new Vector3[segmentCount, numSphere];

        for (int i = 0; i < numSphere; i++)
        {
            // Segment 0 : 0s -> 10s
            startPositions[0, i] = xxx;
            endPositions[0, i] = xxx;

            // Segment 1 : 10s -> 20s
            startPositions[1, i] = xxx;
            endPositions[1, i] = xxx;

            // Segment 2 : 20s -> 30s
            startPositions[2, i] = xxx;
            endPositions[2, i] = xxx;
        }

    }

    
	void Update()
    {
        float currentTime = Time.time;

        if (currentTime < timeFlags[1])
        {

				

        }
        else if (currentTime < timeFlags[2])
        {


        }
        else if (currentTime < timeFlags[3])
        {


        }
        else
        {


        }
    }
}
