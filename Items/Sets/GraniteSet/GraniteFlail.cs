using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Flail;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GraniteSet
{
	public class GraniteFlail : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Colonnade");
			Tooltip.SetDefault("Killing enemies with this weapon causes them to explode into damaging energy wisps");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/GraniteSet/GraniteFlail_Glow");
		}


		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Green;
			Item.damage = 28;
			Item.knockBack = 7;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = Item.useAnimation = 41;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<GraniteMaceProj>();
			Item.shootSpeed = 11.5f;
			Item.UseSound = SoundID.Item1;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.08f, .12f, .52f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.Assets.Request<Texture2D>("Items/Sets/GraniteSet/GraniteFlail_Glow").Value,
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
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 18);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}