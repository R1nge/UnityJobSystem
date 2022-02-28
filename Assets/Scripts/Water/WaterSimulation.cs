using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class WaterSimulation : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter.mesh.MarkDynamic();
    }

    private void Update()
    {
        var vertices = meshFilter.mesh.vertices;

        var vertexArray = new NativeArray<Vector3>(vertices, Allocator.TempJob);
        var job1 = new PerlinNoiseLayerJob
        {
            Vertices = vertexArray,
            Time = Time.timeSinceLevelLoad
        };

        JobHandle handle = job1.Schedule(vertices.Length, 250);
        handle.Complete();

        var job2 = new PerlinNoiseLayerJob
        {
            Vertices = vertexArray,
            Time = Time.timeSinceLevelLoad
        };
        JobHandle handle2 = job2.Schedule(vertices.Length, 250, handle);

        handle2.Complete();

        vertexArray.CopyTo(vertices);
        vertexArray.Dispose();

        meshFilter.mesh.SetVertices(vertices.ToList());
        meshFilter.mesh.RecalculateNormals();
        meshFilter.mesh.RecalculateBounds();
    }
}