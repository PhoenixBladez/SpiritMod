using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Pet;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class CaltfistPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Cultfish");
            Description.SetDefault("This little bugger lights the way!");
			Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetSpiritPlayer().caltfist = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Caltfist>()] <= 0;
			if(petProjectileNotSpawned && player.whoAmI == Main.myPlayer) {
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Caltfist>(), 0, 0f, player.whoAmI);
			}
		}
	}
}