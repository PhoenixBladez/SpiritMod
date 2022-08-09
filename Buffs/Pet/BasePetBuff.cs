using Microsoft.Xna.Framework;
using SpiritMod.GlobalClasses.Players;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
	public abstract class BasePetBuff<T> : ModBuff where T : ModProjectile
	{
		protected abstract (string, string) BuffInfo { get; }
		protected virtual bool IsLightPet => false;

		public sealed override void SetStaticDefaults()
		{
			DisplayName.SetDefault(BuffInfo.Item1);
			Description.SetDefault(BuffInfo.Item2);

			Main.buffNoTimeDisplay[Type] = true;

			if (IsLightPet)
				Main.lightPet[Type] = true;
			else
				Main.vanityPet[Type] = true;
		}

		public sealed override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			SetPetFlag(player, player.GetModPlayer<PetPlayer>());

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<T>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<T>(), 0, 0f, player.whoAmI);
		}

		public abstract void SetPetFlag(Player player, PetPlayer petPlayer);
	}
}
