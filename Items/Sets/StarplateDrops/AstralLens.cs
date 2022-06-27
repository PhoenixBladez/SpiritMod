using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops
{
	public class AstralLens : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Convergence");
			Tooltip.SetDefault("Shoots out bursts of electrical stars that reconverge on the player");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/StarplateDrops/AstralLens_Glow");
		}

		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 11;
			Item.width = 44;
			Item.height = 46;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 01, 10, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.NPCDeath7;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Starshock1>();
			Item.shootSpeed = 63f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			for (int i = 0; i < 2; i++)
			{
				Projectile.NewProjectile(source, position.X - 8, position.Y + 8, velocity.X + ((float)Main.rand.Next(-300, 300) / 30), velocity.Y + ((float)Main.rand.Next(-300, 300) / 30), type, damage, knockback, player.whoAmI, 0f, 0f);
				if (Main.rand.Next(6) == 0)
					Projectile.NewProjectile(source, position.X - 8, position.Y + 8, velocity.X + ((float)Main.rand.Next(-300, 300) / 30), velocity.Y + ((float)Main.rand.Next(-300, 300) / 30), type, damage, knockback, player.whoAmI, 0f, 0f);
			}
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.08f, .28f, .38f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/StarplateDrops/AstralLens_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
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
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 17);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}