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
        available = false;
        AsyncGPUReadback.Request(src, mipIndex, x, width, y, height, z, depth, OnCompleteReadback);
    }

    private void OnCompleteReadback(AsyncGPUReadbackRequest request)
    {
        if(request.hasError)
        {

            Debug.LogError("Async GPU Readback error detected.");
            return;
        }

        NativeArray<uint> data = request.GetData<uint>();

        if(data.Length <= 0)
        {
            Debug.LogWarning("Async GPU Readback data length is less than 1.");
        }

        uint c = data[0];
        result.b = (byte)((c) & 0xFF);
        result.g = (byte)((c >> 8) & 0xFF);
        result.r = (byte)((c >> 16) & 0xFF);
        result.a = (byte)((c >> 24) & 0xFF);

        available = true;
    }
}
