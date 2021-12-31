using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Mechanics.Coverings
{
    public class CoveringsModWorld : ModWorld
    {
        public override void Load(TagCompound tag)
        {
			if (!tag.ContainsKey("SpiritMod:Coverings")) return;

            byte[] array = tag.Get<byte[]>("SpiritMod:Coverings");

            using (var memStream = new MemoryStream(array))
            {
                using (var reader = new BinaryReader(memStream))
                {
					SpiritMod.Coverings.LoadWorld(reader);
                }
            }
        }

        public override TagCompound Save()
        {
            var compound = new TagCompound();

            using (var memStream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStream))
                {
                    SpiritMod.Coverings.SaveWorld(writer);
                }

                byte[] array = memStream.ToArray();
                compound.Set("SpiritMod:Coverings", array);
            }

            return compound;
        }
    }
}
