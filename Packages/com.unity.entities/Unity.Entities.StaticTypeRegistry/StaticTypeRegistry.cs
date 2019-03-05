using System;

namespace Unity.Entities.StaticTypeRegistry
{
    static internal unsafe class StaticTypeRegistry
    {
        static public readonly Type[] Types;
        static public readonly int[] EntityOffsets;
        // This field will be generated in the replacement assembly
        //static public readonly TypeManager.TypeInfo[] TypeInfos;

        public static void RegisterStaticTypes() {
            // empty -- dynamic reg is used.  TypeRegGen will generate
            // a replacement assembly
            throw new NotImplementedException("This function should have been replaced by the TypeRegGen build step. Ensure TypeRegGen.exe is generating a new Unity.Entities.StaticTypeRegistry assembly.");
        }

        public static bool Equals(void* lhs, void* rhs, int typeIndex)
        {
            // empty -- dynamic reg is used.  TypeRegGen will generate
            // a replacement assembly
            throw new NotImplementedException("This function should have been replaced by the TypeRegGen build step. Ensure TypeRegGen.exe is generating a new Unity.Entities.StaticTypeRegistry assembly.");
        }

        public static int GetHashCode(void* val, int typeIndex)
        {
            // empty -- dynamic reg is used.  TypeRegGen will generate
            // a replacement assembly
            throw new NotImplementedException("This function should have been replaced by the TypeRegGen build step. Ensure TypeRegGen.exe is generating a new Unity.Entities.StaticTypeRegistry assembly.");
        }
    }
}
