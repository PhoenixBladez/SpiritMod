using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_Jellyfish : Book
    {
        public override string BookText => "I'm not going insane, I promise! I was climbing up my favorite hill a few nights back and I swear I saw 'em! A school of glowing [c/72a4c2:bright blue jellyfish] darting across the sky! They were flying through the air like it was water, and I thought my time was up. Instead of preparing for my inevitable alien abduction, I scrammed! By the time I ran back down, I couldn't see 'em anymore. And no one I've talked to believes me! Why not? Because I tried sellin' them painted rocks disguised as gems? Bunch of babies. Stuff's not right, I'm telling you!\n\nWhile you're readin', can I interest you in some 100% genuine diamonds?";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jellyfish in the Sky!");
            Tooltip.SetDefault("by Jonah the Knave");
        }
    }
}