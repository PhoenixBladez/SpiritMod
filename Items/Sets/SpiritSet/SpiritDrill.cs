using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	public class SpiritDrill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Drill");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/SpiritSet/SpiritDrill_Glow");
		}

		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 22;
			Item.value = 40000;
			Item.pick = 180;
			Item.damage = 39;
			Item.knockBack = 0;
			Item.shootSpeed = 40f;
			Item.useTime = 7;
			Item.useAnimation = 25;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item23;
			Item.rare = ItemRarityID.Pink;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<SpiritDrillProjectile>();
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.Assets.Request<Texture2D>("Items/Sets/SpiritSet/SpiritDrill_Glow").Value,
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
			Recipe modRecipe = CreateRecipe();
			modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 18);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();
		}
	}
}
