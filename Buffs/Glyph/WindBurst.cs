using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Glyph
{
	public class WindBurst : ModBuff
	{
		public static int _type;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Wind Burst");
			Description.SetDefault("Knockback is amplified");
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().stormBurst = true;

			if (Main.rand.NextDouble() < 0.1)
			{
				Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, Dusts.Wind._type);
				dust.customData = new Dusts.WindAnchor(npc.Center, dust.position);
			}
		}
	}
}
