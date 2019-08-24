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
			player.GetModPlayer<MyPlayer>(mod).shadowPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("ShadowPet")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("ShadowPet"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}