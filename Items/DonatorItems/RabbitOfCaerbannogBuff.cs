using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	class RabbitOfCaerbannogBuff : ModBuff
	{
		
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Rabbit of Caerbannog");
			Description.SetDefault("'It's just a harmless little bunny, isn't it⸮'");
			//Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 10;
			bool petNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<RabbitOfCaerbannog>()] <= 0;
			if(petNotSpawned && player.whoAmI == Main.myPlayer) {
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<RabbitOfCaerbannog>(), 0, 0f, player.whoAmI);
			}
		}
	}
}
