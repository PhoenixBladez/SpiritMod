using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.Items.Halloween;

namespace SpiritMod
{
	public static class _ItemUtils
	{
		public static bool IsWeapon(this Item item)
		{
			return item.type != 0 && item.stack > 0 && item.useStyle > 0 && (item.damage > 0 || item.useAmmo > 0 && item.useAmmo != AmmoID.Solution);
		}

		public static void DropItem(this Entity ent, int type, int stack = 1)
		{
			Item.NewItem((int)ent.position.X, (int)ent.position.Y, ent.width, ent.height, type, stack);
		}

		public static void DropItem(this Entity ent, int type, float chance)
		{
			if (Main.rand.NextDouble() < chance)
				Item.NewItem((int)ent.position.X, (int)ent.position.Y, ent.width, ent.height, type);
		}

		public static void DropItem(this Entity ent, int type, int min, int max)
		{
			Item.NewItem((int)ent.position.X, (int)ent.position.Y, ent.width, ent.height, type, Main.rand.Next(min, max));
		}

		public static void DropCandy(Player player)
		{
			int effect = Main.rand.Next(100);
			if (effect < 9)
			{
				player.QuickSpawnItem(Taffy._type);
			}
			else if (effect < 29)
			{
				player.QuickSpawnItem(Candy._type);
			}
			else if (effect < 49)
			{
				player.QuickSpawnItem(ChocolateBar._type);
			}
			else if (effect < 59)
			{
				player.QuickSpawnItem(HealthCandy._type);
			}
			else if (effect < 69)
			{
				player.QuickSpawnItem(ManaCandy._type);
			}
			else if (effect <79)
			{
				player.QuickSpawnItem(Lollipop._type);
			}
			else if (effect< 83)
			{
				player.QuickSpawnItem(Apple._type);
			}
			else if (effect < 95)
			{
				player.QuickSpawnItem(MysteryCandy._type);
			}
			else
			{
				player.QuickSpawnItem(GoldCandy._type);
			}
		}

		public static Color RarityColor(this Item item, float alpha = 1)
		{
			if (alpha > 1)
				alpha = 1;
			else if (alpha <= 0)
				return new Color(0, 0, 0, 0);

			if (item.rare == -11)
				return new Color((byte)(255f * alpha), (byte)(175f * alpha), (byte)(0f * alpha), (byte)(alpha * 255));

			if (item.rare == -1)
				return new Color((byte)(130f * alpha), (byte)(130f * alpha), (byte)(130f * alpha), (byte)(alpha * 255));

			if (item.rare == 1)
				return new Color((byte)(150f * alpha), (byte)(150f * alpha), (byte)(255f * alpha), (byte)(alpha * 255));

			if (item.rare == 2)
				return new Color((byte)(150f * alpha), (byte)(255f * alpha), (byte)(150f * alpha), (byte)(alpha * 255));

			if (item.rare == 3)
				return new Color((byte)(255f * alpha), (byte)(200f * alpha), (byte)(150f * alpha), (byte)(alpha * 255));

			if (item.rare == 4)
				return new Color((byte)(255f * alpha), (byte)(150f * alpha), (byte)(150f * alpha), (byte)(alpha * 255));

			if (item.rare == 5)
				return new Color((byte)(255f * alpha), (byte)(150f * alpha), (byte)(255f * alpha), (byte)(alpha * 255));

			if (item.rare == 6)
				return new Color((byte)(210f * alpha), (byte)(160f * alpha), (byte)(255f * alpha), (byte)(alpha * 255));

			if (item.rare == 7)
				return new Color((byte)(150f * alpha), (byte)(255f * alpha), (byte)(10f * alpha), (byte)(alpha * 255));

			if (item.rare == 8)
				return new Color((byte)(255f * alpha), (byte)(255f * alpha), (byte)(10f * alpha), (byte)(alpha * 255));

			if (item.rare == 9)
				return new Color((byte)(5f * alpha), (byte)(200f * alpha), (byte)(255f * alpha), (byte)(alpha * 255));

			if (item.rare == 10)
				return new Color((byte)(255f * alpha), (byte)(40f * alpha), (byte)(100f * alpha), (byte)(alpha * 255));

			if (item.rare >= 11)
				return new Color((byte)(180f * alpha), (byte)(40f * alpha), (byte)(255f * alpha), (byte)(alpha * 255));

			if (item.expert || item.rare == -12)
				return new Color((byte)(Main.DiscoR * alpha), (byte)(Main.DiscoG * alpha), (byte)(Main.DiscoB * alpha), (byte)(alpha * 255));

			return new Color((byte)(255 * alpha), (byte)(255 * alpha), (byte)(255 * alpha), (byte)(alpha * 255));
		}
	}
}
