using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class GetQuesFromText : MonoBehaviour
{
    // Start is called before the first frame update
    [Serializable]
    public class Output
    {
        public List<Questions> questions;
        public string statement;
    }
    [Serializable]
    public class Questions
    {
        public string Answer;
        public string Question;
        public string context;
        public int id;
    }
    [Serializable]
    public class Root
    {
        public Output output;
    }

    public List<string> questionBank;
    void Start()
    {
        StartCoroutine(GetQues());
        Debug.Log("Question Bank - ");
    }

     IEnumerator GetQues()
    {
        yield return new WaitForSeconds(15);
        string para = FindObjectOfType<SpeechToText>().GetParagraph();

        Root questionsOutput = new Root();

        Debug.Log("Paragraph :" + para);

        WWWForm form = new WWWForm();
        form.AddField("text", para);

        UnityWebRequest www = UnityWebRequest.Post("http://c500-34-125-252-111.ngrok.io/text", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            questionsOutput = JsonUtility.FromJson<Root>(www.downloadHandler.text);
            ArrayList arrayList = new ArrayList(questionsOutput.output.questions);
            foreach (Questions item in arrayList)
                questionBank.Add(item.Question);
        }
    } 
}
