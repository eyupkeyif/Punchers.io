using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
using System.Xml;
using System.Xml.Serialization;


public class XMLOp
{
    public static void Serialize(object item, string path)
    {
        XmlSerializer serializer = new XmlSerializer(item.GetType());
        StreamWriter writer = new StreamWriter(path);
        serializer.Serialize(writer.BaseStream, item);
        writer.Close();
    }

    public static T Deserialize<T>(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StreamReader reader = new StreamReader(path);
        T deserialized = (T)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return deserialized;
    }
}

public class SKAdNetworkID
{
    [XmlElement("key")]
    public string key;

    [XmlElement("string")]
    public string id;
}

[XmlRoot(ElementName = "array")]
public class SKAdNetworkItems
{
    [XmlElement("dict")]
    public SKAdNetworkID[] idArray;
}

public class PostBuildStep
{
    /// <summary>
    /// Description for IDFA request notification 
    /// [sets NSUserTrackingUsageDescription]
    /// </summary>
    const string TrackingDescription =
        "Your data will be used to provide you a better and personalized ad experience.";



    [PostProcessBuild(0)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToXcode)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            AddPListValues(pathToXcode);
        }
    }



    static void AddPListValues(string pathToXcode)
    {

        //SKAdNetworkItems sKAdNetworkItems = XMLOp.Deserialize<SKAdNetworkItems>("Assets/TextAssets/SKAdNetworkIDs.xml");
#if UNITY_IOS
        // Get Plist from Xcode project 
        string plistPath = pathToXcode + "/Info.plist";

        // Read in Plist 
        PlistDocument plistObj = new PlistDocument();
        plistObj.ReadFromString(File.ReadAllText(plistPath));

        // set values from the root obj
        PlistElementDict plistRoot = plistObj.root;

        // Set value in plist
        plistRoot.SetString("NSUserTrackingUsageDescription", TrackingDescription);

        plistRoot.SetString("FacebookClientToken", "d92accbc9e3ee342f01a8eac54e09577");


        ////XmlReader xmlReader = new XmlReader
        //PlistElementArray SKAdNetworkArray = plistRoot.CreateArray("SKAdNetworkItems");

        //foreach (SKAdNetworkID id in sKAdNetworkItems.idArray)
        //{
        //    PlistElementDict dict = SKAdNetworkArray.AddDict();
        //    dict.SetString(id.key, id.id);
        //}
        // save
        File.WriteAllText(plistPath, plistObj.WriteToString());
#endif
    }

}
