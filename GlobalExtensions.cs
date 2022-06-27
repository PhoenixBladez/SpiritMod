using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod
{
	public static class GlobalExtensions
	{
		public static IEntitySource Source_ShootWithAmmo(this Item item, Player player, string? context = null)
		{
			player.PickAmmo(item, out int _, out float _, out int _, out float _, out int ammo, true);
			return item.GetSource_ItemUse_WithPotentialAmmo(item, ammo, context);
		}

		public static bool IsRanged(this Projectile proj) => proj.CountsAsClass(DamageClass.Ranged);
		public static bool IsMelee(this Projectile proj) => proj.CountsAsClass(DamageClass.Melee);
		public static bool IsMagic(this Projectile proj) => proj.CountsAsClass(DamageClass.Magic);
		public static bool IsSummon(this Projectile proj) => proj.CountsAsClass(DamageClass.Summon);
		public static bool IsThrown(this Projectile proj) => proj.CountsAsClass(DamageClass.Throwing);
	}
}
