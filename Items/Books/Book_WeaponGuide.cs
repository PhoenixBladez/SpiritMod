using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_WeaponGuide : Book
    {
        public override string BookText => "[c/95a3ab:The following is a transcript of an informational speech made by adventurer Aurelius Finch, Knight-Crusader.]\n\n---\n\n" +
            "Hey, all! Knight-Crusader Aurelius here, again. Before we get started (and on a completely unrelated note), my last guide on Armor didn't sell as well as I want to, and I have no clue why! But I need YOUR money to support my lavish hero lifestyle. So buy this new weapon guide for your dad, your grandma, and maybe even your cat. Please? I promise it'll be useful!\n\n" +
            "[c/80deed:The Beginner's Guide to Weaponry]\n\n" +
            "Aside from a strong set of armor, you'll need a trusty weapon by your side to really stand out as an adventurer. Now, not all of us can have a weapon that's as flashy as my trusted fireblade. And some of you never will! But that's okay. Let's start small, with weapons that are reliable and easy to forge.\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("WoodenClub") + "] [i:9] [c/80deed: Wooden Club:] You'll definitely need some arm strength for this one. Throw together some [c/72a4c2:wood] at a workbench to make this ugly but effective club. I have an image to maintain, so I'd never be caught dead using this thing. But it makes for a great crowd-control tool!\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("IcySpear") + "] [i:" + SpiritMod.Instance.ItemType("FrigidFragment") + "] [c/80deed: Frigid Javelin:] Do you live close to a snowy tundra or boreal forest? Then this weapon is perfect for you! Simply harvest some [c/72a4c2:Frigid Fragments] and temper them at an anvil to make this ice-cold, razor sharp weapon. Unlike other inaccurate throwing weapons (who uses those, anyway?), you can aim for precise strikes with javelins!\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("BismiteSummonStaff") + "] [i:" + SpiritMod.Instance.ItemType("BismiteCrystal") + "] [c/80deed: Bismite Crystal Staff:] Normally, I wouldn't recommend summoned creatures, since they love to bite me and rip up my things. But you won't have that problem with a semi-sentient poisonous rock, that's for sure. Made of [c/72a4c2:Bismite Shards], it's sure to help deal with enemies for you.\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("JellyfishStaff") + "] [i:275] [c/80deed: Jellyfish Staff:] Again, familiars and I don't get along, but this one has no teeth or claws! I suggest this little electrical buddy because you may not be strong enough to fight on your own, like I am. But keep trying, and you're maybe probably not going to get to my level! Until then, piece together this shockingly effective weapon from [c/72a4c2:Coral], [c/72a4c2:Starfish], and [c/72a4c2:Glowsticks].\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("Florang") + "] [i:" + SpiritMod.Instance.ItemType("FloranBar") + "] [c/80deed: Floran Cutter:] Now, if you want a sleek, cool, and potent weapon, look no further. The caveat is that you'll need to venture into the depths of the dark Briar to harvest [c/72a4c2:Floran Bars]. Are you capable of doing that? Probably not! But if you do manage to luck out, throw these sharp disks along the ground and watch them cut up your foes.\n\n" +
			"[i:" + SpiritMod.Instance.ItemType("CactusStaff") + "] [i:276] [c/80deed: Cactus Staff:] Would you ever think that [c/72a4c2:cacti] would have magical properties? I sure wouldn't! The only interaction I've had with cacti is falling straight into a pit of pricklethorns when I was a novice adventurer, like you. But I've been told that Cactus Staves are quite powerful and poisonous.\n\n" +
            "---\n\n" +
            "I'm barely breaking even anymore. These pieces of junk better tide me over so I can afford that solid gold statue I commissioned. Well, the people still love me. I'm sure they'll buy my book this time. Hey, why are you still here? The speech is over. Shoo!";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Beginner's Guide to Weaponry");
            Tooltip.SetDefault("by Knight-Crusader Aurelius");
        }
    }
}