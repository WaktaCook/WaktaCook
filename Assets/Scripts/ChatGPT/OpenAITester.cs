using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;

public class OpenAITester : MonoBehaviour
{
    public Text text;
    public Text inputFieldText;

    private string postText;
    private string requestText = "";
    private OpenAIAPI api;

    // Start is called before the first frame update
    void Start()
    {
        api = this.gameObject.GetComponent<OpenAIAPI>();
        api.completedRepostEvent = delegate (string _string) { postText = _string; };
    }

    public void Request()
    {
        requestText = (inputFieldText.text.ToString());
        Test();
    }

    private async void Test()
    {
        postText = (await api.AsyncRequestStringData(requestText, _SendMessageDebugLog: true));

    }
    private void Update()
    {
        text.text = postText;
    }
}