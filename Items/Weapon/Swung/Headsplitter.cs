using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Arrow;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class Headsplitter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Headsplitter");
			Tooltip.SetDefault("Right click to release an explosion of vengeance\nUsing it too frequently will reduce its damage\nInflicts 'Surging Anguish'");
		}


		public override void SetDefaults()
		{
			item.damage = 20;
			item.melee = true;
			item.width = 34;
			item.height = 40;
			item.useTime = 26;
			item.useAnimation = 26;
			item.axe = 9;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.axe = 10;
			item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<FlayedExplosion>();
			item.shootSpeed = 12f;
			item.autoReuse = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BloodFire>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0) {
				target.AddBuff(ModContent.BuffType<SurgingAnguish>(), 180);
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.shootDelay < 150 && player.altFunctionUse == 2) {
				Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 20);
				damage = 1 + (int)((damage * 1.35f) / (MathHelper.Clamp((float)Math.Sqrt(modPlayer.shootDelay), 1, 180)));
				Projectile.NewProjectile(position.X, position.Y, 0, 0, ModContent.ProjectileType<FlayedExplosion>(), damage, 2.5f, Main.myPlayer);
				modPlayer.shootDelay = 180;
			}
			return false;
		}
		public override void UseStyle(Player player)
		{
			float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
			float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
			for (int i = 0; i < 1; i++) {
				float length = (item.width * 1.2f - i * item.width / 9) * item.scale + 16; //length to base + arm displacement
				int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, 5, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.Transparent, 1.8f);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}