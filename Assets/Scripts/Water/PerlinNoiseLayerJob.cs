using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct PerlinNoiseLayerJob : IJobParallelFor
{
    public NativeArray<Vector3> Vertices;
    public float Time;

    public void Execute(int index)
    {
        var vertex = Vertices[index];

        var x = vertex.x * Time * .25f;
        var z = vertex.z * Time * .25f;

        vertex.y = Mathf.PerlinNoise(x, z) - 0.5f;

        Vertices[index] = vertex;
    }
}