using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
    public class CogSpiderPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Star Spider");
			Description.SetDefault("'The stars will give you solace'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetSpiritPlayer().starPet = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<CogSpiderPet>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<CogSpiderPet>(), 0, 0f, player.whoAmI);
			}
		}
	}
}