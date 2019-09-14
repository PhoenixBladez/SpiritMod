using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
    public class CaltfistPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Cultfish");
			Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetSpiritPlayer().caltfist = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("Caltfist")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("Caltfist"), 0, 0f, player.whoAmI);
			}
		}
	}
}