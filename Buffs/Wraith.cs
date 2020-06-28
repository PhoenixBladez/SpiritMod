using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Wraith : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Wraith");
			Description.SetDefault("You are almost invulnerable and speedy");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxRunSpeed += 0.55f;
			player.jumpSpeedBoost += 3f;
			player.longInvince = true;

			Dust.NewDust(player.position, player.width, player.height, 109);
			Dust.NewDust(player.position, player.width, player.height, 109);
		}
	}
}
