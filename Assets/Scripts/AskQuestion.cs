using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class AskQuestion : MonoBehaviour
{
    public static string language = "en-ie";
    private string APIKey = "6c3b3753038e41a9b22cd322a425799f";
    private string _source;
    AudioSource audioSource;
    private GameObject[] VHs;
    Animator VhAnim;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        VHs = GameObject.FindGameObjectsWithTag("VH");
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<SpeechToText>().checkAskQuestion())
        {
            StartCoroutine(getAudio());
            FindObjectOfType<SpeechToText>().askQuestion = false;
        }

        
    }

    IEnumerator getAudio()
    {
        int randIdx = UnityEngine.Random.Range(0, FindObjectOfType<GetQuesFromText>().questionBank.Count);
        int randVH = UnityEngine.Random.Range(0, 4);
        VhAnim = VHs[randVH].GetComponent<Animator>();
        foreach (string item in FindObjectOfType<GetQuesFromText>().questionBank)
            Debug.Log("Question Bank - " + item);
        Debug.Log("Question -" + FindObjectOfType<GetQuesFromText>().questionBank[randIdx]);
        Regex rgx = new Regex("\\s+");
        _source = rgx.Replace(FindObjectOfType<GetQuesFromText>().questionBank[randIdx], "%20");
        string url = String.Format("http://api.voicerss.org/?key={0}&hl={1}&src={2}&c=WAV", APIKey, language, _source);
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
        Debug.Log("Sending request");
        yield return www.SendWebRequest();
        AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
        if (clip.length > 0 && clip != null)
        {
            Debug.Log("Playing Audio");
            audioSource.clip = clip;
            audioSource.Play();
            
            VhAnim.SetBool("isAsking", true);
            yield return new WaitForSeconds(clip.length);
            VhAnim.SetBool("isAsking", false);
            
        }
        else 
        {
            Debug.LogError("Failed to get the voice. Please try:\n" +
                "1.Try it in other languages.\n" +
                "2.Fill in something in text field.\n" +
                "3.Choose the correct audio format.");
        }
    }
}
