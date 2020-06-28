using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ReachVineStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodpetal Wand");
			Tooltip.SetDefault("Rains down multiple blood petals from the sky around the cursor position");
		}


		public override void SetDefaults()
		{
			item.damage = 17;
			item.magic = true;
			item.mana = 6;
			item.width = 44;
			item.height = 48;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<ReachPetal>();
			item.shootSpeed = 15f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(6, (int)player.position.X, (int)player.position.Y);
			Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
			int amount = Main.rand.Next(1, 3);
			for(int i = 0; i < amount; ++i) {
				Vector2 pos = new Vector2(mouse.X + player.width * 0.5f + Main.rand.Next(-20, 21), mouse.Y - 600f);
				pos.X = (pos.X * 10f + mouse.X) / 11f + (float)Main.rand.Next(-10, 11);
				pos.Y -= 150;
				float spX = mouse.X + player.width * 0.5f + Main.rand.Next(-200, 201) - mouse.X;
				float spY = mouse.Y - pos.Y;
				if(spY < 0f)
					spY *= -1f;
				if(spY < 20f)
					spY = 20f;

				float length = (float)Math.Sqrt((double)(spX * spX + spY * spY));
				length = 12 / length;
				spX *= length;
				spY *= length;
				spX = spX - (float)Main.rand.Next(-10, 11) * 0.02f;
				spY = spY + (float)Main.rand.Next(-40, 41) * 0.2f;
				spX *= (float)Main.rand.Next(-10, 10) * 0.006f;
				pos.X += (float)Main.rand.Next(-10, 11);
				Projectile.NewProjectile(pos.X, pos.Y, spX, 12f, type, damage, 4, player.whoAmI);
			}
			return false;
		}
	}
}