using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_MJW : Book
    {
        public override string BookText => "Either those boomer shroomies I had for brekky hit too hard or I'm way too buggered, but [c/72a4c2:some jellyfish with a hat and robe] is flying around tossin' wildlife in a bubble while havin' a giggle. To think the stingers in the bloody water ain't enough, this one's got the hand of a wizard and the heart of a dolphin. Stirrer's been causin' all sorts of trouble overnight, and I ain't got the tools to deal with 'em yet. Wonder how space stingers taste over a barby.";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dag Dinger");
            Tooltip.SetDefault("by Explorer Trev Irwin");
        }
    }
}