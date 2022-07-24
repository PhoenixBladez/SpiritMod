using Microsoft.Xna.Framework;
using SpiritMod.GlobalClasses.Players;
using SpiritMod.Projectiles.Pet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public class JellyfishBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boop");
			Description.SetDefault("'The Jellyfish is helping you relax'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.AddBuff(BuffID.PeaceCandle, 2);
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<PetPlayer>().jellyfishPet = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<JellyfishPet>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer) {
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<JellyfishPet>(), 0, 0f, player.whoAmI);
			}
		}
	}
}