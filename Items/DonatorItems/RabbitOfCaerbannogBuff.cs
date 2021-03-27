using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class RabbitOfCaerbannogBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Rabbit of Caerbannog");
			Description.SetDefault("'It's just a harmless little bunny, isn't it?'");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<RabbitOfCaerbannog>()] > 0) {
				modPlayer.rabbitMinion = true;
				player.buffTime[buffIndex] = 18000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}
