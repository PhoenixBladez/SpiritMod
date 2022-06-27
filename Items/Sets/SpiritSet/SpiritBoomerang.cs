using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	public class SpiritBoomerang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Boomerang");
			Tooltip.SetDefault("Inflicts Soul Burn");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/SpiritSet/SpiritBoomerang_Glow");
		}


		public override void SetDefaults()
		{
			Item.damage = 54;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = Terraria.Item.sellPrice(0, 1, 20, 0);
			Item.rare = ItemRarityID.Pink;
			Item.shootSpeed = 9f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Returning.SpiritBoomerang>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.Assets.Request<Texture2D>("Items/Sets/SpiritSet/SpiritBoomerang_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
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
			for (int I = 0; I < 2; I++) {
				Projectile.NewProjectile(source, position.X - 8, position.Y + 8, velocity.X + ((float)Main.rand.Next(-250, 250) / 100), velocity.Y + ((float)Main.rand.Next(-250, 250) / 100), type, damage, knockback, player.whoAmI, 0f, 0f);
			}
			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<SoulShred>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}