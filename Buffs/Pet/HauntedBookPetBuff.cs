using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
    public class HauntedBookPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Haunted Tome");
			Description.SetDefault("'Haunted, yet comforting'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetSpiritPlayer().bookPet = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<HauntedBookPet>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<HauntedBookPet>(), 0, 0f, player.whoAmI);
			}
		}
	}
}