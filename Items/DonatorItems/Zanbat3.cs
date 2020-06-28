using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.DonatorItems;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
	public class Zanbat3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zanbat Blade");
		}
		int charger;
		public override void SetDefaults()
		{
			item.damage = 61;
			item.useTime = 19;
			item.useAnimation = 19;
			item.melee = true;
			item.width = 48;
			item.height = 48;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 7;
			item.value = 25700;
			item.rare = 4;
			item.crit = 5;
			item.shootSpeed = 11f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = ModContent.ProjectileType<ZanbatProj>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(Main.rand.Next(4) == 1) {
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 20);
				return true;
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void UseStyle(Player player)
		{
			float cosRot = (float)Math.Cos(player.itemRotation - 0.98f * player.direction * player.gravDir * 90f);
			float sinRot = (float)Math.Sin(player.itemRotation - 0.98f * player.direction * player.gravDir * 90f);
			for(int i = 0; i < 8; i++) {

				float length = (item.width * 1.2f - i * item.width / 9) * item.scale + 7 + i; //length to base + arm displacement
				int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, 133, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.White, .8f);
				Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(58, Main.LocalPlayer);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Zanbat2>(), 1);
			recipe.AddIngredient(ItemID.TitaniumSword, 1);
			recipe.AddIngredient(ItemID.SoulofMight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ModContent.ItemType<Zanbat2>(), 1);
			recipe1.AddIngredient(ItemID.AdamantiteSword, 1);
			recipe1.AddIngredient(ItemID.SoulofMight, 10);
			recipe1.AddTile(TileID.MythrilAnvil);
			recipe1.SetResult(this, 1);
			recipe1.AddRecipe();

		}
	}
}