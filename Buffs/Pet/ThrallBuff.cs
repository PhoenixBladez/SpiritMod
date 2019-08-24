using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class ThrallBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Lil' Leonard");
			Description.SetDefault("'Grrr...'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<MyPlayer>(mod).thrallPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("ThrallPet")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("ThrallPet"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}