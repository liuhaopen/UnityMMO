using Unity.Collections;

namespace Unity.Entities
{
    internal static class CalculateReaderWriterDependency
    {
        public static bool Add(ComponentType type, NativeList<int> reading, NativeList<int> writing)
        {
            if (!type.RequiresJobDependency)
                return false;

            if (type.AccessModeType == ComponentType.AccessMode.ReadOnly)
            {
                if (reading.Contains(type.TypeIndex))
                    return false;
                if (writing.Contains(type.TypeIndex))
                    return false;

                reading.Add(type.TypeIndex);
                return true;
            }
            else
            {
                if (writing.Contains(type.TypeIndex))
                    return false;

                var readingIndex = reading.IndexOf(type.TypeIndex);
                if (readingIndex != -1)
                    reading.RemoveAtSwapBack(readingIndex);

                writing.Add(type.TypeIndex);
                return true;
            }
        }
    }
}
