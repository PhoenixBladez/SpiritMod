using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class Revenant : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Revenant");
			Tooltip.SetDefault("Converts regular arrows into Revenant Arrows");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Bow/Revenant_Glow");
		}


		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 28;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.damage = 44;
			item.knockBack = 1f;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useAmmo = AmmoID.Arrow;
			item.ranged = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<SpiritArrow>();
			item.shootSpeed = 10f;
			item.UseSound = SoundID.Item5;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Bow/Revenant_Glow"),
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
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<SpiritArrow>();
			}
				Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 14);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
