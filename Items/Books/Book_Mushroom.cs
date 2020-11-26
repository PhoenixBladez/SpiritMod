using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_Mushroom : Book
    {
        public override string BookText => "As my team and I rested up after a close encounter with those zombified [c/72a4c2:fungal botanists] the day before, I finally managed to find the sickly specimens that had been following us. I awoke to the sound of recurrent coughing somewhere in my camp, and I soon traced it to a pair of [c/72a4c2:one-eyed quadrupeds] that were scrounging around our storage tent. The darned things had a clear affinity for mushrooms, evidenced by how they had consumed almost ALL the specimens we had gathered in the glowing mushroom fields. I was lucky to chase the creatures away with little resistance.\n\nI informed Dr.Cho about it earlier this morning, and we've gathered the remains of what fungal specimens we had left in order lure the sickly creatures back to us. I'll report again once we've captured one of them. This could be an exciting discovery of an undocumented monster!";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fungal Pilferers");
            Tooltip.SetDefault("by Dr. Charlie Maitake");
        }
    }
}