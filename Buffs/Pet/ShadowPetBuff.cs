using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Pet;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class ShadowPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Shadow Pup");
			Description.SetDefault("'Awww'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetSpiritPlayer().shadowPet = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<ShadowPet>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer) {
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<ShadowPet>(), 0, 0f, player.whoAmI);
			}
		}
	}
}