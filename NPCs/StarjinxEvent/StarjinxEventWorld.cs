using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;

namespace SpiritMod.NPCs.StarjinxEvent
{
    public class StarjinxEventWorld : ModWorld
    {
        public static bool StarjinxActive = false;
        public static bool SpawnedStarjinx = false;

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
	}
}