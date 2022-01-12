using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FinTOKMAK.WeaponSystem.Runtime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WeaponRuntimeDataSerializationTest
{
    
    
    /// <summary>
    /// Test the basic serialization of the WeaponRuntimeData class.
    /// </summary>
    [Test]
    public void WeaponRuntimeDataSerializationTestSimplePasses()
    {
        WeaponRuntimeData runtimeData = ScriptableObject.CreateInstance<WeaponRuntimeData>();
        runtimeData.id = "TEST_WEAPON_ID";
        byte[] serializedObj = ObjectToByteArray(runtimeData);
        WeaponRuntimeData deserializedRuntimeData = (WeaponRuntimeData) ByteArrayToObject(serializedObj);
        
        Assert.AreEqual(deserializedRuntimeData.id, runtimeData.id);
    }
    
    // Convert an object to a byte array
    private static byte[] ObjectToByteArray(Object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
    
    // Convert a byte array to an Object
    private static System.Object ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }
}
