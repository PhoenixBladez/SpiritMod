using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ScarabeusDrops.ScarabPet;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class ScarabPetBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Scarab");
			Description.SetDefault("'It really loves to roll...'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<GlobalClasses.Players.PetPlayer>().scarabPet = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<ScarabPetProjectile>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<ScarabPetProjectile>(), 0, 0f, player.whoAmI);
		}
	}
}