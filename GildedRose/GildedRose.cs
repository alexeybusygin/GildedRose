using System;
using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose
{
    IList<Item> Items;

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    public void UpdateQuality()
    {
        static int KeepQualityBounds(int quality) => Math.Min(Math.Max(quality, 0), 50);

        static void ChangeQualityAction(Item item, int speed)
        {
            item.SellIn--;
            item.Quality = item.SellIn >= 0 ? item.Quality - speed : item.Quality - speed * 2;

            item.Quality = KeepQualityBounds(item.Quality);
        }

        static void BackstageAction(Item item)
        {
            item.SellIn--;
            if (item.SellIn < 0)
                item.Quality = 0;
            else if (item.SellIn < 5)
                item.Quality += 3;
            else if (item.SellIn < 10)
                item.Quality += 2;
            else
                item.Quality++;

            item.Quality = KeepQualityBounds(item.Quality);
        }

        static void MakeNoAction(Item item) { }

        for (var i = 0; i < Items.Count; i++)
        {
            Action<Item> updateAction = Items[i].Name switch
            {
                "Aged Brie"                                 => item => ChangeQualityAction(item, speed: -1),
                "Conjured Mana Cake"                        => item => ChangeQualityAction(item, speed: 2),
                "Sulfuras, Hand of Ragnaros"                => MakeNoAction,
                "Backstage passes to a TAFKAL80ETC concert" => BackstageAction,
                _                                           => item => ChangeQualityAction(item, speed: 1)
            };

            updateAction(Items[i]);
        }
    }
}