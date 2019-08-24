using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class CaptiveMaskPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Unbound Mask");
			Description.SetDefault("'Once more unto the breach!'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<MyPlayer>(mod).maskPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("CaptiveMaskPet")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("CaptiveMaskPet"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}