//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Configuration;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JEM.Test
{
    public class JEMSmartConfigurationTest : MonoBehaviour
    {
        private void Awake()
        {
            MyConfig.Load();
            MyConfig.Loaded.Database.RegisterObjectChange("test.bool", o =>
            { 
                Debug.Log("Hey, A change!");
            });

            Debug.Log($"test.bool: {MyConfig.TestBool}");
            Debug.Log($"test.string: {MyConfig.TestString}");

            MyConfig.TestString += Random.Range(1, 9);
            MyConfig.CustomSerializable.MyString += ", Hello!";

            Debug.Log("MyConfig2 ->");

            MyConfig2.Load();
            MyConfig2.Loaded.Database.RegisterObjectChange("TestBool", o =>
            {
                Debug.Log("Hey, A change!");
            });

            Debug.Log($"TestBool {MyConfig2.TestBool}");
            Debug.Log($"TestString: {MyConfig2.TestString}");

            MyConfig2.TestString += Random.Range(1, 9);
            MyConfig2.CustomSerializable.MyString += ", Hello!";

            Debug.Log(MyConfig2.MyEnum);

        }

        private IEnumerator Start()
        {
            while (true)
            {
                MyConfig.Update();
                MyConfig2.Update();
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnApplicationQuit()
        {
            MyConfig.Save();
            MyConfig2.Save();
        }
    }

    [Serializable]
    public class MyCustomSerializable
    {
        public string MyString = "Hello, World";
        public bool MyBool = true;
    }

    public enum MyEnum
    {
        Hello,
        World,
    }

    [JEMSmartConfig("smartTest")]
    public static class MyConfig
    {
        [JEMSmartConfigVar("test.bool")] public static bool TestBool { get; set; } = true;
        [JEMSmartConfigVar("test.string", "OnStringChanged")] public static string TestString { get; set; } = "Hello Smart Configurations!";
        [JEMSmartConfigVar("test.int32")] public static int TestInt32 { get; set; } = 32;
        [JEMSmartConfigVar("test.int16")] public static short TestInt16 { get; set; } = 16;
        [JEMSmartConfigVar("test.int64")] public static long TestInt64 { get; set; } = 32;

        [JEMSmartConfigVar("test.int32N")] public static int TestInt32N { get; set; } = -32;
        [JEMSmartConfigVar("test.int16N")] public static short TestInt16N { get; set; } = -16;
        [JEMSmartConfigVar("test.int64N")] public static long TestInt64N { get; set; } = -32;

        [JEMSmartConfigVar("test.single")] public static float TestSingle { get; set; } = 4.1234f;
        [JEMSmartConfigVar("test.double")] public static double TestDouble { get; set; } = 8.5678;

        [JEMSmartConfigVar("test.uint32")] public static uint TestUInt32 { get; set; } = 132;
        [JEMSmartConfigVar("test.uint16")] public static ushort TestUInt16 { get; set; } = 116;
        [JEMSmartConfigVar("test.uint64")] public static ulong TestUInt64 { get; set; } = 132;

        [JEMSmartConfigVar("test.custom")] public static MyCustomSerializable CustomSerializable { get; set; } = new MyCustomSerializable();

        private static void OnStringChanged(object obj)
        {
            Debug.Log($"The string was changed! {(string) obj}");
        }

        internal static void Load() => Loaded = JEMSmartConfiguration.LoadConfiguration(typeof(MyConfig));
        internal static void Update() => Loaded?.Update();
        internal static void Save() => Loaded?.Save();

        internal static JEMSmartConfigSave Loaded { get; private set; }
    }

    [JEMSmartConfig("smartTest2", PropertyMode = JEMSmartConfigPropertyMode.ForcedAll)]
    public static class MyConfig2
    {
        public static bool TestBool { get; set; } = true;
        public static string TestString { get; set; } = "Hello Smart Configurations!";
        public static int TestInt32 { get; set; } = 32;
        public static short TestInt16 { get; set; } = 16;
        public static long TestInt64 { get; set; } = 32;

        public static int TestInt32N { get; set; } = -32;
        public static short TestInt16N { get; set; } = -16;
        public static long TestInt64N { get; set; } = -32;

        public static float TestSingle { get; set; } = 4.1234f;
        public static double TestDouble { get; set; } = 8.5678;

        public static uint TestUInt32 { get; set; } = 132;
        public static ushort TestUInt16 { get; set; } = 116;
        public static ulong TestUInt64 { get; set; } = 132;

        public static MyEnum MyEnum { get; set; } = MyEnum.World;

        public static MyCustomSerializable CustomSerializable { get; set; } = new MyCustomSerializable();

        internal static void Load() => Loaded = JEMSmartConfiguration.LoadConfiguration(typeof(MyConfig2));
        internal static void Update() => Loaded?.Update();
        internal static void Save() => Loaded?.Save();

        internal static JEMSmartConfigSave Loaded { get; private set; }
    }
}
