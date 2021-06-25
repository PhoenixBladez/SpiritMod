using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.StarplateDrops;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.AlphaBladeTree
{
	public class Starblade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starblade");
			Tooltip.SetDefault("'Harness the night sky'\nEvery sixth swing causes the blade to release multiple bright stars\nEach star explodes into homing star wisps");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/AlphaBladeTree/Starblade_Glow");
		}

		int charger;
		public override void SetDefaults()
		{
			item.damage = 32;
			item.useTime = 27;
			item.useAnimation = 27;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 4;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.shootSpeed = 8;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = ModContent.ProjectileType<Starshock2>();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 226);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0.1f;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/AlphaBladeTree/Starblade_Glow"),
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
			charger++;
			if (charger >= 6) {
				for (int i = 0; i < 3; i++) {
					Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("Starshock2"), damage, knockBack, player.whoAmI, 0f, 0f);
				}
				charger = 0;
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Starfury, 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.AvianDrops.TalonBlade>(), 1);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}