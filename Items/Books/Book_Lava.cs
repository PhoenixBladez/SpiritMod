using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_Lava : Book
    {
        public override string BookText => "Today's a very exciting day; it's the first time \nI've been authorized to do independent field research! I'm really excited to test out my unique hypothesis: Is lava hot? I mean, how do we know? No one I've ever talked to has touched or even seen lava. But I'm here in a deep cavern, and I'm going to make my mark on the scientific community today! I see some lava over there, so I'm going to touch it. WOW, that is hot! We're making real discoveries here! My left hand is all gone, and it really, really hurts! I'll try my right hand just to be sure and report back my findings. [c/ff0000:_--_--+--=&&^^ ^&& ^&____/~~~\\_-]";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Is Lava Hot?");
            Tooltip.SetDefault("by Field Researcher Ocano");
        }
    }
}