using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Glyph
{
	public class BurningRage : ModBuff
	{
		public static int _type;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Burning Rage");
			Description.SetDefault("+17% damage and attack speed");
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<MyPlayer>().blazeBurn = true;
			player.meleeDamage += .17f;
			player.rangedDamage += .17f;
			player.magicDamage += .17f;
			player.minionDamage += .17f;
			player.thrownDamage += .17f;

			if (Main.rand.NextDouble() < .5)
			{
				int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Fire);
				Main.dust[dust].scale = Main.rand.NextFloat(1.4f, 2.4f);
				Main.dust[dust].velocity.Y += Main.rand.NextFloat(0, -2f);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
