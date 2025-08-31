using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AnimationCurve animationCurve = null;

    private float time;

    private void Update()
    {
        time += Time.deltaTime;

        transform.localScale = new Vector3(1, animationCurve.Evaluate(time), 1);
    }
}
