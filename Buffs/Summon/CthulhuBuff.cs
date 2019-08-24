using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class CthulhuBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mini R'lyehian");
			Description.SetDefault("It speaks in a strange, arcane language");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("Cthulhu")] > 0)
			{
				modPlayer.cthulhuMinion = true;
			}
			if (!modPlayer.cthulhuMinion)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			player.buffTime[buffIndex] = 18000;
		}
	}
}
