#if !UNITY_ZEROPLAYER
using System;
using System.Diagnostics;
using Unity.Assertions;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
    // We have defined three fixed-size NativeStrings, all of which are value types with zero allocation.
    // You can copy them freely without ever generating garbage or needing to Dispose, but they are limited in size.
    //
    // NativeString64 - consumes 64 bytes (one line) of memory. suitable for short names and descriptions.
    // NativeString512 - consumes 512 bytes (eight lines) of memory. can hold a few lines of text, a filename, a URL.
    // NativeString4096 - consumes 4096 bytes (one page) of memory. can hold a printed page of text.
    // 
    // These names are not very friendly, we might want to change them to, for example:
    //
    // NativeStringName
    // NativeStringLine
    // NativeStringPage
    //
    // There is also maybe a need for NativeString? which has a thread safety handle and calls malloc and requires
    // Dispose()? But, you have to wonder for what purpose. Text larger than 4096 bytes is probably a JSON file or
    // something like that - something you process offline or outside of gameplay. C# String and managed code might
    // be OK for that. Or, you could just use a NativeArray<char> for the file, and NativeString512 for the little
    // parseable pieces inside the file, since you wouldn't expect any of them to exceed 512 bytes. It seems fair
    // to limit token sizes to 512 bytes, no reasonable person would need tokens longer than that.
    //
    // The horrible waste of NativeString512 isn't so bad, if you consider:
    //
    // 1. These are almost certainly going to be on the stack most of the time
    // 2. The stack is hot in the page cache
    // 3. Since memory access is by 64 byte cache line, short strings only access first 64 byte cache line of 512

    internal struct NativeString
    {
        public static unsafe int CompareTo(char *a, int aa, char* b, int bb)
        {
            int chars = aa < bb ? aa : bb;
            for (var i = 0; i < chars; ++i)
            {
                if (a[i] < b[i])
                    return -1;
                if (a[i] > b[i])
                    return 1;
            }
            if (aa < bb)
                return -1;
            if (aa > bb)
                return 1;
            return 0;            
        }        
        public static unsafe bool Equals(char *a, int aa, char* b, int bb)
        {
            if (aa != bb)
                return false;
            return UnsafeUtility.MemCmp(a, b, aa * sizeof(char)) == 0;
        }        
    }
    
    public struct NativeString64 : IComparable<NativeString64>, IEquatable<NativeString64>
    {
        public const int MaxLength = (64 - sizeof(int)) / sizeof(char);
        public int Length;
        unsafe fixed char buffer[MaxLength];

        public unsafe char* GetUnsafePtr()
        {
            fixed(char *b = buffer)
                return b;
        }
        
        unsafe void CopyFrom(char* s, int length)
        {
            Assert.IsTrue(length <= MaxLength);
            Length = length;
            fixed (char* d = buffer)
                UnsafeUtility.MemCpy(d, s, length * sizeof(char));
        }
        
        public NativeString64(String source)
        {
            Length = 0;
            unsafe
            {
                fixed(char *s = source)
                    CopyFrom(s, source.Length);
            }
        }
        
        public NativeString64(ref NativeString512 source)
        {
            Length = 0;
            unsafe
            {
                CopyFrom(source.GetUnsafePtr(), source.Length);
            }
        }

        public NativeString64(ref NativeString4096 source)
        {
            Length = 0;
            unsafe
            {
                CopyFrom(source.GetUnsafePtr(), source.Length);
            }
        }
        
        public char this[int index]
        {
            get
            {
                Assert.IsTrue(index >= 0 && index < Length);
                unsafe
                {
                    fixed (char* c = buffer)
                        return c[index];
                }
            }
            set
            {
                Assert.IsTrue(index >= 0 && index < Length);
                unsafe
                {
                    fixed (char* c = buffer)
                        c[index] = value;
                }
            }
        }
        public override String ToString()
        {
            unsafe
            {
                fixed (char* c = buffer)
                {
                    char[] temp = new char[Length];
                    for (var i = 0; i < Length; ++i)
                        temp[i] = c[i];
                    return new String(c, 0, Length);
                }
            }
        }
        public override int GetHashCode()
        {
            unsafe
            {
                fixed (char* c = buffer)
                    return (int) math.hash(c, Length * sizeof(char));
            }
        }

        
        public int CompareTo(NativeString64 other)
        {
            unsafe
            {
                fixed(char *c = buffer)
                    return NativeString.CompareTo(c, Length, other.buffer, other.Length);
            }
        }

        public bool Equals(NativeString64 other)
        {
            unsafe
            {
                fixed (char* c = buffer)
                    return NativeString.Equals(c, Length, other.buffer, other.Length);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is NativeString64 other && Equals(other);
        }
    }

    public struct NativeString512 : IComparable<NativeString512>, IEquatable<NativeString512>
    {
        public const int MaxLength = (512 - sizeof(int)) / 2;
        public int Length;
        unsafe fixed char buffer[MaxLength];

        public unsafe char* GetUnsafePtr()
        {
            fixed(char *b = buffer)
                return b;
        }
        
        unsafe void CopyFrom(char* s, int length)
        {
            Assert.IsTrue(length <= MaxLength);
            Length = length;
            fixed (char* d = buffer)
                UnsafeUtility.MemCpy(d, s, length * sizeof(char));
        }
        
        public NativeString512(String source)
        {
            Length = 0;
            unsafe
            {
                fixed(char *s = source)
                    CopyFrom(s, source.Length);
            }
        }
        
        public NativeString512(ref NativeString64 source)
        {
            Length = 0;
            unsafe
            {
                CopyFrom(source.GetUnsafePtr(), source.Length);
            }
        }

        public NativeString512(ref NativeString4096 source)
        {
            Length = 0;
            unsafe
            {
                CopyFrom(source.GetUnsafePtr(), source.Length);
            }
        }
        
        public char this[int index]
        {
            get
            {
                Assert.IsTrue(index >= 0 && index < Length);
                unsafe
                {
                    fixed (char* c = buffer)
                        return c[index];
                }
            }
            set
            {
                Assert.IsTrue(index >= 0 && index < Length);
                unsafe
                {
                    fixed (char* c = buffer)
                        c[index] = value;
                }
            }
        }
        public override String ToString()
        {
            unsafe
            {
                fixed (char* c = buffer)
                    return new String(c, 0, Length);
            }
        }
        public override int GetHashCode()
        {
            unsafe
            {
                fixed (char* c = buffer)
                    return (int) math.hash(c, Length * sizeof(char));
            }
        }
       
        public int CompareTo(NativeString512 other)
        {
            unsafe
            {
                fixed(char* b = buffer)
                    return NativeString.CompareTo(b, Length, other.buffer, other.Length);
            }
        }

        public bool Equals(NativeString512 other)
        {
            unsafe
            {
                fixed (char* c = buffer)
                    return NativeString.Equals(c, Length, other.buffer, other.Length);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is NativeString512 other && Equals(other);
        }
    }

    public struct NativeString4096 : IComparable<NativeString4096>, IEquatable<NativeString4096>
    {
        public const int MaxLength = (4096 - sizeof(int)) / 2;
        public int Length;
        unsafe fixed char buffer[MaxLength];

        public unsafe char* GetUnsafePtr()
        {
            fixed(char *b = buffer)
                return b;
        }
        
        unsafe void CopyFrom(char* s, int length)
        {
            Assert.IsTrue(length <= MaxLength);
            Length = length;
            fixed (char* d = buffer)
                UnsafeUtility.MemCpy(d, s, length * sizeof(char));
        }
        
        public NativeString4096(String source)
        {
            Length = 0;
            unsafe
            {
                fixed(char *s = source)
                    CopyFrom(s, source.Length);
            }
        }
        
        public NativeString4096(ref NativeString64 source)
        {
            Length = 0;
            unsafe
            {
                CopyFrom(source.GetUnsafePtr(), source.Length);
            }
        }

        public NativeString4096(ref NativeString512 source)
        {
            Length = 0;
            unsafe
            {
                CopyFrom(source.GetUnsafePtr(), source.Length);
            }
        }
        
        public char this[int index]
        {
            get
            {
                Assert.IsTrue(index >= 0 && index < Length);
                unsafe
                {
                    fixed (char* c = buffer)
                        return c[index];
                }
            }
            set
            {
                Assert.IsTrue(index >= 0 && index < Length);
                unsafe
                {
                    fixed (char* c = buffer)
                        c[index] = value;
                }
            }
        }
        public override String ToString()
        {
            unsafe
            {
                fixed (char* c = buffer)
                    return new String(c, 0, Length);
            }
        }
        public override int GetHashCode()
        {
            unsafe
            {
                fixed (char* c = buffer)
                    return (int) math.hash(c, Length * sizeof(char));
            }
        }
        
        public int CompareTo(NativeString4096 other)
        {
            unsafe
            {
                fixed(char* b = buffer)
                    return NativeString.CompareTo(b, Length, other.buffer, other.Length);
            }
        }

        public bool Equals(NativeString4096 other)
        {
            unsafe
            {
                fixed (char* c = buffer)
                    return NativeString.Equals(c, Length, other.buffer, other.Length);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is NativeString4096 other && Equals(other);
        }
    }
    
    // A "NativeStringView" does not manage its own memory - it expects some other object to manage its memory
    // on its behalf.        
    
    public struct NativeStringView
    {
        unsafe char* pointer;
        int length;
        public unsafe NativeStringView(char* p, int l)
        {
            pointer = p;
            length = l;
        }

        public unsafe char this[int index]
        {
            get => UnsafeUtility.ReadArrayElement<char>(pointer, index);
            set => UnsafeUtility.WriteArrayElement<char>(pointer, index, value);
        }        
        public int Length => length;
        public override String ToString()
        {
            unsafe
            {
                return new String(pointer, 0, length);                
            }
        }

        public override int GetHashCode()
        {
            unsafe
            {
                return (int)math.hash(pointer, Length * sizeof(char));                
            }            
        }
    }
           
    sealed class WordStorageDebugView
    {
        WordStorage m_wordStorage;

        public WordStorageDebugView(WordStorage wordStorage)
        {
            m_wordStorage = wordStorage;
        }
        
        public NativeStringView[] Table
        {
            get
            {
                var table = new NativeStringView[m_wordStorage.Entries];
                for (var i = 0; i < m_wordStorage.Entries; ++i)
                    table[i] = m_wordStorage.GetNativeStringView(i);
                return table;
            }
        }
    }
    
    [DebuggerTypeProxy(typeof(WordStorageDebugView))]
    public class WordStorage : IDisposable
    {        
        private NativeArray<ushort> buffer; // all the UTF-16 encoded bytes in one place
        private NativeArray<int> offset; // one offset for each text in "buffer"
        private NativeArray<ushort> length; // one length for each text in "buffer"
        private NativeMultiHashMap<int,int> hash; // from string hash to table entry
        private int chars; // bytes in buffer allocated so far
        private int entries; // number of strings allocated so far
        static WordStorage _Instance;

        public static WordStorage Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new WordStorage();
                return _Instance;
            }
            set { _Instance = value; }
        }

        const int kMaxEntries = 10000;
        const int kMaxChars = kMaxEntries * 100;

        public const int kMaxCharsPerEntry = 4096;
        
        public int Entries => entries;
        
        void Initialize()
        {
            buffer = new NativeArray<ushort>(kMaxChars, Allocator.Persistent);
            offset = new NativeArray<int>(kMaxEntries, Allocator.Persistent);
            length = new NativeArray<ushort>(kMaxEntries, Allocator.Persistent);
            hash = new NativeMultiHashMap<int,int>(kMaxEntries, Allocator.Persistent);
            chars = 0;
            entries = 0;
            GetOrCreateIndex(new NativeStringView()); // make sure that Index=0 means empty string
        }
        WordStorage()
        {
            Initialize();
        }
        public static void Setup()
        {
            if(Instance.buffer.Length > 0)
                Instance.Dispose();
            Instance.Initialize();
        }
        
        public unsafe NativeStringView GetNativeStringView(int index)
        {
            Assert.IsTrue(index < entries);
            var o = offset[index];
            var l = length[index];
            Assert.IsTrue(l <= kMaxCharsPerEntry);
            return new NativeStringView((char*)buffer.GetUnsafePtr() + o, l);
        }
        
        public int GetIndex(int h, NativeStringView temp)
        {
            Assert.IsTrue(temp.Length <= kMaxCharsPerEntry); // about one printed page of text
            int itemIndex;
            NativeMultiHashMapIterator<int> iter;
            if (hash.TryGetFirstValue(h, out itemIndex, out iter))
            {
                var l = length[itemIndex];
                Assert.IsTrue(l <= kMaxCharsPerEntry);
                if (l == temp.Length)
                {
                    var o = offset[itemIndex];
                    int matches;
                    for(matches = 0; matches < l; ++matches)
                        if (temp[matches] != buffer[o + matches])
                            break;
                    if (matches == temp.Length)
                        return itemIndex;

                }
            } while (hash.TryGetNextValue(out itemIndex, ref iter));
            return -1;            
        }

        public bool Contains(NativeStringView value)
        {            
            int h = value.GetHashCode();
            return GetIndex(h, value) != -1;
        }

        public unsafe bool Contains(String value)
        {
            fixed(char *c = value)
                return Contains(new NativeStringView(c, value.Length));
        }

        public int GetOrCreateIndex(NativeStringView value)
        {
            int h = value.GetHashCode();
            var itemIndex = GetIndex(h, value);
            if (itemIndex != -1)
                return itemIndex;
            Assert.IsTrue(entries < kMaxEntries);
            Assert.IsTrue(chars + value.Length <= kMaxChars);
            var o = chars;
            var l = (ushort)value.Length;
            for (var i = 0; i < l; ++i)
                buffer[chars++] = value[i];
            offset[entries] = o;
            length[entries] = l;
            hash.Add(h, entries);
            return entries++;
        }
        
        public void Dispose()
        {
            buffer.Dispose();
            offset.Dispose();
            length.Dispose();
            hash.Dispose();
        }
    }

    // A "Words" is an integer that refers to 4,096 or fewer chars of UTF-16 text in a global storage blob.
    // Each should refer to *at most* about one printed page of text.
    // If you need more text, consider using one Words struct for each printed page's worth.
    // If you need to store the text of "War and Peace" in a single object, you've come to the wrong place.
    
    public struct Words
    {
        private int Index;     
        public NativeStringView ToNativeStringView()
        {
            return WordStorage.Instance.GetNativeStringView(Index);
        }
        public override String ToString()
        {
            return WordStorage.Instance.GetNativeStringView(Index).ToString();
        }
        public unsafe void SetString(String value)
        {
            fixed(char *c = value)
                Index = WordStorage.Instance.GetOrCreateIndex(new NativeStringView(c, value.Length));            
        }
    }

    // A "NumberedWords" is a "Words", plus possibly a string of leading zeroes, followed by
    // possibly a positive integer.
    // The zeroes and integer aren't stored centrally as a string, they're stored as an int.
    // Therefore, 1,000,000 items with names from FooBarBazBifBoo000000 to FooBarBazBifBoo999999
    // Will cost 8MB + a single copy of "FooBarBazBifBoo", instead of ~48MB. 
    // They say that this is a thing, too.
    
    public struct NumberedWords
    {
        private int Index;
        private int Suffix;
        
        private const int kPositiveNumericSuffixShift = 0;
        private const int kPositiveNumericSuffixBits = 29;
        private const int kMaxPositiveNumericSuffix = (1 << kPositiveNumericSuffixBits) - 1;
        private const int kPositiveNumericSuffixMask = (1 << kPositiveNumericSuffixBits) - 1;

        private const int kLeadingZeroesShift = 29;
        private const int kLeadingZeroesBits = 3;
        private const int kMaxLeadingZeroes = (1 << kLeadingZeroesBits) - 1;
        private const int kLeadingZeroesMask = (1 << kLeadingZeroesBits) - 1;
        
        private int LeadingZeroes
        {
            get => (Suffix >> kLeadingZeroesShift) & kLeadingZeroesMask;
            set
            {
                Suffix &= ~(kLeadingZeroesMask << kLeadingZeroesShift);
                Suffix |= (value & kLeadingZeroesMask) << kLeadingZeroesShift;
            }
        }

        private int PositiveNumericSuffix
        {
            get => (Suffix >> kPositiveNumericSuffixShift) & kPositiveNumericSuffixMask;
            set
            {
                Suffix &= ~(kPositiveNumericSuffixMask << kPositiveNumericSuffixShift);
                Suffix |= (value & kPositiveNumericSuffixMask) << kPositiveNumericSuffixShift;
            }
        }

        bool HasPositiveNumericSuffix => PositiveNumericSuffix != 0;
        
        public override String ToString()
        {
            String temp = WordStorage.Instance.GetNativeStringView(Index).ToString();
            var leadingZeroes = LeadingZeroes;
            if (leadingZeroes > 0)
                temp += new String('0', leadingZeroes);
            if (HasPositiveNumericSuffix)
                temp += PositiveNumericSuffix;
            return temp;
        }
        public unsafe void SetString(String value)
        {
            int beginningOfDigits = value.Length;

            // as long as there are digits at the end,
            // look back for more digits.

            while (beginningOfDigits > 0 && Char.IsDigit(value[beginningOfDigits - 1]))
                --beginningOfDigits;

            // as long as the first digit is a zero, it's not the beginning of the positive integer - it's a leading zero.
            
            var beginningOfPositiveNumericSuffix = beginningOfDigits;
            while (beginningOfPositiveNumericSuffix < value.Length && value[beginningOfPositiveNumericSuffix] == '0')
                ++beginningOfPositiveNumericSuffix;

            // now we know where the leading zeroes begin, and then where the positive integer begins after them.
            // but if there are too many leading zeroes to encode, the excess ones become part of the string.
            
            var leadingZeroes = beginningOfPositiveNumericSuffix - beginningOfDigits;
            if (leadingZeroes > kMaxLeadingZeroes)
            {
                var excessLeadingZeroes = leadingZeroes - kMaxLeadingZeroes;
                beginningOfDigits += excessLeadingZeroes;
                leadingZeroes -= excessLeadingZeroes;
            }
                        
            // if there is a positive integer after the zeroes, here's where we compute it and store it for later.

            PositiveNumericSuffix = 0;
            {
                int number = 0;
                for (var i = beginningOfPositiveNumericSuffix; i < value.Length; ++i)
                {
                    number *= 10;
                    number += value[i] - '0';
                }
                
                // an intrepid user may attempt to encode a positive integer with 20 digits or something.
                // they are rewarded with a string that is encoded wholesale without any optimizations.
                
                if(number <= kMaxPositiveNumericSuffix)
                    PositiveNumericSuffix = number; 
                else
                {
                    beginningOfDigits = value.Length; 
                    leadingZeroes = 0; // and your dog Toto, too.
                }
            }

            // set the leading zero count in the Suffix member.
            
            LeadingZeroes = leadingZeroes;

            // truncate the string, if there were digits at the end that we encoded.
            
            if(beginningOfDigits != value.Length)
                value = value.Substring(0, beginningOfDigits);

            // finally, set the string to its index in the global string blob thing.

            fixed(char *c = value)
                Index = WordStorage.Instance.GetOrCreateIndex(new NativeStringView(c, value.Length));      
        }
    }
}
#endif
