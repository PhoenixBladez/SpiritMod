using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_ArmorGuide : Book
    {
        public override string BookText => "[c/95a3ab:The following is a transcript of an informational speech made by renowned adventurer Aurelius Finch, Knight-Crusader.]\n\n---\n\n" +
            "Hey, folks! Are you ready to become a grand adventurer, just like myself? It seems like a good life up here, and it is! I'm rolling in sponsorship money, and my dating life has never been better!" +
            " But things weren't always this way. I was just like you all, once- dirt poor and owning nothing but the clothes on my back. It took everything I had to survive. But if you purchase my handy series of guides, you'll be just like me in no-time-flat! I can't wait for you to rise through the ranks, rookie.\n\n" +
            "[c/80deed:The Beginner's Guide to Armor]\n\n" +
            "So you're just setting out, and it's time to make your mark on the world. But what to wear? Armor is the most important tool in an adventurer's arsenal. Luckily for you, I've come across many different options for beginners that are easy to forge and effective." +
            "[i:" + SpiritMod.Instance.ItemType("StoneHead") + "] [i:" + SpiritMod.Instance.ItemType("StoneBody") + "] [i:" + SpiritMod.Instance.ItemType("StoneLegs") + "][c/80deed: Stone Armor:] Stone Armor is cheap and reliable! Just cobble together some [c/72a4c2:stone] at a workbench and you've got a strong, bulky set of armor. It does look quite ugly, though, but that isn't a big deal. After all, you're not famous yet!\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("DaybloomHead") + "] [i:" + SpiritMod.Instance.ItemType("DaybloomBody") + "] [i:" + SpiritMod.Instance.ItemType("DaybloomLegs") + "][c/80deed: Sunflower Armor:] Are you an aspiring mage? Well, I'm not- haven't got a magical bone in my body! But this thin and comfortable set of armor, made from [c/72a4c2:Sunflowers] and [c/72a4c2:Fallen Stars], can give a boost to your magic power.\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("LeatherHood") + "] [i:" + SpiritMod.Instance.ItemType("LeatherPlate") + "] [i:" + SpiritMod.Instance.ItemType("LeatherLegs") + "][c/80deed: Marskman's Armor:] If you're willing to venture out at night and take on some zombies, harvest some questionably sourced [c/72a4c2:Old Leather] and try your hand at archery. This lightweight (but smelly) armor allows you to focus on your strikes.\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("SilkHood") + "] [i:" + SpiritMod.Instance.ItemType("SilkScarf") + "] [i:" + SpiritMod.Instance.ItemType("SilkRobe") + "] [i:" + SpiritMod.Instance.ItemType("SilkLegs") + "][c/80deed: Manasilk Armor:] I despise summoned creatures! The buggers always seem to dislike me, and I can't figure out why. I'm a great guy! But if you're looking for a way to summon more familiars, try this exotic armor made of [c/72a4c2:Silk] and [c/72a4c2:Gold or Platinum Bars]. If you can afford it, that is.\n\n" +
            "Those are some cheap and effective armor sets that'll keep you fighting for a long time. Best of luck, adventurer!\n\n---\n\n" +
            "Whew. Thank god that's over. These suckers won't last a day out there, but they'll believe anything I tell them. Easiest money I've made in my life. Hey, why are you still writing this dow-";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Beginner's Guide to Armor");
            Tooltip.SetDefault("by Knight-Crusader Aurelius");
        }
    }
}