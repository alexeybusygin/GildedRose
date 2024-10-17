using GildedRoseKata;
using NUnit.Framework;
using System.Collections.Generic;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [TestCase(10, 20, 3, 7, 17)]
    [TestCase(10, 20, 10, 0, 10)]
    [TestCase(10, 20, 12, -2, 6)]       // Decrease 2x after sell-by date
    [TestCase(10, 20, 30, -20, 0)]      // Can't be negative
    public void RegularItem_DecreasesQualityNormally(int sellIn, int quality, int days, int expectedSellIn, int expectedQuality) =>
        TestItem("Foo", sellIn, quality, days, expectedSellIn, expectedQuality);

    [TestCase(10, 20, 3, 7, 23)]
    [TestCase(10, 20, 10, 0, 30)]
    [TestCase(10, 20, 12, -2, 34)]      // Increase 2x after sell-by date
    [TestCase(10, 20, 45, -35, 50)]     // Can't be greater than 50
    public void AgedBrieTest_IncreasesQuality(int sellIn, int quality, int days, int expectedSellIn, int expectedQuality) =>
        TestItem("Aged Brie", sellIn, quality, days, expectedSellIn, expectedQuality);

    [TestCase(10, 80, 3, 10, 80)]       // Nothing changes along the time
    [TestCase(10, 80, 10, 10, 80)]
    [TestCase(10, 80, 45, 10, 80)]
    public void SulfurasTest_StaysTheSame(int sellIn, int quality, int days, int expectedSellIn, int expectedQuality) =>
        TestItem("Sulfuras, Hand of Ragnaros", sellIn, quality, days, expectedSellIn, expectedQuality);

    [TestCase(15, 5, 4, 11, 9)]         // Increase 1x per day
    [TestCase(15, 5, 6, 9, 12)]         // Increase 2x per day if 10 days or less
    [TestCase(15, 5, 12, 3, 26)]        // Increase 3x per day if 5 days or less
    [TestCase(15, 5, 18, -3, 0)]        // Quality drops to 0 after the concert
    public void BackstagePassesTest_IncreasesAndThenDrops(int sellIn, int quality, int days, int expectedSellIn, int expectedQuality) =>
        TestItem("Backstage passes to a TAFKAL80ETC concert", sellIn, quality, days, expectedSellIn, expectedQuality);

    [TestCase(10, 40, 3, 7, 34)]        // Decrease 2x per day
    [TestCase(10, 40, 5, 5, 30)]
    [TestCase(10, 40, 12, -2, 12)]      // Decrease 4x after sell-by date
    [TestCase(10, 40, 30, -20, 0)]      // Can't be negative
    public void ConjuredItem_DecreasesQuality2x(int sellIn, int quality, int days, int expectedSellIn, int expectedQuality) =>
        TestItem("Conjured Mana Cake", sellIn, quality, days, expectedSellIn, expectedQuality);

    [Test]
    public void IgnoreNullEntriesTest()
    {
        var items = new List<Item> { null };

        var app = new GildedRose(items);
        Assert.DoesNotThrow(app.UpdateQuality);
    }

    private static void TestItem(string name, int sellIn, int quality, int days, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new() { Name = name, SellIn = sellIn, Quality = quality } };

        var app = new GildedRose(items);
        for (var i = 0; i < days; i++)
            app.UpdateQuality();

        Assert.Multiple(() =>
        {
            Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
            Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
        });
    }
}