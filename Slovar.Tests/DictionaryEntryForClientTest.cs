using Slovar.Models;
using Slovar;
using NUnit.Framework;

public class DictionaryEntryForClientTestClass
{
    [Test]
    public void DictionaryEntryForClient()
    {
        var entry = new DictionaryEntry()
        {
            Lemma = "abc",
            Definition = "def"
        };
        var entryForClient = new DictionaryEntryForClient(entry);
        Assert.AreEqual(entryForClient.Lemma, entry.Lemma);
        Assert.AreEqual(entryForClient.Usages, entry.Usages);
        Assert.AreEqual(entryForClient.Translation, entry.Translation);
        Assert.AreEqual(entryForClient.StressIndex, entry.StressIndex);
    }
}