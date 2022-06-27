using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ShroomFishSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fungal Icthyoid");
			Tooltip.SetDefault("Summons slow, homing spores around the player\nThese spores do not take up minion slots");
		}



		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.width = 32;
			Item.height = 42;
			Item.useTime = 25;
			Item.mana = 14;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = Terraria.Item.sellPrice(0, 0, 65, 0);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = false;
			Item.shootSpeed = 9;
			Item.UseSound = SoundID.Item20;
			Item.shoot = ModContent.ProjectileType<ShroomSummon>();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			int dimension = 24;
			int dimension2 = 90;
			for (int j = 0; j < 50; j++) {
				int randValue = Main.rand.Next(200 - j * 2, 400 + j * 2);
				Vector2 center = player.Center;
				center.X += Main.rand.Next(-randValue, randValue + 1);
				center.Y += Main.rand.Next(-randValue, randValue + 1);

				if (!Collision.SolidCollision(center, dimension, dimension) && !Collision.WetCollision(center, dimension, dimension)) {
					center.X += dimension / 2;
					center.Y += dimension / 2;

					if (Collision.CanHit(new Vector2(player.Center.X, player.position.Y), 1, 1, center, 1, 1) || Collision.CanHit(new Vector2(player.Center.X, player.position.Y - 50f), 1, 1, center, 1, 1)) {
						int x = (int)center.X / 16;
						int y = (int)center.Y / 16;

						bool flag = false;
						if (Main.rand.Next(4) == 0 && Main.tile[x, y] != null && Main.tile[x, y].WallType > 0) {
							flag = true;
						}
						else {
							center.X -= dimension2 / 2;
							center.Y -= dimension2 / 2;

							if (Collision.SolidCollision(center, dimension2, dimension2)) {
								center.X += dimension2 / 2;
								center.Y += dimension2 / 2;
								flag = true;
							}
						}

						if (flag) {
							for (int k = 0; k < 1000; k++) {
								if (Main.projectile[k].active && Main.projectile[k].owner == player.whoAmI && Main.projectile[k].type == ModContent.ProjectileType<ShroomSummon>() && (center - Main.projectile[k].Center).Length() < 48f) {
									flag = false;
									break;
								}
							}

							if (flag && Main.myPlayer == player.whoAmI) {
								Projectile.NewProjectile(center.X, center.Y, 0f, 0f, ModContent.ProjectileType<ShroomSummon>(), damage, knockback, player.whoAmI, 0f, 0f);
							}
						}
					}
				}
			}
			for (int k = 0; k < 15; k++) {
				Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
				Vector2 offset = mouse - player.position;
				offset.Normalize();
				if (speedX > 0) {
					offset = offset.RotatedBy(-0.2f);
				}
				else {
					offset = offset.RotatedBy(0.2f);
				}
				offset *= 58f;
				int dust = Dust.NewDust(player.Center + offset, player.width / 2, player.height / 2, DustID.Harpy);

				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				//        Main.dust[dust].scale *= 2f;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.02f);
				Main.dust[dust].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 5f;
				Main.dust[dust].position = (player.Center + offset) + vector2_3;
				if (speedX > 0) {
					Main.dust[dust].velocity = new Vector2(speedX / 3f, speedY / 3f).RotatedBy(Main.rand.Next(-220, 180) / 100);
				}
				else {
					Main.dust[dust].velocity = new Vector2(speedX / 3f, speedY / 3f).RotatedBy(Main.rand.Next(-180, 220) / 100);
				}
			}
			return false;
		}
	}

}
