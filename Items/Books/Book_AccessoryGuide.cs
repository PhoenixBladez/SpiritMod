using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_AccessoryGuide : Book
    {
        public override string BookText => "[c/95a3ab:The following is a transcript of a speech made by questionable adventurer Aurelius Finch, former Knight-Crusader.]\n\n---\n\n" +
            "Aurelius here. Things aren't going too hot for me. The people were 'disappointed by my superiority complex' and 'excessive, almost comical indulgence'. I was stripped of my titles and- after going bankrupt- all my assets were seized. I've fallen into a pretty bad crowd now, and I'm just getting this book out there because I'm being strongarmed into endorsing their products. Is this an emotional plea to get your sympathy? Of course not!" +
            " I still care about making sure you all sucee... who am I kidding? Let's get this over with.\n\n" +
            "[c/80deed:The Beginner's Guide to Accessories]\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("AceOfClubs") + "] [i:" + SpiritMod.Instance.ItemType("AceOfDiamonds") + "] [i:" + SpiritMod.Instance.ItemType("AceOfSpades") + "] [i:" + SpiritMod.Instance.ItemType("AceOfHearts") + "] [c/80deed: Ace Cards:] Like I said, I've been approached by some pretty shady folks, and I've had to get acquainted with some dubious accessories lately. Aces are useful tools if you can land critical hits on foes. You can find them if you run across [c/72a4c2:the Gambler] when travelling. She sells different variations based on the moon phase. And if you see her, please tell her I sent you? Please?\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("AssassinMagazine") + "] [c/80deed: Assassin's Magazine:] How low have I fallen? I can't believe I'm talking about items used by the same mercenaries I used to fight against. But that [c/72a4c2:Bandit] has eyes everywhere. If you're a long-range fighter like he is, this magazine should help you switch your ammo types quickly. I feel so much shame right now.\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("ArcaneNecklace") + "] [c/80deed: Arcane Necklace:] You know what? I'm not going to live like this anymore. I hate who I've become. It's time for me to take my own advice and start adventuring again for the people I swore to protect. No more shady equipment. Instead, if you're a mage and have some [c/72a4c2:Fallen Stars] and [c/72a4c2:chains] lying around, make this pendant to fortify your magic attacks.\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("LeatherShield") + "] [i:" + SpiritMod.Instance.ItemType("LeatherGlove") + "] [i:" + SpiritMod.Instance.ItemType("LeatherBoots") + "][c/80deed: Leather Equipment:] I guess you and I are on the same page now. I no longer have my enchanted gear, so I'm using these pieces of equipment made from [c/72a4c2:Old Leather]. Yes, they're ugly. Yes, they smell. But they'll keep us safe and light out there.\n\n" +
            "It's time for me to return to my roots. To slay monsters and fight for good again. If you've read all my books, I really do hope they help you and me both. Maybe I'll see you out there.\n\n---\n\n" +
            "[c/95a3ab:Editor's Note: The night after this transcription, Aurelius Finch vanished, leaving only a note behind. In it, he promised to become worthy once again by journeying the world and defeating monsters. His current whereabouts are unknown.]";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Beginner's Guide to Accessories");
            Tooltip.SetDefault("by (former) Knight-Crusader Aurelius");
        }
    }
}