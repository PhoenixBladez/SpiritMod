using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_Yeremy : Book
    {
        public override string BookText => "I'm furious. This was supposed to be my masterpiece! I really put my heart and soul into this manuscript, and it got shot down instantly. It was supposed to be a gritty drama about a hero who transforms into a villain. Here's my favorite part:" +
            "\n[c/b5a493:Yeremy, the Rainforest Dictator]\n[c/b5a493:And so, after witnessing his sixth home village be destroyed by evil necromancers, Yeremy decided that enough was enough. He was really mad this time. He was going to exact vengeance on all living things... And he'd start by summoning his accursed demon avatar, a hydra straight from the underworld. The only thoughts that ran through his mind were 'kill, kill, kill...']" +
            "\nBut NOOOOO. My editor says it was 'too asinine' and 'so hyperbolic that I stopped at page three.' Well, his loss! I'm sure I'll find someone that'll like this manuscript.";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("I'm Really Mad");
            Tooltip.SetDefault("by Polius Crassus");
        }
    }
}