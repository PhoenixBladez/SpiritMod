using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CoilSet
{
	public class CoilSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Blade");
			Tooltip.SetDefault("Every six successful hits on an enemy releases an electrical explosion");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/CoilSet/CoilSword_Glow");
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 23;
			Item.useAnimation = 23;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.08f, .12f, .52f);

			Texture2D texture = TextureAssets.Item[Item.type].Value;

			spriteBatch.Draw
			(
				Mod.GetTexture("Items/Sets/CoilSet/CoilSword_Glow"),
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

		int charger;

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			charger++;
			if (charger >= 6) {
				SoundEngine.PlaySound(SoundID.Item, (int)target.position.X, (int)target.position.Y, 14);
				Projectile.NewProjectile(target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<CoiledExplosion>(), damage, knockback, player.whoAmI);
				charger = 0;
			}
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe();
			modRecipe.AddIngredient(ModContent.ItemType<TechDrive>(), 4);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}
