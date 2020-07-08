using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items.Equipment
{
	internal class MagnetHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnet Hook");
			Tooltip.SetDefault("Homes in on nearby blocks");
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
			item.shootSpeed = 12f; // how quickly the hook is shot.
			item.shoot = ProjectileType<MagnetHookProjectile>();
		}
	}

	internal class MagnetHookProjectile : ModProjectile
	{
		public override void SetStaticDefaults() 
			=> DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");

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
			projectile.timeLeft = 400;
		}

		// Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
		public override bool? CanUseGrapple(Player player)
		{
			int hooksOut = 0;
			for(int l = 0; l < 1000; l++) {
				if(Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type) {
					hooksOut++;
				}
			}
			if(hooksOut > 0) // This hook can have 1 hooks out.
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
		public override float GrappleRange() => 350f;

		public override void NumGrappleHooks(Player player, ref int numHooks) => numHooks = 1;

		// default is 11, Lunar is 24
		bool retracting = false;
		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 13f;
			retracting = true;
		}

		public override void GrapplePullSpeed(Player player, ref float speed) => speed = 12;

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SpiritMod/Items/Equipment/MagnetHookChain");
			Vector2 vector = projectile.Center;
			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			Rectangle? sourceRectangle = null;
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float num = texture.Height;
			Vector2 vector2 = mountedCenter - vector;
			float rotation = (float)Math.Atan2(vector2.Y, vector2.X) - 1.57f;
			bool flag = true;
			if(float.IsNaN(vector.X) && float.IsNaN(vector.Y)) {
				flag = false;
			}
			if(float.IsNaN(vector2.X) && float.IsNaN(vector2.Y)) {
				flag = false;
			}
			while(flag) {
				if(vector2.Length() < num + 1.0) {
					flag = false;
				} else {
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
		public override void AI()
		{
			int num = 5;
			for(int k = 0; k < 1; k++) {
				int index2 = Dust.NewDust(new Vector2(projectile.Center.X + 15, projectile.Center.Y), 1, 1, 180, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
			for(int j = 0; j < 1; j++) {
				int index2 = Dust.NewDust(new Vector2(projectile.Center.X - 15, projectile.Center.Y), 1, 1, 130, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * j;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
			{
				float lowestDist = float.MaxValue;
				int tilepositionx = (int)(projectile.position.X / 16);
				int tilepositiony = (int)(projectile.position.Y / 16);
				int targetpositionx = 0;
				int targetpositiony = 0;
				int range = 8;
				for(int i = tilepositionx - 5; i < tilepositionx + 5; i++) {
					for(int j = tilepositiony - 5; j < tilepositiony + 5; j++) {
						Tile tile = Main.tile[i, j];
						if(tile.active() && Main.tileSolid[tile.type]) {
							//if npc is within 50 blocks
							float dist = projectile.Distance(new Vector2(i * 16, j * 16));
							if(dist / 16 < range) {
								//if npc is closer than closest found npc
								if(dist < lowestDist + 32) {
									lowestDist = dist;
									targetpositionx = i * 16;
									targetpositiony = j * 16;
								}
							}
						}
					}
				}
				if(lowestDist < 150 && projectile.timeLeft < 388 && !retracting) {
					Vector2 direction = new Vector2(targetpositionx - projectile.position.X, targetpositiony - projectile.position.Y);
					direction.Normalize();
					projectile.velocity = direction * (int)Math.Sqrt((projectile.velocity.X * projectile.velocity.X) + (projectile.velocity.Y * projectile.velocity.Y));
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