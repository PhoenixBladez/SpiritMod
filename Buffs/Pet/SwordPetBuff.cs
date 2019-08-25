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
			player.GetModPlayer<MyPlayer>(mod).SwordPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("SwordPet")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("SwordPet"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}