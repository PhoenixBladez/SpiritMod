using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SeraphSet
{
	public class StarBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Storm");
			Tooltip.SetDefault("Launches bolts of sporadic lunar energy");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/SeraphSet/StarBow_Glow");

		}


		//private Vector2 newVect;
		public override void SetDefaults()
		{
			Item.damage = 28;
            Item.width = 22;
            Item.height = 40;
			Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.knockBack = 4;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 13;
			Item.useAnimation = 13;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SleepingStar>();
			Item.shootSpeed = 9;
			Item.UseSound = SoundID.Item5;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			for(int i = 1; i <= 2; i++)
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SleepingStar>(), damage, knockback, player.whoAmI, i);

			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/SeraphSet/StarBow_Glow").Value,
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
			recipe.AddIngredient(ModContent.ItemType<MoonStone>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

	}
}