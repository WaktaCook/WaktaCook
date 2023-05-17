using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace OpenAI
{
    public interface IOpenAIAPI
    {
        public string ReqestStringData(string messageString, bool DataAutoAdd = true, bool SendMessageDebugLog = false);
        public void DataAdd(string text, OpenAPIMessage.MessageRole _Role = OpenAPIMessage.MessageRole.assistant);
        public void DataErease(int number, int count);
        public void ResetData();
    }

    [SerializeField]
    public class OpenAIAPI : MonoBehaviour, IOpenAIAPI
    {
        public string apiKey = "key";
        public string apiKey_sub = "key";

        private string useKey = "";
        private bool isSubKey = false;
        public bool hasDefinedMessageJsonDataFile = true;
        public int maxTokens = 1000;
        public int reqestKeyChangeMaxCount = 2;

        private static HttpClient client;
        private static string jsonFileLocation = Application.streamingAssetsPath + "/OpenAI/PredefinedMessages.json";

        public delegate void stringEvent(string _string);
        public stringEvent completedRepostEvent;

        private int KeyChangeCount = 0;
        private const string apiURL = "https://api.openai.com/v1/chat/completions";
        private const string authorizationHeader = "Bearer";
        private const string userAgentHeader = "User-Agent";

        /// <summary>
        /// 미리 인스펙터에서 설정해둘 대화형 
        /// </summary>
        [JsonProperty("message")]
        public List<OpenAPIMessage> defineMessage;
        OpenAPIDefinedMessageClass definedmessageClass = new OpenAPIDefinedMessageClass();

        public OpenAIData apiData = new OpenAIData();

        private void Awake()
        {
            if (hasDefinedMessageJsonDataFile)
            {
                definedmessageClass = JsonPaser.Load<OpenAPIDefinedMessageClass>(jsonFileLocation);
                definedmessageClass.RefineMessageData(ref defineMessage);
            }
        }

        private void Start()
        {
            CreateHttpClient();
            InitReqestData();
        }

        public void DataAdd(string text, OpenAPIMessage.MessageRole _Role = OpenAPIMessage.MessageRole.assistant)
        {
            apiData.Multiplemessages.Add(new OpenAPIMessage { Role = _Role, Message = text });
        }

        public void DataErease(int number, int count)
        {
            apiData.Multiplemessages.RemoveRange(number, count);
        }

        public void ResetData()
        {
            apiData.Resetmessages();
        }

        public void InitReqestData()
        {
            if (defineMessage?.Count > 0)
            {
                apiData.ConceptSettingMessages = defineMessage;
            }
            apiData.Multiplemessages = new List<OpenAPIMessage>(apiData.ConceptSettingMessages);
            apiData.maxTokens = maxTokens;
        }

        public string ReqestStringData(string _messageString, bool _DataAutoAdd = true, bool _SendMessageDebugLog = false)
        {
            string ReqestStringData;

            DataAdd(_messageString, OpenAPIMessage.MessageRole.user);
            var responsData = ClieantResponse(apiData, _SendMessageDebugLog);
            ReqestStringData = responsData?.Result.Message[0].messageData.Content;

            if (_DataAutoAdd)
            {
                DataAdd(ReqestStringData);
            }
            if (completedRepostEvent != null)
                completedRepostEvent(ReqestStringData);
            return ReqestStringData;
        }

        /// <summary>
        /// 대화 메시지 보내면 자동으로 
        /// </summary>
        /// <param name="messageString">user가 입력한 string 값 </param>
        /// <param name="DataAutoAdd"> 데이터 자동 으로 multiplemessages 의 List에 추가</param>
        /// <returns>받은 데이터</returns>
        public async Task<string> AsyncRequestStringData(string _messageString, bool _DataAutoAdd = true, bool _SendMessageDebugLog = false)
        {
            string ReqestStringData;

            DataAdd(_messageString, OpenAPIMessage.MessageRole.user);
            var responsData = await ClieantResponse(apiData, _SendMessageDebugLog);
            ReqestStringData = responsData?.Message[0].messageData.Content;

            if (_DataAutoAdd)
            {
                DataAdd(ReqestStringData);
            }

            if (completedRepostEvent != null)
            {
                completedRepostEvent(ReqestStringData);
            }

            return ReqestStringData;
        }

        private void CreateHttpClient()
        {
            useKey = apiKey;
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authorizationHeader, useKey);
            client.DefaultRequestHeaders.Add(userAgentHeader, "okgodoit/dotnet_openai_api");
        }

        private async Task<OpenAPICompletionResult> ClieantResponse(OpenAIData request, bool SendMessageDebugLog)
        {
            if (client == null)
            {
                CreateHttpClient();
            }

            string jsonContent = JsonConvert.SerializeObject(request, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            var stringContent = new StringContent(jsonContent, UnicodeEncoding.UTF8, "application/json");
            if (SendMessageDebugLog)
            {
                UnityEngine.Debug.Log(jsonContent);
            }
            var response = await client.PostAsync(apiURL, stringContent);

            if (response.IsSuccessStatusCode)
            {
                KeyChangeCount = 0;
                string resultAsString = await response.Content.ReadAsStringAsync();
                UnityEngine.Debug.Log(resultAsString);
                var resultData = JsonConvert.DeserializeObject<OpenAPICompletionResult>(resultAsString);
                return resultData;
            }
            else
            {
                if (response.StatusCode == (System.Net.HttpStatusCode)429)
                {
                    if (KeyChangeCount < reqestKeyChangeMaxCount)
                    {
                        KeyChangeCount++;
                        useKey = isSubKey ? apiKey : apiKey_sub;
                        isSubKey = !isSubKey;
                        return await ClieantResponse(request, SendMessageDebugLog);
                    }
                }

                throw new HttpRequestException("Error calling OpenAi API to get completion.  HTTP status code: " + response.StatusCode.ToString() + ". Request body: " + jsonContent);
            }
        }
    }

    /// <summary>
    /// json 메시지를 보낼 형식 구성
    /// </summary>
    [SerializeField]
    public class OpenAIData
    {
        [JsonProperty("model")]
        public string Model = "gpt-3.5-turbo-0301";

        [JsonProperty("messages")]
        public List<OpenAPIMessage> Multiplemessages { get; set; } = new List<OpenAPIMessage>();

        //추가 옵션

        //[JsonProperty("temperature"), XmlAttribute("temperature")]
        //public int temperature = 1;

        //[JsonProperty("top_p"), XmlAttribute("top_p")]
        //public int top_p = 1;

        //[JsonProperty("n"), XmlAttribute("n")]
        //public int n = 1;

        //[JsonProperty("stream"), XmlAttribute("stream")]
        //public bool stream = false;

        //[JsonProperty("stop"), XmlAttribute("stop")]
        //public List<string> stop;

        [JsonProperty("max_tokens"), XmlAttribute("max_tokens")]
        public int maxTokens = 100;

        //[JsonProperty("presence_penalty"), XmlAttribute("presence_penalty")]
        //public int presence_penalty = 0;

        //[JsonProperty("frequency_penalty"), XmlAttribute("frequency_penalty")]
        //public int frequency_penalty = 0;

        //[JsonProperty("logit_bias"), XmlAttribute("logit_bias")]
        //public int logit_bias = 1;

        //[JsonProperty("user"), XmlAttribute("user")]
        //public string user = "00";

        [JsonIgnore]
        public List<OpenAPIMessage> ConceptSettingMessages { get; set; } = new List<OpenAPIMessage>();
        public void Resetmessages()
        {
            Multiplemessages.Clear();
            Multiplemessages = new List<OpenAPIMessage>(ConceptSettingMessages);
        }
    }

    /// <summary>
    /// json Defin Message Setting Data Class
    /// 
    /// defindMessage -> Role , Message
    /// 
    /// RefineMessageData -> (defindMessage chainge List<message>) 
    /// </summary>
    public class OpenAPIDefinedMessageClass
    {
        [JsonProperty("message")]
        public List<OpenAPIDefinedJsonMessage> definedMessages = new List<OpenAPIDefinedJsonMessage>();

        public void RefineMessageData(ref List<OpenAPIMessage> messages)
        {
            try
            {
                foreach (var data in definedMessages)
                {
                    messages.Add(new OpenAPIMessage { Role = (OpenAPIMessage.MessageRole)data.Role, Message = data.Message });
                }
            }
            catch (NullReferenceException e)
            {
                throw new Exception("Null Data" + e);
            }
        }
    }
    [Serializable]
    public class OpenAPIDefinedJsonMessage
    {
        [JsonProperty("role")]
        public int Role;
        [JsonProperty("message")]
        public string Message;
    }

    /// <summary>
    /// chat gpt json 기본구성
    /// 
    /// role 3가지 설정과 텍스트로 구성
    /// </summary>
    [Serializable]
    public class OpenAPIMessage
    {
        public enum MessageRole
        {
            [EnumMember(Value = "system")]
            system,
            [EnumMember(Value = "user")]
            user,
            [EnumMember(Value = "assistant")]
            assistant
        }

        [JsonProperty("role"), JsonConverter(typeof(StringEnumConverter)), XmlAttribute("role")]
        public MessageRole Role;
        [JsonProperty("content"), XmlAttribute("content")]
        public string Message = "";
    }

    #region jsonData class

    /// <summary>
    /// response 를 받았을때 데이터 구성 deserialize 를 위해 필요함
    /// </summary>
    public class OpenAPICompletionResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("choices")]
        public List<OpenAPINumberData> Message;
    }

    [System.Serializable]
    public class OpenAPINumberData
    {
        [JsonProperty("message")]
        public OpenAPIMessageData messageData { get; set; }
    }

    public class OpenAPIMessageData
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    #endregion
}