using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.MoonWizardDrops.MJWPet;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class MJWPetBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Jelly Lightbulb");
			Description.SetDefault("'No installation necessary!'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<GlobalClasses.Players.PetPlayer>().mjwPet = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<MJWPetProjectile>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<MJWPetProjectile>(), 0, 0f, player.whoAmI);
		}
	}
}