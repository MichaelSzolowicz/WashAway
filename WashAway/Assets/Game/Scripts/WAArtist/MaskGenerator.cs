using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaskGenerator : WAArtist
{
    new protected void Start()
    {
        base.Start();

        GameState.onToggleCharacterDead += OnDeath;
    }

    private void OnDeath()
    {
        if (!GameState.CharacterDead) return;

        Graphics.SetRenderTarget(painting);
        commandBuffer.ClearRenderTarget(true, true, Color.clear);
        Graphics.ExecuteCommandBuffer(commandBuffer);
    }

    new protected void OnDestroy()
    {
        base.OnDestroy();

        GameState.onToggleCharacterDead -= OnDeath;
    }
}
