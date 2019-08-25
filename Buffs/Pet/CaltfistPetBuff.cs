using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class CaltfistPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Cultfish");
			//Description.SetDefault("R'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<MyPlayer>(mod).caltfist = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("Caltfist")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("Caltfist"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}