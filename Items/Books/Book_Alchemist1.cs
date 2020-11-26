using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Potion;
namespace SpiritMod.Items.Books
{
    public class Book_Alchemist1 : Book
    {
		public override string BookText =>  "After experimenting with ingredients from all around this world, I've decided to detail some of the more interesting concoctions that I've produced.\n" +
            "[i:" + SpiritMod.Instance.ItemType("BismitePotion") + "] [c/72a4c2: Toxin Potion:] I believe I made this nasty brew from a combination of a poisonous [c/95a3ab:Noxophyll] and some of those strange metallic [c/95a3ab:Bismite Crystals]. The poisons cancelled each other out and I was left with this foul-tasting, but drinkable, substance that proves to be quite potent in battle.\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("MushroomPotion") + "] [c/72a4c2: Sporecoid Potion:] After obtaining a rare sample of [c/95a3ab:Glowroot] from a band of field researchers, I mixed it in with [c/95a3ab:Glowing Mushrooms] to produce this. The field researchers told me that they were never going back to that 'freaky place', but that I could retrieve more Glowroot by cutting down giant mushroom trees. I must investigate soon.\n\n" +
            "[i:" + SpiritMod.Instance.ItemType("PinkPotion") + "] [c/72a4c2: Jump Potion:] It was one of those days where I decided to throw stuff in a pot at random to see what would happen. Who would've thought that [c/95a3ab:Pink Gel] and [c/95a3ab:Daybloom] would cause me to feel so giddy! After downing this sweet stuff, I was jumping for joy- and jumping much higher than I normally would, too.";

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("An Alchemist's Observations");
            Tooltip.SetDefault("by Lena Ashwood, Alchemist");
        }
	}
}
