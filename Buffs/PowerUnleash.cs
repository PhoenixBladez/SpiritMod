using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class PowerUnleash : ModBuff
	{

		int timer = 0;

		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Power Unleash");
			Description.SetDefault("Powers up the Darkfire Katana");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 1.20f;
			{
				int dust = Dust.NewDust(player.position, player.width, player.height, 61);
			}
			Lighting.AddLight(player.position, 0.0f, 1.2f, 0.0f);
		}
	}
}