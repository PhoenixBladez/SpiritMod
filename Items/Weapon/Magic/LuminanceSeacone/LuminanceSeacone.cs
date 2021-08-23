using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.LuminanceSeacone
{
	public class LuminanceSeacone : ModItem
	{
		public override void SetDefaults()
		{

			item.damage = 17;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.magic = true;
			item.width = 36;
			item.height = 40;
			item.useTime = 31;
			item.useAnimation = 31;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = mod.ProjectileType("LuminanceSeaconeProjectile");
			item.shootSpeed = 9.5f;
			item.knockBack = 4f;
			item.autoReuse = false;
			item.rare = ItemRarityID.Green;
			item.value = Item.sellPrice(silver: 80);
			item.useTurn = false;
			item.mana = 12;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminance Seacone");
			Tooltip.SetDefault("Bursts into a cluster of luminescent bubbles\nLuminescent Bubbles deal increased damage after getting wet");
			Item.staff[item.type] = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 50f;
			float num2 = (float)Main.mouseX + Main.screenPosition.X;
			float num3 = (float)Main.mouseY + Main.screenPosition.Y;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			for (int i = 0; i<3; i++)
			{
				int spread = Main.rand.Next(-6,6);
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(spread));
				int p = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 0.0f, 0.0f);
			}

			return false;
		}
	}
}
