using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Conditions : MonoBehaviour
{
    private int Points = 0;
    public int actualPoints = 0;
    public int AmountTillNextLevel = 5;
    public OnChangePosition HoleScript;
    public TMP_Text scoreText;
    private int amt = 5;
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        CalculateProgress();
        Points += other.gameObject.GetComponent<GenericObject>().worth;
        actualPoints += other.gameObject.GetComponent<GenericObject>().worth;
        //if (other.gameObject.GetComponent<GenericObject>().worth <= 5)
        //{
        //    amt = 0;
        //}
    }

    private void CalculateProgress()
    {
        if (Points % AmountTillNextLevel == 0)
        {
            StartCoroutine(HoleScript.ScaleHole());
            AmountTillNextLevel += amt;
            //if (amt == 0)
            //{
            //    amt = 5;
            //}
            HoleScript.holeSpeed -= 0.01f;
            Points = 0;
        }
    }

    void Update()
    {
        scoreText.text = actualPoints.ToString();
    }
}
