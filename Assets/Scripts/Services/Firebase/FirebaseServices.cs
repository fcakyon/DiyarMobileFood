using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.DynamicLinks;
using System;

public class FirebaseServices : MonoBehaviour {

    public UnityEngine.UI.Text text;

    // Use this for initialization
    void Start()
    {

        DynamicLinks.DynamicLinkReceived += OnDynamicLink;

    }

    // Display the dynamic link received by the application.
    void OnDynamicLink(object sender, EventArgs args)
    {
        var dynamicLinkEventArgs = args as ReceivedDynamicLinkEventArgs;
        text.text += "Received dynamic link: " +
                        dynamicLinkEventArgs.ReceivedDynamicLink.Url.OriginalString;

        Dictionary<string, string> queryParams = QueryParser(dynamicLinkEventArgs.ReceivedDynamicLink.Url);
        if (queryParams.ContainsKey("model"))
        {
            Debug.Log(queryParams["model"]);
        }

    }

    public Dictionary<string, string> QueryParser(Uri uri)
    {
        string query = uri.Query.Split('?')[1];
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        string[] parameters = query.Split('&');
        foreach (string s in parameters)
        {
            string[] pairs = s.Split('=');
            dictionary.Add(pairs[0], pairs[1]);
        }
        return dictionary;
    }
}
