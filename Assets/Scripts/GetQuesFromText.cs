using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetQuesFromText : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        Debug.Log("Paragraph :" + FindObjectOfType<SpeechToText>().GetParagraph());
        StartCoroutine(WaitForParagraph());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator  WaitForParagraph()
    {
        while(true){
        yield return new WaitForSeconds (15);
        Debug.Log("Paragraph :" + FindObjectOfType<SpeechToText>().GetParagraph());
        }

    }
}
