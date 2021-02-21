using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.DonatorItems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
	public class FolvBlade4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Ancient Blade");
			Tooltip.SetDefault("Returns a large amount of mana on swing\nReleases a powerful arcane sword\n'The power of ancient mana runs through this sword'");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/DonatorItems/Folv/FolvBlade4_Glow");
		}


		public override void SetDefaults()
		{
			item.damage = 121;
			item.melee = true;
			item.width = 80;
			item.height = 80;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.autoReuse = true;
			item.knockBack = 8.3f;
			item.value = Item.sellPrice(0, 40, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<ArcaneSword>();
			item.shootSpeed = 12;
		}
		public override void UseStyle(Player player)
		{
			float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
			float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
			for (int i = 0; i < 6; i++) {
				float length = (item.width * 1.2f - i * item.width / 9) * item.scale - 4 + i; //length to base + arm displacement
				int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, 187, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.Transparent, 1.1f);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
		}
		public override bool OnlyShootOnSwing => true;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/DonatorItems/Folv/FolvBlade4_Glow"),
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
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			{
				player.statMana += 18;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Material.EternityEssence
				>(), 5);
            recipe.AddIngredient(ModContent.ItemType<FolvBlade3>(), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}