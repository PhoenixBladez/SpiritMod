using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Pet;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class SwordPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Possessed Blade");
			Description.SetDefault("'Is this a dagger I see in front of me?'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetSpiritPlayer().SwordPet = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<SwordPet>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer) {
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<SwordPet>(), 0, 0f, player.whoAmI);
			}
		}
	}
}