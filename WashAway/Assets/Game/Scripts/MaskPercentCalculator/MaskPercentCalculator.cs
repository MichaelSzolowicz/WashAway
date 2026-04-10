using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MaskPercentCalculator
{
    public enum Channel
    {
        R = 0,
        G = 8,
        B = 16,
        A = 24
    }

    private Action<float> onCompleteRequest;
    private int maskSize = 0;
    private Channel channel;    

    public MaskPercentCalculator(Action<float> onCompleteRequest)
    {
        this.onCompleteRequest = onCompleteRequest;
    }

    public void RequestPercentCleared(RenderTexture mask, Channel channel = Channel.R)
    {
        this.channel = channel;
        maskSize = mask.width * mask.height;
        AsyncGPUReadback.Request(mask, 0, 0, mask.width, 0, mask.height, 0, 1, OnCompletePercentRequest);
    }

    private void OnCompletePercentRequest(AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.LogError("Async GPU Readback error detected.");
            return;
        }

        NativeArray<uint> data = request.GetData<uint>();

        if (data.Length <= 0)
        {
            Debug.LogError("Async GPU Readback data length is less than 1.");
        }

        int maskedPixelCount = 0;
        for (int i = 0; i < data.Length; i++)
        {
            uint c = (byte)(data[i] >> (int)channel);

            if(c != 0)
            {
                maskedPixelCount++;
            }
        }

        onCompleteRequest((float)maskedPixelCount/maskSize*100);
    }
}
