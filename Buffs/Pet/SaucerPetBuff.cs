using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class SaucerPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Support Saucer");
			Description.SetDefault("'It seems to only provide moral support...'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetSpiritPlayer().saucerPet = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<SaucerPet>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<SaucerPet>(), 0, 0f, player.whoAmI);
			}
		}
	}
}