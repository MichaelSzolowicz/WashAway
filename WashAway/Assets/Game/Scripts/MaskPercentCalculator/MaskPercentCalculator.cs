using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MaskPercentCalculator
{
    private Action<float> onCompleteRequest;
    private int maskSize = 0;

    public MaskPercentCalculator(Action<float> onCompleteRequest)
    {
        this.onCompleteRequest = onCompleteRequest;
    }

    public void RequestPercentCleared(RenderTexture mask)
    {
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
            uint alpha = (byte)(data[i] >> 24);

            if(alpha > 255 / 2)
            {
                maskedPixelCount++;
            }
        }

        onCompleteRequest((float)maskedPixelCount/maskSize*100);
    }
}
