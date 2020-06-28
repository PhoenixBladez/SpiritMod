using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class PowerUnleash : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Power Unleash");
			Description.SetDefault("Powers up the Darkfire Katana");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 1.20f;

			Dust.NewDust(player.position, player.width, player.height, 61);
			Lighting.AddLight(player.position, 0.0f, 1.2f, 0.0f);
		}
	}
}