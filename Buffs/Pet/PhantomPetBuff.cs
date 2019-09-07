using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class PhantomPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Phantom");
			Description.SetDefault("'It blends into the night'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<MyPlayer>(mod).phantomPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("PhantomPet")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("PhantomPet"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}