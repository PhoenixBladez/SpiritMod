using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_Briar : Book
    {
        public override string BookText => "Our three month expedition into the scarcely explored Briar came to an end two weeks ago. The team consisted of I, Dr. Nisha Laywatts, Dr. Rathi Everest, and a hired adventurer whose name is unknown. Dr. Everest and I completed the expedition unharmed, but the adventurer went missing four days before the excursion was complete. We tried searching for him to no avail. What is more pertinent to this book, however, is the data we have accrued.\n\n---\n\n" +
            "[c/7c9e5a:Climate and Ecology]\n\nThe Briar presents one of the most unique climate systems in the world: the entire region undergoes perpetual rainfall. Temperatures are warm and humid, and plant life is overgrown. So overgrown, in fact, that a giant root system seems to run across and under the entire ecosystem. The reason for this continued rainfall is unknown, but I hypothesize that some megaflora (or perhaps even megafauna) at the center of this root system exerts a form of arcane power over the area that causes this torrential downpour.\n\n---\n\n" +
            "[i:" + SpiritMod.Instance.Find<ModItem>("BriarFlowerItem").Type + "] [c/7c9e5a:Flora] [i:" + SpiritMod.Instance.Find<ModItem>("BriarFlowerItem").Type + "]\n\nAs mentioned earlier, a complex and extensive root system spans the entire Briar. Data shows that every individual plant is connected via this expanse. This connection lets all plants in the Briar to share nutrients with one another, allowing for plant growth to occur far below the surface. Interestingly, plants in the Briar all display some form of bioluminescence. The underlying cause behind this is unknown, but I suspect that plants emit this alluring light to attract prey for the carnivorous beasts of the Briar. In turn, they absorb some of the detritus and use it for nutrition.\n\n---\n\n" +
            "[i:" + SpiritMod.Instance.Find<ModItem>("RawMeat").Type + "] [c/7c9e5a:Fauna] [i:" + SpiritMod.Instance.Find<ModItem>("RawMeat").Type + "]\n\nThe Briar is dominated by feral, low-intelligence bipeds and quadrapeds. Every faunal organism in the Briar is carnivorous yet composed of a large percentage of plant matter. A few bipeds seem to exhibit higher intelligence and wield magic, but their origin and habits could not be discerned. All organisms in the briar are hyper-aggressive and should be approached with extreme caution.\n\n---\n\n" +
            "[c/7c9e5a:Anomalies]\n\nThe underground Briar displays signs of inhabitation by some now-extinct civilization. Ruined brick and wood houses can be observed along with primitive weapons, tools, and trinkets. Many mysteries remain about these ruins and the Briar at large. Where did it come from? Why is it so hostile? And where are the people that once lived here?\n\n---\n\n" +
            "Note: It seems as though a page of notes and diagrams is missing from the book. I probably lost it back in the depths of the Briar. Not worth a return journey, though. ";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Notes on the Briar");
            Tooltip.SetDefault("by Field Researcher Laywatts");
        }
    }
}