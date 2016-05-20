using UnityEngine;
using System.Collections;
using Sapphire;
using Google.Protobuf;
using System.IO;

public class TestProto : MonoBehaviour {

	// Use this for initialization
	void Start () {
        login msg = new login
        {
            Username = "abcdef",
            Passwd = "123456"
        };
        using (var output = File.Create("test.dat"))
        {
            msg.WriteTo(output);
        }
        //        byte[] buffer = new byte[msg.CalculateSize()];

        //CodedOutputStream stream = new CodedOutputStream(new byte[msg.CalculateSize()]);
        //msg.WriteTo(stream);
        login read;
        using (var input = File.OpenRead("test.dat"))
        {
            read = login.Parser.ParseFrom(input);
        }


    }

    // Update is called once per frame
    void Update () {
	
	}
}
