using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_FoodGuide : Book
    {
        public override string BookText => "Despite the lack of cravings in this place, that won't stop us gettin' absolutely chockers, ey? Oddly enough, full course meals are a regular thing to find in most dead buggers, fresh and hot too - tell that to ya closest mate and they'll think you're off ya chops! Sane or not, I've noted down some sources for ya, so get ya runners on and your magical sword of death and destruction in your favorite hand, ready for a good hunt.\n\n" +
                "[i:" + SpiritMod.Instance.Find<ModItem>("Hummus").Type + "][c/80deed: Hummus:]  Travelling the desert unprepared could lead some shifty bandits to jump ya! Fortunately, it wasn't me. Unfortunately, it was some other wandering bloke. Dealin' with them proper rewarded me with a bowl of 'hummus' from their wrappings remains - that's what the turban bloke calls it, anyway. Think he sells dye or sum'n. Goes well on any sanger!\n\n"+
				"[i:" + SpiritMod.Instance.Find<ModItem>("Meatballs").Type + "][c/80deed: Meatballs:] This place is rather cute compared to my home, smell included. Although, can't say I expected an angry rotting meatball to gun me down. After kindly telling it to rack off with my knife, its interiors revealed a fresh plate of meatballs. The irony couldn't be higher. After debating it, I gave it a chomp and it seemed solid enough, so you don't have to worry about spending your day on the dunny after having some.\n\n"+
                "[i:" + SpiritMod.Instance.Find<ModItem>("Popsicle").Type + "][c/80deed: Popsicle:] Going to the tundra during a storm is not a great vacation plan, especially when the locals are dissocial and probably want you dead. One entered the cave I snuggled in, and frankly never left. Bored, an idea popped into my head and a skinner in my hand. I gave one of its toothpicks some good whacks and it magically turned into a popsicle after hitting the floor with a poof, stick and all! Sadly it melted quick near my campfire, along with the buggers' remains and my mood.";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meals On Wheels");
            Tooltip.SetDefault("by Explorer Trev Irwin");
        }
    }
}