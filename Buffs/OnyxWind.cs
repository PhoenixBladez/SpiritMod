using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class OnyxWind : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Onyx Whirlwind");
			Description.SetDefault("Increases movement speed by 10%");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxRunSpeed += .1f;
			{

				Dust.NewDust(player.position, player.width, player.height, DustID.GoldCoin);
			}
		}
	}
}
