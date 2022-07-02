using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Magic;
using System;
using System.Collections;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.RlyehianDrops
{
	public class TomeOfRylien : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of R'lyeh");
		}



		public override void SetDefaults()
		{
			Item.damage = 29;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.width = 22;
			Item.height = 22;
			Item.useTime = 12;
			Item.mana = 9;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
			Item.value = Terraria.Item.buyPrice(0, 7, 0, 0);
			Item.rare = ItemRarityID.Orange;
			//	item.UseSound = SoundID.Item103;
			Item.autoReuse = true;
			Item.shootSpeed = 11;
			Item.shoot = ModContent.ProjectileType<TentacleSpike>();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 mouse = Main.MouseWorld;
			List<Vector2> tiles = new List<Vector2>();
			for (int i = -12; i < 12; i++)
			{
				for (int j = -12; j < 12; j++)
				{
					Tile tile = Framing.GetTileSafely(i + (int)(mouse.X / 16), j + (int)(mouse.Y / 16));
					if (tile.HasTile && Main.tileSolid[tile.TileType])
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
				direction9 *= Item.shootSpeed;
				velocity = direction9;
				SoundEngine.PlaySound(SoundID.Item103, position);
			}
		}
	}
}
