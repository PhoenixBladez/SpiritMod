using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    public class Book_Slime : Book
    {
		public override string BookText =>  "They come from goo.\n\nThat's about it. At least, that's where conventional research ends. But as an accomplished mage, I must delve deeper!\n" +
            "First, it is important to take stock of what we know. Slimes are, scientifically speaking, big blobs of goo. They are highly flammable, possess no internal organs, and yet seem quite intelligent.\n" +
            "Their composition matches no other material my fellow mages and I have ever seen. Where then, do they come from? Could a rival mage be summoning them? And to what devious end?! Are they a punishment from the gods themselves? Are we forever trapped in this bouncy, slimy purgatory?\n\n" +
            "And most importantly- are they edible? We must keep searching for answers.";

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Where Do Slimes Come From?");
            Tooltip.SetDefault("by Sorcerer Galen Arcturus");
        }
	}
}
