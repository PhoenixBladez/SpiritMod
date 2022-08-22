using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod
{
	public static class GlobalExtensions
	{
		public static IEntitySource Source_ShootWithAmmo(this Item item, Player player, string context = null)
		{
			player.PickAmmo(item, out int _, out float _, out int _, out float _, out int ammo, true);
			return item.GetSource_ItemUse_WithPotentialAmmo(item, ammo, context);
		}

		public static bool IsRanged(this Projectile proj) => proj.CountsAsClass(DamageClass.Ranged);
		public static bool IsMelee(this Projectile proj) => proj.CountsAsClass(DamageClass.Melee);
		public static bool IsMagic(this Projectile proj) => proj.CountsAsClass(DamageClass.Magic);
		public static bool IsSummon(this Projectile proj) => proj.CountsAsClass(DamageClass.Summon);
		public static bool IsThrown(this Projectile proj) => proj.CountsAsClass(DamageClass.Throwing);

		public static bool IsRanged(this Item item) => item.CountsAsClass(DamageClass.Ranged);
		public static bool IsMelee(this Item item) => item.CountsAsClass(DamageClass.Melee);
		public static bool IsMagic(this Item item) => item.CountsAsClass(DamageClass.Magic);
		public static bool IsSummon(this Item item) => item.CountsAsClass(DamageClass.Summon);
		public static bool IsThrown(this Item item) => item.CountsAsClass(DamageClass.Throwing);

		public static Vector2 GetTreeSize(this ModTree tree, Tile tile)
		{
			int discard = 0;
			int width = 0;
			int height = 0;
			tree.SetTreeFoliageSettings(tile, ref discard, ref discard, ref discard, ref width, ref height);
			return new Vector2(width, height);
		}

		public static Vector2 GetRandomTreePosition(this ModTree tree, Tile tile)
		{
			var size = GetTreeSize(tree, tile);
			var halfSize = size / 2f;
			var offset = new Vector2(Main.rand.NextFloat(-halfSize.X, halfSize.X), -Main.rand.NextFloat(size.Y * 0.1f, size.Y * 0.8f));
			return offset;
		}
	}
}
