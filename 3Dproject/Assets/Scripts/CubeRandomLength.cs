using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRandomLength : MonoBehaviour
{
    private float minLength = 2;
    private float maxLength = 20;
    private float growthRate = 10f;
    private float minGrowthRate = 5f;
    private float maxGrowthRate = 13.5f;
    private bool isGrowing = true;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x, Random.Range(minLength, maxLength), transform.localScale.z);
        growthRate = Random.Range(minGrowthRate, maxGrowthRate);
    }

    // Update is called once per frame
    void Update()
    {
        LerpingSizeOnY();
    }

    public void LerpingSizeOnY()
    {
        float yScale = transform.localScale.y;
        if(yScale >= maxLength)
            isGrowing = false;
        if(yScale <= minLength)
            isGrowing = true;
        if(yScale <= maxLength && isGrowing)
            Growing();
        else if(yScale >= minLength && isGrowing == false)
            Shrinking();
    }

    private void Growing()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + growthRate * Time.deltaTime, transform.localScale.z);
    }

    private void Shrinking()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - growthRate * Time.deltaTime, transform.localScale.z);
    }
}