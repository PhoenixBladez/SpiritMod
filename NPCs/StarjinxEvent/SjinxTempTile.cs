using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	class SjinxTempTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			AddMapEntry(new Color(173, 216, 230));
			dustType = DustID.Water_Space;
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (!NPC.AnyNPCs(ModContent.NPCType<StarjinxMeteorite>()))
				WorldGen.KillTile(i, j);
		}
	}
}
