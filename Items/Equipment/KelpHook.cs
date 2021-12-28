using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items.Equipment
{
	internal class KelpHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kelp Hook");
			Tooltip.SetDefault("Provides faster mobility when underwater");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.AmethystHook);
			item.shootSpeed = 12f; // how quickly the hook is shot.
			item.shoot = ProjectileType<KelpHookHead>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.FloatingItems.Kelp>(), 20);
			recipe.AddIngredient(ItemID.Hook, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	internal class KelpHookHead : ModProjectile
	{
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Kelp Hook");

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
			projectile.timeLeft = 1200;
		}
		public override bool? CanUseGrapple(Player player)
		{
			int hooksOut = 0;
			for (int l = 0; l < 1000; l++) {
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type) {
					hooksOut++;
				}
			}
			if (hooksOut > 0) 
			{
				return false;
			}
			return true;
		}

		public override float GrappleRange() => Main.player[projectile.owner].wet ? 425 : 325;

		public override void NumGrappleHooks(Player player, ref int numHooks) => numHooks = 1;


		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			int retreatSpeed = Main.player[projectile.owner].wet ? 17 : 12;
			speed = retreatSpeed;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			int underwaterSpeed = Main.player[projectile.owner].wet ? 18 : 10;
			speed = underwaterSpeed;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SpiritMod/Items/Equipment/KelpHookChain");
			Vector2 vector = projectile.Center;
			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			Rectangle? sourceRectangle = null;
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float num = texture.Height;
			Vector2 vector2 = mountedCenter - vector;
			float rotation = (float)Math.Atan2(vector2.Y, vector2.X) - 1.57f;
			bool flag = true;
			if (float.IsNaN(vector.X) && float.IsNaN(vector.Y)) {
				flag = false;
			}
			if (float.IsNaN(vector2.X) && float.IsNaN(vector2.Y)) {
				flag = false;
			}
			while (flag) {
				if (vector2.Length() < num + 1.0) {
					flag = false;
				}
				else {
					Vector2 value = vector2;
					value.Normalize();
					vector += value * num;
					vector2 = mountedCenter - vector;
					Color color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16.0));
					color = projectile.GetAlpha(color);
					Main.spriteBatch.Draw(texture, vector - Main.screenPosition, sourceRectangle, color, rotation, origin, 1f, SpriteEffects.None, 0f);
				}
			}
		}
	}
}