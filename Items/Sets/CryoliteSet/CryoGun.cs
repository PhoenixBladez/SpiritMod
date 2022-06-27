using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.CryoliteSet
{
	public class CryoGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winter's Wake");
			Tooltip.SetDefault("Converts rockets into flake rockets");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/CryoliteSet/CryoGun_Glow");
		}

		public override void SetDefaults()
		{
			Item.damage = 42;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.rare = ItemRarityID.Orange;
			Item.width = 50;
            Item.height = 26;
            Item.value = Item.sellPrice(0, 0, 70, 0);
            Item.useAnimation = 60;
			Item.useTime = 60;
			Item.knockBack = 2.5f;
			Item.crit = 6;
			Item.UseSound = SoundID.Item96;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.RocketI;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Rocket;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-8, 0);

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => type = ModContent.ProjectileType<Projectiles.Bullet.FlakeRocketProj>();

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.GetTexture("Items/Sets/CryoliteSet/CryoGun_Glow"),
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
			recipe.AddIngredient(ItemID.Handgun);
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}