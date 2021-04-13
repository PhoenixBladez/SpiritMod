using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Vulture_Matriarch
{
	public class Golden_Egg : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 42;
			item.maxStack = 999;
			item.rare = -11;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulture Matriarch's Golden Egg");
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange *= 0;
		}
		public override bool ItemSpace(Player player)
		{
			return true;
		}
		public override bool OnPickup(Player player)
		{
			for (int i = 0; i < item.stack; i++)
			{
				if (Main.rand.Next(5) == 0)
				{
					player.QuickSpawnItem(mod.ItemType("HourglassItem"), 1);
				}
				int weaponType = Utils.SelectRandom<int>(Main.rand, new int[2]
				{
				  mod.ItemType("Tome_of_the_Great_Scavenger"),
				  mod.ItemType("Sovereign_Talon"),
				});
				if (Main.rand.Next(7) == 0)
				{
					Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, mod.ItemType("Vulture_Matriarch_Mask"), 1);
				}
				Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, weaponType, 1);

				float num5 = 15000f * (float)(1.0 + (double)Main.rand.Next(-20, 21) * 0.00999999977648258);
				if (Main.rand.Next(5) == 0)
				{
					num5 *= (float)(1.0 + (double)Main.rand.Next(5, 11) * 0.00999999977648258);
				}

				if (Main.rand.Next(10) == 0)
				{
					num5 *= (float)(1.0 + (double)Main.rand.Next(10, 21) * 0.00999999977648258);
				}

				if (Main.rand.Next(15) == 0)
				{
					num5 *= (float)(1.0 + (double)Main.rand.Next(15, 31) * 0.00999999977648258);
				}

				if (Main.rand.Next(20) == 0)
				{
					num5 *= (float)(1.0 + (double)Main.rand.Next(20, 41) * 0.00999999977648258);
				}

				while ((int)num5 > 0)
				{
					if ((double)num5 > 1000000.0)
					{
						int stack = (int)((double)num5 / 1000000.0);
						num5 -= (float)(1000000 * stack);
						player.QuickSpawnItem(74, stack);
					}
					else if ((double)num5 > 10000.0)
					{
						int stack = (int)((double)num5 / 10000.0);
						num5 -= (float)(10000 * stack);
						player.QuickSpawnItem(73, stack);
					}
					else if ((double)num5 > 100.0)
					{
						int stack = (int)((double)num5 / 100.0);
						num5 -= (float)(100 * stack);
						player.QuickSpawnItem(72, stack);
					}
					else
					{
						int stack = (int)num5;
						if (stack < 1)
						{
							stack = 1;
						}

						num5 -= (float)stack;
						player.QuickSpawnItem(71, stack);
					}
				}
			}
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (Main.rand.Next(20) == 0)
			{
				int index3 = Dust.NewDust(new Vector2(item.position.X, item.position.Y), item.width, item.height, 228, 0.0f, 0.0f, 150, new Color(), 0.3f);
				Main.dust[index3].fadeIn = 0.75f;
				Dust dust = Main.dust[index3];
				Vector2 vector2_2 = dust.velocity * 0.1f;
				dust.velocity = vector2_2;
				Main.dust[index3].noLight = true;
				Main.dust[index3].noGravity = true;
			}
		}
	}
}
