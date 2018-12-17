
using NUnit.Framework;

namespace Unity.Entities.Editor.Tests
{
    class GenericClassTest<T>
    {
        public class InternalClass {}
        public class InternalGenericClass<U, V> {}
    }
    
    public class ComponentGroupGUITests
    {
        
        [Test]
        public void ComponentGroupGUI_SpecifiedTypeName_NestedTypeInGeneric()
        {
            var typeName = ComponentGroupGUI.SpecifiedTypeName(typeof(GenericClassTest<object>.InternalClass));
            Assert.AreEqual("GenericClassTest<Object>.InternalClass", typeName);
        }
        
        [Test]
        public void ComponentGroupGUI_SpecifiedTypeName_NestedGenericTypeInGeneric()
        {
            var typeName = ComponentGroupGUI.SpecifiedTypeName(typeof(GenericClassTest<object>.InternalGenericClass<int, bool>));
            Assert.AreEqual("GenericClassTest<Object>.InternalGenericClass<Int32, Boolean>", typeName);
        }
    }
}