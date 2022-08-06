using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SeraphSet
{
	public class GlowSting : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Strike");
			Tooltip.SetDefault("Right-click to release a flurry of strikes");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/SeraphSet/GlowSting_Glow");
		}

		int currentHit;
		public override void SetDefaults()
		{
			Item.damage = 47;
			Item.DamageType = DamageClass.Melee;
			Item.width = 34;
			Item.height = 40;
			Item.autoReuse = true;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<GlowStingSpear>();
			Item.shootSpeed = 10f;
			this.currentHit = 0;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/SeraphSet/GlowSting_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
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
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 180);
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				Item.noUseGraphic = true;
				Item.shoot = ModContent.ProjectileType<GlowStingSpear>();
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.useTime = 7;
				Item.useAnimation = 7;
				Item.damage = 34;
				Item.knockBack = 2;
				Item.shootSpeed = 8f;
			}
			else {
				Item.damage = 47;
				Item.noUseGraphic = false;
				Item.useTime = 25;
				Item.useAnimation = 25;
				Item.shoot = ProjectileID.None;
				Item.knockBack = 5;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.shootSpeed = 0f;
			}
			if (player.ownedProjectileCounts[Item.shoot] > 0)
				return false;
			return true;
		}
		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.altFunctionUse != 2) {
				for (int projFinder = 0; projFinder < 300; ++projFinder) {
					if (Main.projectile[projFinder].type == ModContent.ProjectileType<GlowStingSpear>()) {
						Main.projectile[projFinder].Kill();
						Main.projectile[projFinder].timeLeft = 2;
					}
				}
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Vector2 origVect = velocity;
				if (Main.rand.NextBool(2))
					velocity = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				else
					velocity = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				currentHit++;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MoonStone>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}