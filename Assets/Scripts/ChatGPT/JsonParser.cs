using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

static public class JsonPaser
{
    static public T Load<T>(string FileLocation) where T : class
    {
        try
        {
            var jsonFile = File.ReadAllText(FileLocation);
            var data = JsonUtility.FromJson<T>(jsonFile) as T;
            return data;
        }
        catch (Exception error)
        {
            throw new JsonException("err Loader" + error);

        }
        return default(T);
    }

    static public void Save<T>(string FileLocation, T data) where T : class
    {
        try
        {
            string serializedData = (JsonUtility.ToJson(data));
            File.WriteAllText(FileLocation, serializedData);

        }
        catch (Exception error)
        {
            Debug.LogWarning(error);
        }

    }
}