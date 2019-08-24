using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class GhastBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ghast Wisp");
			Description.SetDefault("'An ethereal power'");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("Ghast")] > 0)
			{
				modPlayer.Ghast = true;
			}
			if (!modPlayer.Ghast)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			player.buffTime[buffIndex] = 18000;
		}
	}
}
