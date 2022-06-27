using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.LuminanceSeacone
{
	public class LuminanceSeacone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminance Seacone");
			Tooltip.SetDefault("Bursts into a cluster of luminescent bubbles\nLuminescent Bubbles deal increased damage after getting wet");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{

			Item.damage = 17;
			Item.noMelee = true;
			Item.noUseGraphic = false;
			Item.DamageType = DamageClass.Magic;
			Item.width = 36;
			Item.height = 40;
			Item.useTime = 31;
			Item.useAnimation = 31;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<LuminanceSeaconeProjectile>();
			Item.shootSpeed = 9.5f;
			Item.knockBack = 4f;
			Item.autoReuse = false;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(silver: 80);
			Item.useTurn = false;
			Item.mana = 12;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 50f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			
			for (int i = 0; i<3; i++)
			{
				int spread = Main.rand.Next(-6,6);
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(spread));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI, 0.0f, 0.0f);
			}
			return false;
		}
	}
}
