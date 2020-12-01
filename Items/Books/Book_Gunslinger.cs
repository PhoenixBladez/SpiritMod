using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_Gunslinger : Book
    {
        public override string BookText => "I saw him walk through town yesterday. For some days now townsfolk have been talking about the lone gunslinger who only goes where the earth takes him. Sounded like a bunch of garbage to me. Nobody really knew what he did, only ever been seen for a few moments at a time like some sort of cryptid. One of the old fellas at the saloon recalled having spoken to someone with a similar look years prior. He was apparently a gambling man, kept to himself most of the time. Another claimed he came over a hill to see this gunslinger take out a pack of antlions. Said his iron was so powerful it caused those buggers to explode like a stick of dynamite. Still, it all felt like a whole lot of ghost stories until that moment I saw him. His revolver was marked with some symbol from a playing card. Instead of a hat, he wore a tattered cloak with mysterious lettering on the back. He saw me and gave a slow nod, before continuing on his path.";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Gunslinger");
            Tooltip.SetDefault("by Usov Lynx");
        }
    }
}