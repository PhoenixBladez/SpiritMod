using Terraria;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class GuidingLight : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Guiding Light");
			Description.SetDefault("You emit a faint glow");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{

			Lighting.AddLight(player.Center, Color.Yellow.ToVector3() / 2.5f);
		}
	}
}
