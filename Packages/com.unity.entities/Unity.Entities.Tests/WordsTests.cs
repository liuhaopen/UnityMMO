using System;
using NUnit.Framework;

namespace Unity.Entities.Tests
{       
	public class WordsTests	
	{
	    [SetUp]
	    public virtual void Setup()
	    {
	        WordStorage.Setup();
	    }

	    [TearDown]
	    public virtual void TearDown()
	    {
	    }

        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("green")]
        [TestCase("blue")]
        [TestCase("indigo")]
        [TestCase("violet")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("绿色")]
        [TestCase("蓝色")]
        [TestCase("靛蓝色")]
        [TestCase("紫罗兰色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("James Madison")]
        [TestCase("James Monroe")]
        [TestCase("John Quincy Adams")]
        [TestCase("Andrew Jackson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        [TestCase("大江健三郎")]
        [TestCase("川端 康成")]
        [TestCase("桐野夏生")]
        [TestCase("芥川龍之介")]
        public void NativeString64ToStringWorks(String a)
        {
            NativeString64 aa = new NativeString64(a);
            Assert.AreEqual(aa.ToString(), a);
        }
        
        [TestCase("monkey", "monkey")]
        [TestCase("red","orange")]
        [TestCase("yellow","green")]
        [TestCase("blue", "indigo")]
        [TestCase("violet","紅色")]
        [TestCase("橙色","黄色")]
        [TestCase("绿色","蓝色")]
        [TestCase("靛蓝色","紫罗兰色")]
        [TestCase("George Washington","John Adams")]
        [TestCase("Thomas Jefferson","James Madison")]
        [TestCase("James Monroe","John Quincy Adams")]
        [TestCase("Andrew Jackson","村上春樹")]
        [TestCase("三島 由紀夫","吉本ばなな")]
        [TestCase("大江健三郎","川端 康成")]
        [TestCase("桐野夏生","芥川龍之介")]
        public void NativeString64EqualsWorks(String a, String b)
        {
            NativeString64 aa = new NativeString64(a);
            NativeString64 bb = new NativeString64(b);
            Assert.AreEqual(aa.Equals(bb), a.Equals(b));
        }
        
        [TestCase("monkey", "monkey")]
        [TestCase("red","orange")]
        [TestCase("yellow","green")]
        [TestCase("blue", "indigo")]
        [TestCase("violet","紅色")]
        [TestCase("橙色","黄色")]
        [TestCase("绿色","蓝色")]
        [TestCase("靛蓝色","紫罗兰色")]
        [TestCase("George Washington","John Adams")]
        [TestCase("Thomas Jefferson","James Madison")]
        [TestCase("James Monroe","John Quincy Adams")]
        [TestCase("Andrew Jackson","村上春樹")]
        [TestCase("三島 由紀夫","吉本ばなな")]
        [TestCase("大江健三郎","川端 康成")]
        [TestCase("桐野夏生","芥川龍之介")]
        public void NativeString64CompareToWorks(String a, String b)
        {
            NativeString64 aa = new NativeString64(a);
            NativeString64 bb = new NativeString64(b);
            var c0 = aa.CompareTo(bb);
            var c1 = a.CompareTo(b);
            Assert.AreEqual(c0, c1);
        }

        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("green")]
        [TestCase("blue")]
        [TestCase("indigo")]
        [TestCase("violet")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("绿色")]
        [TestCase("蓝色")]
        [TestCase("靛蓝色")]
        [TestCase("紫罗兰色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("James Madison")]
        [TestCase("James Monroe")]
        [TestCase("John Quincy Adams")]
        [TestCase("Andrew Jackson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        [TestCase("大江健三郎")]
        [TestCase("川端 康成")]
        [TestCase("桐野夏生")]
        [TestCase("芥川龍之介")]
        public void NativeString512ToStringWorks(String a)
        {
            NativeString512 aa = new NativeString512(a);
            Assert.AreEqual(aa.ToString(), a);
        }
        
        [TestCase("monkey", "monkey")]
        [TestCase("red","orange")]
        [TestCase("yellow","green")]
        [TestCase("blue", "indigo")]
        [TestCase("violet","紅色")]
        [TestCase("橙色","黄色")]
        [TestCase("绿色","蓝色")]
        [TestCase("靛蓝色","紫罗兰色")]
        [TestCase("George Washington","John Adams")]
        [TestCase("Thomas Jefferson","James Madison")]
        [TestCase("James Monroe","John Quincy Adams")]
        [TestCase("Andrew Jackson","村上春樹")]
        [TestCase("三島 由紀夫","吉本ばなな")]
        [TestCase("大江健三郎","川端 康成")]
        [TestCase("桐野夏生","芥川龍之介")]
        public void NativeString512EqualsWorks(String a, String b)
        {
            NativeString512 aa = new NativeString512(a);
            NativeString512 bb = new NativeString512(b);
            Assert.AreEqual(aa.Equals(bb), a.Equals(b));
        }
        
        [TestCase("monkey", "monkey")]
        [TestCase("red","orange")]
        [TestCase("yellow","green")]
        [TestCase("blue", "indigo")]
        [TestCase("violet","紅色")]
        [TestCase("橙色","黄色")]
        [TestCase("绿色","蓝色")]
        [TestCase("靛蓝色","紫罗兰色")]
        [TestCase("George Washington","John Adams")]
        [TestCase("Thomas Jefferson","James Madison")]
        [TestCase("James Monroe","John Quincy Adams")]
        [TestCase("Andrew Jackson","村上春樹")]
        [TestCase("三島 由紀夫","吉本ばなな")]
        [TestCase("大江健三郎","川端 康成")]
        [TestCase("桐野夏生","芥川龍之介")]
        public void NativeString512CompareToWorks(String a, String b)
        {
            NativeString512 aa = new NativeString512(a);
            NativeString512 bb = new NativeString512(b);
            Assert.AreEqual(aa.CompareTo(bb), a.CompareTo(b));
        }

        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("green")]
        [TestCase("blue")]
        [TestCase("indigo")]
        [TestCase("violet")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("绿色")]
        [TestCase("蓝色")]
        [TestCase("靛蓝色")]
        [TestCase("紫罗兰色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("James Madison")]
        [TestCase("James Monroe")]
        [TestCase("John Quincy Adams")]
        [TestCase("Andrew Jackson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        [TestCase("大江健三郎")]
        [TestCase("川端 康成")]
        [TestCase("桐野夏生")]
        [TestCase("芥川龍之介")]
        public void NativeString4096ToStringWorks(String a)
        {
            NativeString4096 aa = new NativeString4096(a);
            Assert.AreEqual(aa.ToString(), a);
        }
        
        [TestCase("monkey", "monkey")]
        [TestCase("red","orange")]
        [TestCase("yellow","green")]
        [TestCase("blue", "indigo")]
        [TestCase("violet","紅色")]
        [TestCase("橙色","黄色")]
        [TestCase("绿色","蓝色")]
        [TestCase("靛蓝色","紫罗兰色")]
        [TestCase("George Washington","John Adams")]
        [TestCase("Thomas Jefferson","James Madison")]
        [TestCase("James Monroe","John Quincy Adams")]
        [TestCase("Andrew Jackson","村上春樹")]
        [TestCase("三島 由紀夫","吉本ばなな")]
        [TestCase("大江健三郎","川端 康成")]
        [TestCase("桐野夏生","芥川龍之介")]
        public void NativeString4096EqualsWorks(String a, String b)
        {
            NativeString4096 aa = new NativeString4096(a);
            NativeString4096 bb = new NativeString4096(b);
            Assert.AreEqual(aa.Equals(bb), a.Equals(b));
        }
        
        [TestCase("monkey", "monkey")]
        [TestCase("red","orange")]
        [TestCase("yellow","green")]
        [TestCase("blue", "indigo")]
        [TestCase("violet","紅色")]
        [TestCase("橙色","黄色")]
        [TestCase("绿色","蓝色")]
        [TestCase("靛蓝色","紫罗兰色")]
        [TestCase("George Washington","John Adams")]
        [TestCase("Thomas Jefferson","James Madison")]
        [TestCase("James Monroe","John Quincy Adams")]
        [TestCase("Andrew Jackson","村上春樹")]
        [TestCase("三島 由紀夫","吉本ばなな")]
        [TestCase("大江健三郎","川端 康成")]
        [TestCase("桐野夏生","芥川龍之介")]
        public void NativeString4096CompareToWorks(String a, String b)
        {
            NativeString4096 aa = new NativeString4096(a);
            NativeString4096 bb = new NativeString4096(b);
            Assert.AreEqual(aa.CompareTo(bb), a.CompareTo(b));
        }

        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        public void NativeString512ToNativeString64Works(String a)
        {
            var b = new NativeString512(a);
            var c = new NativeString64(ref b);
            String d = c.ToString();
            Assert.AreEqual(a, d);
        }
        
        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        public void NativeString4096ToNativeString64Works(String a)
        {
            var b = new NativeString4096(a);
            var c = new NativeString64(ref b);
            String d = c.ToString();
            Assert.AreEqual(a, d);
        }
        
        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        public void NativeString64ToNativeString512Works(String a)
        {
            var b = new NativeString64(a);
            var c = new NativeString512(ref b);
            String d = c.ToString();
            Assert.AreEqual(a, d);
        }
        
        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        public void NativeString4096ToNativeString512Works(String a)
        {
            var b = new NativeString4096(a);
            var c = new NativeString512(ref b);
            String d = c.ToString();
            Assert.AreEqual(a, d);
        }

        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        public void NativeString64ToNativeString4096Works(String a)
        {
            var b = new NativeString64(a);
            var c = new NativeString4096(ref b);
            String d = c.ToString();
            Assert.AreEqual(a, d);
        }
        
        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        public void NativeString512ToNativeString4096Works(String a)
        {
            var b = new NativeString512(a);
            var c = new NativeString4096(ref b);
            String d = c.ToString();
            Assert.AreEqual(a, d);
        }
        
        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("green")]
        [TestCase("blue")]
        [TestCase("indigo")]
        [TestCase("violet")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("绿色")]
        [TestCase("蓝色")]
        [TestCase("靛蓝色")]
        [TestCase("紫罗兰色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("James Madison")]
        [TestCase("James Monroe")]
        [TestCase("John Quincy Adams")]
        [TestCase("Andrew Jackson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        [TestCase("大江健三郎")]
        [TestCase("川端 康成")]
        [TestCase("桐野夏生")]
        [TestCase("芥川龍之介")]
        [TestCase("로마는 하루아침에 이루어진 것이 아니다")]
        [TestCase("낮말은 새가 듣고 밤말은 쥐가 듣는다")]
        [TestCase("말을 냇가에 끌고 갈 수는 있어도 억지로 물을 먹일 수는 없다")]
        [TestCase("호랑이에게 물려가도 정신만 차리면 산다")]
        [TestCase("Үнэн үг хэлсэн хүнд ноёд өстэй, үхэр унасан хүнд ноход өстэй.")]
        [TestCase("Өнгөрсөн борооны хойноос эсгий нөмрөх.")]
        [TestCase("Барын сүүл байснаас батганы толгой байсан нь дээр.")]
        [TestCase("Гараар ганц хүнийг дийлэх. Tолгойгоор мянган хүнийг дийлэх.")]
        [TestCase("Աղւէսը բերանը խաղողին չի հասնում, ասում է՝ խակ է")]
        [TestCase("Ամեն փայտ շերեփ չի դառնա, ամեն սար՝ Մասիս")]
        [TestCase("Արևին ասում է դուրս մի արի՝ ես դուրս եմ եկել")]
        [TestCase("Գայլի գլխին Աւետարան են կարդում, ասում է՝ շուտ արէ՛ք, գալլէս գնաց")]
        [TestCase("पृथिव्यां त्रीणी रत्नानि जलमन्नं सुभाषितम्।")]
        [TestCase("जननी जन्मभुमिस्छ स्वर्गादपि गरीयसि")]
        [TestCase("न अभिशेको न संस्कारः सिम्हस्य कृयते वनेविक्रमार्जितसत्वस्य स्वयमेव मृगेन्द्रता")]
        public void WordsWorks(String value)
        {
            Words s = new Words();
            s.SetString(value);
            Assert.AreEqual(s.ToString(), value);
        }

        [TestCase("red")]
        [TestCase("orange")]
        [TestCase("yellow")]
        [TestCase("green")]
        [TestCase("blue")]
        [TestCase("indigo")]
        [TestCase("violet")]
        [TestCase("紅色")]
        [TestCase("橙色")]
        [TestCase("黄色")]
        [TestCase("绿色")]
        [TestCase("蓝色")]
        [TestCase("靛蓝色")]
        [TestCase("紫罗兰色")]
        [TestCase("George Washington")]
        [TestCase("John Adams")]
        [TestCase("Thomas Jefferson")]
        [TestCase("James Madison")]
        [TestCase("James Monroe")]
        [TestCase("John Quincy Adams")]
        [TestCase("Andrew Jackson")]
        [TestCase("村上春樹")]
        [TestCase("三島 由紀夫")]
        [TestCase("吉本ばなな")]
        [TestCase("大江健三郎")]
        [TestCase("川端 康成")]
        [TestCase("桐野夏生")]
        [TestCase("芥川龍之介")]
        [TestCase("로마는 하루아침에 이루어진 것이 아니다")]
        [TestCase("낮말은 새가 듣고 밤말은 쥐가 듣는다")]
        [TestCase("말을 냇가에 끌고 갈 수는 있어도 억지로 물을 먹일 수는 없다")]
        [TestCase("호랑이에게 물려가도 정신만 차리면 산다")]
        [TestCase("Үнэн үг хэлсэн хүнд ноёд өстэй, үхэр унасан хүнд ноход өстэй.")]
        [TestCase("Өнгөрсөн борооны хойноос эсгий нөмрөх.")]
        [TestCase("Барын сүүл байснаас батганы толгой байсан нь дээр.")]
        [TestCase("Гараар ганц хүнийг дийлэх. Tолгойгоор мянган хүнийг дийлэх.")]
        [TestCase("Աղւէսը բերանը խաղողին չի հասնում, ասում է՝ խակ է")]
        [TestCase("Ամեն փայտ շերեփ չի դառնա, ամեն սար՝ Մասիս")]
        [TestCase("Արևին ասում է դուրս մի արի՝ ես դուրս եմ եկել")]
        [TestCase("Գայլի գլխին Աւետարան են կարդում, ասում է՝ շուտ արէ՛ք, գալլէս գնաց")]
        [TestCase("पृथिव्यां त्रीणी रत्नानि जलमन्नं सुभाषितम्।")]
        [TestCase("जननी जन्मभुमिस्छ स्वर्गादपि गरीयसि")]
        [TestCase("न अभिशेको न संस्कारः सिम्हस्य कृयते वनेविक्रमार्जितसत्वस्य स्वयमेव मृगेन्द्रता")]
	    public void AddWorks(String value)
	    {
	        Words w = new Words();
            Assert.IsFalse(WordStorage.Instance.Contains(value));
	        Assert.IsTrue(WordStorage.Instance.Entries == 1);
	        w.SetString(value);	        
	        Assert.IsTrue(WordStorage.Instance.Contains(value));
	        Assert.IsTrue(WordStorage.Instance.Entries == 2);
	    }

	    [TestCase("red")]
	    [TestCase("orange")]
	    [TestCase("yellow")]
	    [TestCase("green")]
	    [TestCase("blue")]
	    [TestCase("indigo")]
	    [TestCase("violet")]
	    [TestCase("紅色")]
	    [TestCase("橙色")]
	    [TestCase("黄色")]
	    [TestCase("绿色")]
	    [TestCase("蓝色")]
	    [TestCase("靛蓝色")]
	    [TestCase("紫罗兰色")]
	    [TestCase("로마는 하루아침에 이루어진 것이 아니다")]
	    [TestCase("낮말은 새가 듣고 밤말은 쥐가 듣는다")]
	    [TestCase("말을 냇가에 끌고 갈 수는 있어도 억지로 물을 먹일 수는 없다")]
	    [TestCase("호랑이에게 물려가도 정신만 차리면 산다")]
	    public void NumberedWordsWorks(String value)
	    {
	        NumberedWords w = new NumberedWords();
	        Assert.IsTrue(WordStorage.Instance.Entries == 1);
	        for (var i = 0; i < 100; ++i)
	        {
	            w.SetString( value + i);
	            Assert.IsTrue(WordStorage.Instance.Entries == 2);
	        }	        
	    }
	}
}
