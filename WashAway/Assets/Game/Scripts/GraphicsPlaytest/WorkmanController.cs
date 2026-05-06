using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkmanController : MonoBehaviour
{
    [SerializeField] private Transform workman;
    [SerializeField] private Transform moveTo;
    [SerializeField] private float speed;
    [SerializeField] private GameObject lineCollider;
    [SerializeField] private SpriteRenderer workmanRenderer;
    [SerializeField] private Sprite runAnim;


    [SerializeField] private float reactionDelay = .5f;

    private bool destinationReached = false;

    public void StartRunAway()
    {
        StartCoroutine(RunAwayCoroutine());
    }

    private IEnumerator RunAwayCoroutine()
    {
        if(lineCollider != null) 
            lineCollider.SetActive(false);

        yield return new WaitForSeconds(reactionDelay);

        if(runAnim != null) 
            workmanRenderer.sprite = runAnim;

        while (!destinationReached)
        {
            Vector3 direction = (moveTo.position - workman.position).normalized;

            Vector3 newPosition = workman.position + direction * speed * Time.deltaTime;
            
            if(Vector3.Dot((moveTo.position - newPosition).normalized, direction) <= 0)
            {
                workman.position = moveTo.position;
                destinationReached = true;
            }
            else
            {
                workman.position = newPosition;
            }

            yield return new WaitForSeconds(0); 
        }
    }

}
