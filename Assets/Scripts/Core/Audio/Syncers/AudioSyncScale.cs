using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncScale : AudioSyncer
{
    public Vector3 beatScale;
    public Vector3 restScale;

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isBeat) return;
        transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
    }

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", beatScale);
    }

    private IEnumerator MoveToScale(Vector3 targetScale)
    {
        Vector3 currentScale = transform.localScale;
        Vector3 initialScale = currentScale;
        Vector3 editedScale = new(targetScale.x, (targetScale.y + audioValue)/beatScale.y, targetScale.z);
        float timer = 0;

        while (currentScale != editedScale)
        {
            currentScale = Vector3.Lerp(initialScale, editedScale, timer / timeToBeat);
            timer += Time.deltaTime;

            transform.localScale = currentScale;
            yield return null;
        }
        isBeat = false;
    }


}
