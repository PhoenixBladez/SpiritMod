using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Armor
{
	public class StellarSpeed : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Stellar Speed");
			Description.SetDefault("Greatly boosts speed");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += .15f;
			player.maxRunSpeed += .18f;
			Dust.NewDustPerfect(new Vector2(player.position.X + Main.rand.Next(player.width), player.position.Y + player.height - Main.rand.Next(7)), DustID.GoldCoin, Vector2.Zero);

		}
	}
}
