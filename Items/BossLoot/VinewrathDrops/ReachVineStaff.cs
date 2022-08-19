using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.VinewrathDrops
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
			Item.damage = 17;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 6;
			Item.width = 44;
			Item.height = 48;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ReachPetal>();
			Item.shootSpeed = 15f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			SoundEngine.PlaySound(SoundID.Grass, player.Center);
			Vector2 mouse = Main.MouseWorld;
			int amount = Main.rand.Next(1, 3);
			for (int i = 0; i < amount; ++i) {
				Vector2 pos = new Vector2(mouse.X + player.width * 0.5f + Main.rand.Next(-20, 21), mouse.Y - 600f);
				pos.X = (pos.X * 10f + mouse.X) / 11f + (float)Main.rand.Next(-10, 11);
				pos.Y -= 150;
				float spX = mouse.X + player.width * 0.5f + Main.rand.Next(-200, 201) - mouse.X;
				float spY = mouse.Y - pos.Y;
				if (spY < 0f)
					spY *= -1f;
				if (spY < 20f)
					spY = 20f;

				float length = (float)Math.Sqrt((double)(spX * spX + spY * spY));
				length = 12 / length;
				spX *= length;
				spY *= length;
				spX = spX - (float)Main.rand.Next(-10, 11) * 0.02f;
				spY = spY + (float)Main.rand.Next(-40, 41) * 0.2f;
				spX *= (float)Main.rand.Next(-10, 10) * 0.006f;
				pos.X += (float)Main.rand.Next(-10, 11);
				Projectile.NewProjectile(source, pos.X, pos.Y, spX, 12f, type, damage, 4, player.whoAmI);
			}
			return false;
		}
	}
}