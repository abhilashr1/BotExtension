using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace BotExtension.Helper
{
    public class FetchIDKeys
    {
        public static Tuple<string,string> GetMicrosoftKeys()
        {
            // Values which we want to output
            String MicrosoftAppID = "";
            String MicrosoftAppPassword = "";

            // The default SDK will be 4, unless changed. 

            String botFilePath = Environment.GetEnvironmentVariable("botFilePath");
            String botFileSecret = Environment.GetEnvironmentVariable("botFileSecret");

            if (string.IsNullOrEmpty(botFilePath))
            {
                // v3 Detected. 
                // Populate the v3 related entries
                MicrosoftAppID = Environment.GetEnvironmentVariable("MicrosoftAppId");
                MicrosoftAppPassword = Environment.GetEnvironmentVariable("MicrosoftAppPassword");

                if (string.IsNullOrEmpty(MicrosoftAppID) || string.IsNullOrEmpty(MicrosoftAppPassword))
                {
                    // If we dont have anything in the environment variables, try to get the values from web.config.
                    XmlDocument doc = new XmlDocument();
                    doc.Load("../../site/wwwroot/web.config");
                    XmlNodeList elemList = doc.GetElementsByTagName("add");

                    for (int i = 0; i < elemList.Count; i++)
                    {
                        if (elemList[i].Attributes != null)
                        {
                            var x = elemList[i].Attributes["key"];
                            if (x != null && x.Value == "MicrosoftAppId")
                                if (elemList[i].Attributes["value"] != null && elemList[i].Attributes["value"].Value != "")
                                    MicrosoftAppID = elemList[i].Attributes["value"].Value;

                            if (x != null && x.Value == "MicrosoftAppPassword")
                                if (elemList[i].Attributes["value"] != null && elemList[i].Attributes["value"].Value != "")
                                    MicrosoftAppPassword = elemList[i].Attributes["value"].Value;

                        }

                    }
                }

            }
            else
            {
                // Populate the v4 related entries

                string botFileRelativePath = System.IO.Path.Combine(@"..\..\site\wwwroot", botFilePath);
                string botFile = System.IO.File.ReadAllText(botFileRelativePath);
                var botFileObject = JObject.Parse(botFile);

                foreach (var ele in botFileObject["services"])
                {
                    if (ele["appPassword"] != null && (string)ele["appPassword"] != "")
                    {
                        MicrosoftAppID = (string)ele["appId"];
                        MicrosoftAppPassword = (string)ele["appPassword"];
                    }
                }

                // If the padlock is not null or empty, then try to decrypt it. If the padlock is null, then the AppPassword is plain-text
                string password = (string)botFileObject["padlock"];
                if (!(password == null || password == ""))
                {
                    /* Decrypt the MicrosoftAppPassword*/
                    MicrosoftAppPassword = Decrypter.DecryptString(MicrosoftAppPassword, botFileSecret);
                }
            }

            return new Tuple<string, string>(MicrosoftAppID, MicrosoftAppPassword);
        }


        public static Tuple<string, string, string> GetLuisKeys()
        {
            // Values which we want to output
            String LUISAppID = "";
            String LUISKey = "";
            String LUISHostname = "";

            // The default SDK will be 4, unless changed. 
            // Detect the SDK Version being used by checking if there is a .bot file present. If it is, then it is a v4 bot. 
            String botFilePath = Environment.GetEnvironmentVariable("botFilePath");
            String botFileSecret = Environment.GetEnvironmentVariable("botFileSecret");

            
            if (string.IsNullOrEmpty(botFilePath))
            {
                // v3 Detected. 
                // Populate the v3 related entries
                LUISAppID = Environment.GetEnvironmentVariable("LuisAppId");
                LUISKey = Environment.GetEnvironmentVariable("LuisAPIKey");
                LUISHostname = Environment.GetEnvironmentVariable("LuisAPIHostName");


                if (string.IsNullOrEmpty(LUISAppID) || string.IsNullOrEmpty(LUISKey) || string.IsNullOrEmpty(LUISHostname))
                {
                    // If we dont have anything in the environment variables, try to get the values from web.config.
                    XmlDocument doc = new XmlDocument();
                    doc.Load("../../site/wwwroot/web.config");
                    XmlNodeList elemList = doc.GetElementsByTagName("add");

                    for (int i = 0; i < elemList.Count; i++)
                    {
                        if (elemList[i].Attributes != null)
                        {
                            var x = elemList[i].Attributes["key"];
                            if (x != null && x.Value == "LuisAppId")
                                if (elemList[i].Attributes["value"] != null && elemList[i].Attributes["value"].Value != "")
                                    LUISAppID = elemList[i].Attributes["value"].Value;

                            if (x != null && x.Value == "LuisAPIKey")
                                if (elemList[i].Attributes["value"] != null && elemList[i].Attributes["value"].Value != "")
                                    LUISKey = elemList[i].Attributes["value"].Value;

                            if (x != null && x.Value == "LuisAPIHostName")
                                if (elemList[i].Attributes["value"] != null && elemList[i].Attributes["value"].Value != "")
                                    LUISHostname = elemList[i].Attributes["value"].Value;

                        }

                    }
                }

            }
            else
            {
                // Populate the v4 related entries

                string botFileRelativePath = System.IO.Path.Combine(@"..\..\site\wwwroot", botFilePath);
                string botFile = System.IO.File.ReadAllText(botFileRelativePath);
                var botFileObject = JObject.Parse(botFile);
                bool isLuisPresent = false;

                foreach (var ele in botFileObject["services"])
                {
                    if (ele["type"] != null && (string)ele["type"] == "luis")
                    {
                        isLuisPresent = true;
                        if (ele["appId"] != null && (string)ele["appId"] != "")
                        {
                            LUISAppID = (string)ele["appId"];
                            LUISKey = (string)ele["authoringKey"];
                            LUISHostname = (string)ele["region"] == "" ? "" : (string)ele["region"]+ ".api.cognitive.microsoft.com";
                        }
                    }
                }
                if (isLuisPresent == false)
                {
                    return new Tuple<string, string, string>("", "", "");
                }


                // If the padlock is not null or empty, then try to decrypt it. If the padlock is null, then the AppPassword is plain-text
                string password = (string)botFileObject["padlock"];
                if (!(password == null || password == ""))
                {
                    /* Decrypt the MicrosoftAppPassword*/
                    LUISKey = Decrypter.DecryptString(LUISKey, botFileSecret);
                }
            }

            return new Tuple<string, string,string>(LUISAppID, LUISKey, LUISHostname);
        }


        public static Tuple<string, string, string> GetQnaKeys()
        {
            // Values which we want to output
            String QnAKnowledgebaseId = "";
            String QnAAuthKey = "";
            String QnAEndpointHostName = "";

            // The default SDK will be 4, unless changed. 
            // Detect the SDK Version being used by checking if there is a .bot file present. If it is, then it is a v4 bot. 
            String botFilePath = Environment.GetEnvironmentVariable("botFilePath");
            String botFileSecret = Environment.GetEnvironmentVariable("botFileSecret");


            if (string.IsNullOrEmpty(botFilePath))
            {
                // v3 Detected. 
                // Populate the v3 related entries
                QnAKnowledgebaseId = Environment.GetEnvironmentVariable("QnAKnowledgebaseId");
                QnAAuthKey = Environment.GetEnvironmentVariable("QnAAuthKey");
                QnAEndpointHostName = Environment.GetEnvironmentVariable("QnAEndpointHostName");


                if (string.IsNullOrEmpty(QnAKnowledgebaseId) || string.IsNullOrEmpty(QnAAuthKey) || string.IsNullOrEmpty(QnAEndpointHostName))
                { 
                    // If we dont have anything in the environment variables, try to get the values from web.config.
                    XmlDocument doc = new XmlDocument();
                    doc.Load("../../site/wwwroot/web.config");
                    XmlNodeList elemList = doc.GetElementsByTagName("add");

                    for (int i = 0; i < elemList.Count; i++)
                    {
                        if (elemList[i].Attributes != null)
                        {
                            var x = elemList[i].Attributes["key"];
                            if (x != null && x.Value == "QnAKnowledgebaseId")
                                if (elemList[i].Attributes["value"] != null && elemList[i].Attributes["value"].Value != "")
                                    QnAKnowledgebaseId = elemList[i].Attributes["value"].Value;

                            if (x != null && x.Value == "QnAAuthKey")
                                if (elemList[i].Attributes["value"] != null && elemList[i].Attributes["value"].Value != "")
                                    QnAAuthKey = elemList[i].Attributes["value"].Value;

                            if (x != null && x.Value == "QnAEndpointHostName")
                                if (elemList[i].Attributes["value"] != null && elemList[i].Attributes["value"].Value != "")
                                    QnAEndpointHostName = elemList[i].Attributes["value"].Value;

                        }

                    }
                }

            }
            else
            {
                // Populate the v4 related entries

                string botFileRelativePath = System.IO.Path.Combine(@"..\..\site\wwwroot", botFilePath);
                string botFile = System.IO.File.ReadAllText(botFileRelativePath);
                var botFileObject = JObject.Parse(botFile);
                bool isQnaPresent = false;
                foreach (var ele in botFileObject["services"])
                {
                    if (ele["type"] != null && (string)ele["type"] == "qna")
                    {
                        isQnaPresent = true;
                        if (ele["appId"] != null && (string)ele["appId"] != "")
                        {
                            QnAKnowledgebaseId = (string)ele["kbId"];
                            QnAAuthKey = (string)ele["endpointKey"];
                            QnAEndpointHostName = (string)ele["hostname"];
                        }
                    }
                }

                if(isQnaPresent == false) {
                    return new Tuple<string, string, string>("", "", "");
                }

                // If the padlock is not null or empty, then try to decrypt it. If the padlock is null, then the AppPassword is plain-text
                string password = (string)botFileObject["padlock"];
                if (!(password == null || password == ""))
                {
                    /* Decrypt the MicrosoftAppPassword*/
                    QnAAuthKey = Decrypter.DecryptString(QnAAuthKey, botFileSecret);
                }
            }

            return new Tuple<string, string, string>(QnAKnowledgebaseId, QnAAuthKey, QnAEndpointHostName);
        }


    }
}
