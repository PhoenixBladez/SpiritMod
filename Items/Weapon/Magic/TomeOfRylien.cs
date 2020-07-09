using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Magic;
using System;
using System.Collections;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class TomeOfRylien : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of R'lyeh");
		}



		public override void SetDefaults()
		{
			item.damage = 29;
			item.noMelee = true;
			item.magic = true;
			item.width = 22;
			item.height = 22;
			item.useTime = 21;
			item.mana = 13;
			item.useAnimation = 21;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 4;
			item.value = Terraria.Item.buyPrice(0, 7, 0, 0);
			item.rare = 3;
		//	item.UseSound = SoundID.Item103;
			item.autoReuse = true;
			item.shootSpeed = 11;
			item.shoot = ModContent.ProjectileType<TentacleSpike>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
			List<Vector2> tiles = new List<Vector2>();
			for (int i = -12; i < 12; i++)
			{
				for (int j= -12; j < 12; j++)
				{
					Tile tile = Framing.GetTileSafely(i + (int)(mouse.X / 16), j + (int)(mouse.Y / 16));
					if (tile.active() && Main.tileSolid[tile.type])
					{
						tiles.Add(new Vector2(i + (int)(mouse.X / 16), j + (int)(mouse.Y / 16)));
					}
				}
			}
			double currentdist = 9999;
			if (tiles.Count > 0)
			{
				for (int k = 0; k < tiles.Count; k++)
				{
					Vector2 distance = (tiles[k] * 16) - mouse;
					double truedist = Math.Sqrt((distance.X * distance.X) + (distance.Y * distance.Y));
					if (truedist < currentdist && (Main.rand.NextBool(5) || currentdist == 9999))
					{
						position = tiles[k] * 16;
						currentdist = truedist;
					}
				}
				Vector2 direction9 = mouse - position;
				direction9.Normalize();
				direction9 *= item.shootSpeed;
				speedX = direction9.X;
				speedY = direction9.Y;
				Main.PlaySound(2, position, 103);
				return true;
			}
			return false;
		}
	}
}
