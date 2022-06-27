using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	public class Revenant : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Revenant");
			Tooltip.SetDefault("Converts regular arrows into Revenant Arrows");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/SpiritSet/Revenant_Glow");
		}


		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.damage = 44;
			Item.knockBack = 1f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SpiritArrow>();
			Item.shootSpeed = 10f;
			Item.UseSound = SoundID.Item5;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.Assets.Request<Texture2D>("Items/Sets/SpiritSet/Revenant_Glow").Value,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
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

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<SpiritArrow>();
			}
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 14);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();
		}
	}
}
