using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class IJOB : MonoBehaviour
{
    public NativeArray<int> numbers;
    public struct JobStruct : IJob
    {
        public NativeArray<int> Numbers;

        public void Execute() 
        {
            for (int i = 0; i < Numbers.Length; i++) 
            {
                if (Numbers[i] >= 10)
                {
                    Numbers[i] *= 0;
                }
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        numbers = new NativeArray<int>(10, Allocator.Persistent);
        for (int i = 0; i < numbers.Length; i++) 
        {
            numbers[i] = Random.Range(0, numbers.Length);
        }

        JobStruct jobStruct = new JobStruct()
        {
            Numbers = numbers,
        };
        foreach (var i in numbers) 
        {
            Debug.Log(i);
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnDestroy()
    {
        numbers.Dispose();
    }
}
