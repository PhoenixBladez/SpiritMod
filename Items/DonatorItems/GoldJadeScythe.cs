using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.DonatorItems;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class GoldJadeScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crook of the Tormented");
			Tooltip.SetDefault("Occasionally spawns ornate scarabs to defend you upon hitting enemies");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/DonatorItems/GoldJadeScythe_Glow");
		}


		public override void SetDefaults()
		{
			Item.damage = 42;
			Item.DamageType = DamageClass.Melee;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 19;
			Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(0, 10, 0, 0);
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.useTurn = true;
			Item.crit = 9;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.255f, .509f, .072f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.Assets.Request<Texture2D>("Items/DonatorItems/GoldJadeScythe_Glow").Value,
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
			if (Main.rand.NextBool(8)) {
				Vector2 velocity = new Vector2(player.direction, 0) * 4f;
				int proj = Projectile.NewProjectile(Item.GetSource_ItemUse(Item), player.Center.X, player.position.Y + player.height + -35, velocity.X, velocity.Y, ModContent.ProjectileType<JadeScarab>(), Item.damage / 2, Item.playerIndexTheItemIsReservedFor, 0, 0f);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
			}
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
			float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
			for (int i = 0; i < 1; i++) {
				float length = (Item.width * 1.2f - i * Item.width / 9) * Item.scale - 4; //length to base + arm displacement
				int dust = Dust.NewDust(new Vector2((player.itemLocation.X + length * cosRot * player.direction), (player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, DustID.TerraBlade, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.Transparent, .8f);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Sets.ScarabeusDrops.Chitin>(), 12);
			recipe.AddIngredient(ItemID.Emerald, 4);
			recipe.AddRecipeGroup("SpiritMod:GoldBars", 5);
			recipe.AddIngredient(ItemID.SoulofLight, 5);
			recipe.AddIngredient(ItemID.SoulofNight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}