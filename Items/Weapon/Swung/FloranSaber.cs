using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Sword;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class FloranSaber : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Saber");
			Tooltip.SetDefault("Launches vines that occasionally ensnare foes, reducing movement speed");
		}


		public override void SetDefaults()
		{
			item.damage = 16;
			item.melee = true;
			item.width = 30;
			item.height = 36;
			item.useTime = 60;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3;
			item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<FloranVine>();
			item.shootSpeed = 8f;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<FloranBar>(), 13);
            modRecipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 4);
            modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(SoundID.Item20, player.position);
			Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
			for(int i = 0; i < 2; i++) {
				float xnew = position.X + Main.rand.Next(-15, 15);
				float ynew = position.Y + Main.rand.Next(-25, 15);
				Vector2 newpos = new Vector2(xnew, ynew);
				Vector2 newSpeed = mouse - newpos;
				float distance = (float)Math.Sqrt((double)newSpeed.X * (double)newSpeed.X + (double)newSpeed.Y * (double)newSpeed.Y);
				newSpeed.Normalize();
				newSpeed *= 4f;
				int proj = Projectile.NewProjectile(newpos, newSpeed, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].timeLeft = 60;
			}
			return false;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			{
				if(Main.rand.Next(5) == 0) target.AddBuff(ModContent.BuffType<VineTrap>(), 180);
			}
		}
		public override void UseStyle(Player player)
		{
			float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
			float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
			float length = (item.width * 1.2f - 1 * item.width / 9) * item.scale + 16; //length to base + arm displacement
			int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, 39, player.velocity.X * 0.9f, player.velocity.Y * 0.9f);
			Main.dust[dust].velocity *= 0f;
			Main.dust[dust].noGravity = true;
		}

	}
}