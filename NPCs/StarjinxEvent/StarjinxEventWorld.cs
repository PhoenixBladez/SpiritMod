using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using SpiritMod.NPCs.StarjinxEvent;

namespace SpiritMod.NPCs.StarjinxEvent
{
    public class StarjinxEventWorld : ModWorld
    {
        public static bool StarjinxActive = false;
        public static bool SpawnedStarjinx = false;

        public override void PostUpdate()
        {
            if (StarjinxActive && !SpawnedStarjinx && !NPC.AnyNPCs(ModContent.NPCType<StarjinxMeteorite>()))
            {
                NPC.NewNPC(1500, 1500, ModContent.NPCType<StarjinxMeteorite>());
                SpawnedStarjinx = true;
            }
            if (!StarjinxActive && Main.rand.Next(300) == 0)
            {
                Main.NewText("An enchanted comet has appeared in the asteroid field!", 252, 150, 255);
                StarjinxActive = true;
                SpawnedStarjinx = false;
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

		public override void PostDrawTiles()
		{
			base.PostDrawTiles();
		}

		/*public override void Initialize()
		{
            SpiritMod.InitStargoop();
		}*/
	}
}