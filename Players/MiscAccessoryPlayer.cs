using SpiritMod.Items.Accessory;
using SpiritMod.Projectiles.Clubs;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Players
{
	public class MiscAccessoryPlayer : ModPlayer
	{
		/// <summary>Allows you to modify the knockback given ANY damage source. NOTE: This is an IL hook, which is why it needs a Player instance and is static.</summary>
		/// <param name="player">The specific player to change.</param>
		/// <param name="horizontal">Whether this is a horizontal (velocity.X) change or a vertical (velocity.Y) change.</param>
		public static float KnockbackMultiplier(Player player, bool horizontal)
		{
			float totalKb = 1f;

			if (player.AccessoryEquipped<FrostGiantBelt>() && player.channel)
				if (HeldItemIsClub(player))
					totalKb *= 0.5f;

			if (totalKb < 0.001f) //Throws NullReferenceException if 
				totalKb = 0.001f;
			return totalKb;
		}

		public override void ModifyWeaponDamage(Item item, ref float add, ref float mult, ref float flat)
		{
			if (HeldItemIsClub(player))
				mult += item.knockBack / 5f;
		}

		/// <summary>A bit of a wacky way of checking if a held item is a club, but it works.</summary>
		/// <param name="player">Player to check held item of.</param>
		public static bool HeldItemIsClub(Player player)
		{
			Item heldItem = player.HeldItem;
			if (heldItem.shoot > ProjectileID.None && heldItem.modItem != null && heldItem.modItem.mod == ModContent.GetInstance<SpiritMod>())
			{
				var p = new Projectile();
				p.SetDefaults(player.HeldItem.shoot);

				if (p.modProjectile != null && p.modProjectile is ClubProj)
					return true;
			}
			return false;
		}
	}
}
