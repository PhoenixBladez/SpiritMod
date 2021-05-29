using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Magic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ShadowflameStoneStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowbreak Wand");
			Tooltip.SetDefault("Holding the item summons erratic shadowflame wisps around the player\nAttacking with the weapon allows these wisps to be controlled by the cursor\nUp to five wisps can exist at once\nInflicts Shadowflame");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/ShadowflameStoneStaff_Glow");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 46;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.damage = 12;
			item.knockBack = 4;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.useTime = 12;
			item.useAnimation = 24;
			item.mana = 10;
			item.magic = true;
			item.channel = true;
			item.UseSound = SoundID.Item8;
			item.autoReuse = false;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<ShadowflameStoneBolt>();
			item.shootSpeed = 10f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int I = 0; I < Main.rand.Next(1, 3); I++) {
				float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
				Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) {
					position += spawnPlace;
				}

				Vector2 velocity = Vector2.Normalize(Main.MouseWorld - position) * item.shootSpeed;
				int p = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, 0, 0.0f, 0.0f);
				for (float num2 = 0.0f; (double)num2 < 10; ++num2) {
					int dustIndex = Dust.NewDust(position, 2, 2, 173, 0f, 0f, 0, default(Color), .8f);
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].velocity = Vector2.Normalize(spawnPlace.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
				}
			}

			return false;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/ShadowflameStoneStaff_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
	}
}
