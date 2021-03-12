using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items.Tool
{
	internal class ThornHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bramble Hook");
		}

		public override void SetDefaults()
		{
			/*
				this.noUseGraphic = true;
				this.damage = 0;
				this.knockBack = 7f;
				this.useStyle = 5;
				this.name = "Amethyst Hook";
				this.shootSpeed = 10f;
				this.shoot = 230;
				this.width = 18;
				this.height = 28;
				this.useSound = 1;
				this.useAnimation = 20;
				this.useTime = 20;
				this.rare = 1;
				this.noMelee = true;
				this.value = 20000;
			*/
			// Instead of copying these values, we can clone and modify the ones we want to copy
			item.CloneDefaults(ItemID.AmethystHook);
			item.shootSpeed = 16f; // how quickly the hook is shot.
			item.shoot = ProjectileType<ThornHookProj>();
		}
	}

	internal class ThornHookProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bramble Hook");
		}

		public override void SetDefaults()
		{
			/*	this.netImportant = true;
				this.name = "Gem Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			*/
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
		}

		// Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
		public override bool? CanUseGrapple(Player player)
		{
			int hooksOut = 0;
			for (int l = 0; l < 1000; l++) {
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type) {
					hooksOut++;
				}
			}
			if (hooksOut > 0) // This hook can have 1 hooks out.
			{
				return false;
			}
			return true;
		}

		// Return true if it is like: Hook, CandyCaneHook, BatHook, GemHooks
		//public override bool? SingleGrappleHook(Player player)
		//{
		//	return true;
		//}

		// Use this to kill oldest hook. For hooks that kill the oldest when shot, not when the newest latches on: Like SkeletronHand
		// You can also change the projectile like: Dual Hook, Lunar Hook
		//public override void UseGrapple(Player player, ref int type)
		//{
		//	int hooksOut = 0;
		//	int oldestHookIndex = -1;
		//	int oldestHookTimeLeft = 100000;
		//	for (int i = 0; i < 1000; i++)
		//	{
		//		if (Main.projectile[i].active && Main.projectile[i].owner == projectile.whoAmI && Main.projectile[i].type == projectile.type)
		//		{
		//			hooksOut++;
		//			if (Main.projectile[i].timeLeft < oldestHookTimeLeft)
		//			{
		//				oldestHookIndex = i;
		//				oldestHookTimeLeft = Main.projectile[i].timeLeft;
		//			}
		//		}
		//	}
		//	if (hooksOut > 1)
		//	{
		//		Main.projectile[oldestHookIndex].Kill();
		//	}
		//}

		// Amethyst Hook is 300, Static Hook is 600
		public override float GrappleRange()
		{
			return 400f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 1;
		}

		// default is 11, Lunar is 24
		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 11f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			speed = 9.6f;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SpiritMod/Items/Tool/ThornHook_Chain");
			Vector2 vector = projectile.Center;
			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			Rectangle? sourceRectangle = null;
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
			float num = (float)texture.Height;
			Vector2 vector2 = mountedCenter - vector;
			float rotation = (float)Math.Atan2((double)vector2.Y, (double)vector2.X) - 1.57f;
			bool flag = true;
			if (float.IsNaN(vector.X) && float.IsNaN(vector.Y)) {
				flag = false;
			}
			if (float.IsNaN(vector2.X) && float.IsNaN(vector2.Y)) {
				flag = false;
			}
			while (flag) {
				if ((double)vector2.Length() < (double)num + 1.0) {
					flag = false;
				}
				else {
					Vector2 value = vector2;
					value.Normalize();
					vector += value * num;
					vector2 = mountedCenter - vector;
					Color color = Lighting.GetColor((int)vector.X / 16, (int)((double)vector.Y / 16.0));
					color = projectile.GetAlpha(color);
					Main.spriteBatch.Draw(texture, vector - Main.screenPosition, sourceRectangle, color, rotation, origin, 1f, SpriteEffects.None, 0f);
				}
			}
		}
	}

	// Animated hook example
	// Multiple, 
	// only 1 connected, spawn mult
	// Light the path
	// Gem Hooks: 1 spawn only
	// Thorn: 4 spawns, 3 connected
	// Dual: 2/1 
	// Lunar: 5/4 -- Cycle hooks, more than 1 at once
	// AntiGravity -- Push player to position
	// Static -- move player with keys, don't pull to wall
	// Christmas -- light ends
	// Web slinger -- 9/8, can shoot more than 1 at once
	// Bat hook -- Fast reeling

}
