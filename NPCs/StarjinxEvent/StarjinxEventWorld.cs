using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;

namespace SpiritMod.NPCs.StarjinxEvent
{
    public class StarjinxEventWorld : ModWorld
    {
        public static bool StarjinxActive = false;
        public static bool SpawnedStarjinx = false;

        public override void PostUpdate()
        {
            if (Main.rand.Next(5) == 0 && !NPC.AnyNPCs(ModContent.NPCType<StarjinxMeteorite>()))
            {
                Main.NewText("An enchanted comet has appeared in the asteroid field!", 252, 150, 255);
                NPC.NewNPC(1800, 1800, ModContent.NPCType<StarjinxMeteorite>());
                SpawnedStarjinx = true;
			}
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"Starjinx Active?", StarjinxActive}
            };
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("Starjinx Active?"))
                StarjinxActive = tag.GetBool("Starjinx Active?");
            SpawnedStarjinx = false;
        }

		/*public override void Initialize()
		{
            SpiritMod.InitStargoop();
		}*/
	}
}