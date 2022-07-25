using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.DataStructures;
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

			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.width = 32;
			Item.height = 42;
			Item.useTime = Item.useAnimation = 27;
			Item.mana = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 0, 65, 0);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.shootSpeed = 9;
			Item.UseSound = SoundID.Item20;
			Item.shoot = ModContent.ProjectileType<ShroomSummon>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			const int ProjDimensions = 24;

			while (true)
			{
				float dist = Main.rand.NextFloat(100, 200);
				Vector2 offset = new Vector2(dist, 0).RotatedByRandom(MathHelper.TwoPi);
				Vector2 pos = position + offset;

				if (!Collision.SolidCollision(pos, ProjDimensions, ProjDimensions) && Collision.CanHitLine(pos, ProjDimensions, ProjDimensions, position, player.width, player.height))
				{
					Projectile.NewProjectile(source, pos, Vector2.Zero, ModContent.ProjectileType<ShroomSummon>(), damage, knockback, player.whoAmI, 0f, 0f);
					break;
				}
			}

			SpawnDust(player, velocity);
			return false;
		}

		private static void SpawnDust(Player player, Vector2 velocity)
		{
			for (int k = 0; k < 15; k++)
			{
				Vector2 offset = Vector2.Normalize(Main.MouseWorld - player.position);

				if (velocity.X > 0)
					offset = offset.RotatedBy(-0.2f);
				else
					offset = offset.RotatedBy(0.2f);

				offset *= 58f;
				int dust = Dust.NewDust(player.Center + offset, player.width / 2, player.height / 2, DustID.Harpy);

				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				Vector2 vector2_1 = Vector2.Normalize(new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101))) * (Main.rand.Next(50, 100) * 0.02f);
				Main.dust[dust].velocity = vector2_1;
				vector2_1.Normalize();
				Vector2 vector2_3 = vector2_1 * 5f;
				Main.dust[dust].position = (player.Center + offset) + vector2_3;

				if (velocity.X > 0)
					Main.dust[dust].velocity = new Vector2(velocity.X / 3f, velocity.Y / 3f).RotatedBy(Main.rand.Next(-220, 180) / 100);
				else
					Main.dust[dust].velocity = new Vector2(velocity.X / 3f, velocity.Y / 3f).RotatedBy(Main.rand.Next(-180, 220) / 100);
			}
		}
	}
}