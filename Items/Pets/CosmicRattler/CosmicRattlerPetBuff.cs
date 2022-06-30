using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Pet;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pets.CosmicRattler
{
	public class CosmicRattlerPetBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starachnid");
			Description.SetDefault("'Inside it you can see the depths of space'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetSpiritPlayer().starachnidPet = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<CosmicRattlerPet>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer) {
				Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), player.Center, Vector2.Zero, ModContent.ProjectileType<CosmicRattlerPet>(), 0, 0f, player.whoAmI);
			}
		}
	}
}