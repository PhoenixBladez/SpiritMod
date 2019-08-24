using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Glyph
{
	public class PhantomVeil : ModBuff
	{
		public static int _type;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Phantom Veil");
			Description.SetDefault("The next attack will be blocked");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 8;

			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (modPlayer.glyph == GlyphType.Veil)
				player.buffTime[buffIndex] = 2;
			else
				player.buffTime[buffIndex] = 0;
		}
	}
}
