using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PixelReader
{
    private bool available = true;
    public Color32 result;

    public bool Available { get { return available; } }

    public void ReadPixelAsync(Texture src, int mipIndex, int x, int width, int y, int height, int z = 0, int depth = 1)
    {
        if (x < 0 || x >= src.width || y < 0 || y >= src.height) return;

        available = false;
        AsyncGPUReadback.Request(src, mipIndex, x, width, y, height, z, depth, OnCompleteReadback);
    }

    private void OnCompleteReadback(AsyncGPUReadbackRequest request)
    {
        if(request.hasError)
        {
            Debug.LogError("Async GPU Readback error detected. ");
            available = true;
            //return;
        }

        //Debug.Log(request.done);

        NativeArray<uint> data = request.GetData<uint>();

        if(data.Length <= 0)
        {
            Debug.LogError("Async GPU Readback data length is less than 1.");
        }

        uint c = data[0];
        result.r = (byte)(c);
        result.g = (byte)(c >> 8);
        result.b = (byte)(c >> 16);
        result.a = (byte)(c >> 24);

        available = true;
    }
}

