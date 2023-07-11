using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class IJOBPARALLELFOR : MonoBehaviour
{
    private void Start()
    {
        int numElements = 100;
        NativeArray<Vector3> positions = new NativeArray<Vector3>(numElements, Allocator.TempJob);
        NativeArray<Vector3> velocities = new NativeArray<Vector3>(numElements, Allocator.TempJob);

        for (int i = 0; i < numElements; i++)
        {
            positions[i] = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            velocities[i] = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

        NativeArray<Vector3> finalPositions = new NativeArray<Vector3>(numElements, Allocator.TempJob);

        SumPositionsAndVelocitiesJob job = new SumPositionsAndVelocitiesJob
        {
            Positions = positions,
            Velocities = velocities,
            FinalPositions = finalPositions
        };

        JobHandle jobHandle = job.Schedule(numElements, 64); // Здесь 64 - это размер блока (chunk size), вы можете изменить его на оптимальное значение

        jobHandle.Complete();

        for (int i = 0; i < numElements; i++)
        {
            Debug.Log("Final Position " + i + ": " + finalPositions[i]);
        }

        positions.Dispose();
        velocities.Dispose();
        finalPositions.Dispose();
    }

    private struct SumPositionsAndVelocitiesJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> Positions;

        [ReadOnly]
        public NativeArray<Vector3> Velocities;

        public NativeArray<Vector3> FinalPositions;

        public void Execute(int index)
        {
            FinalPositions[index] = Positions[index] + Velocities[index];
        }
    }
}