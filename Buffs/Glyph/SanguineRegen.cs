using Microsoft.Xna.Framework;
using SpiritMod.NPCs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
	public class SanguineRegen : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sanguine Regeneration");
			Description.SetDefault("You are rapidly gaining blood.");
			Main.buffNoSave[Type] = true;
		}
		public override bool ReApply(Player player, int time, int buffIndex)
		{
			player.buffTime[buffIndex] = Math.Min(player.buffTime[buffIndex] + time, 600);
			return false;
		}
		public override void Update(Player player, ref int buffIndex) => player.lifeRegen += 4;
	}
}
