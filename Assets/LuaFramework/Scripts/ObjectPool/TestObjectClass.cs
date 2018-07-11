using UnityEngine;
using System.Collections;

namespace LuaFramework {

    public class TestObjectClass {

        public string name;
        public int value1;
        public float value2;

        // Use this for initialization
        public TestObjectClass(string name, int value1, float value2) {
            this.name = name;
            this.value1 = value1;
            this.value2 = value2;
        }

        public string ToString() {
            return string.Format("name={0} value1={1} = value2={2}", name, value1, value2);
        }
    }
}