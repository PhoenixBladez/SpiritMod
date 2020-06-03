using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items.Equipment
{
	internal class MagnetHook : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Magnet Hook");
		}

		public override void SetDefaults() {
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
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
		}

		public override void SetDefaults() {
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
		public override bool? CanUseGrapple(Player player) {
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
		public override bool? SingleGrappleHook(Player player)
		{
			return true;
		}

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
		public override float GrappleRange() {
			return 350f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks) {
			numHooks = 1;
		}

		// default is 11, Lunar is 24
		public override void GrappleRetreatSpeed(Player player, ref float speed) {
			speed = 11f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed) {
			speed = 9;
		}
		public override void AI()
		{
			float lowestDist = float.MaxValue;
			int tilepositionx = (int)(projectile.position.X / 16);
			int tilepositiony = (int)(projectile.position.Y / 16);
			int targetpositionx = 0;
			int targetpositiony = 0;
			int range = 5;
			for (int i = tilepositionx - 5; i < tilepositionx + 5; i++)
			{
				for (int j = tilepositiony - 5; j < tilepositiony + 5; j++)
				{
					Tile tile = Main.tile[i,j];
					if (tile.active() && Main.tileSolid[tile.type])
					{
						//if npc is within 50 blocks
						float dist = projectile.Distance(new Vector2(i * 16,j * 16));
						if (dist / 16 < range)
						{
							//if npc is closer than closest found npc
							if (dist < lowestDist + 32)
							{
								lowestDist = dist;
								targetpositionx = i * 16;
								targetpositiony = j * 16;
							}
						}
					}
				}
			}
			if (lowestDist < 150 && projectile.timeLeft > 366 && projectile.timeLeft < 385)
			{
				Vector2 direction = new Vector2(targetpositionx - projectile.position.X, targetpositiony - projectile.position.Y);
				direction.Normalize();
				projectile.velocity = direction * (int)Math.Sqrt((projectile.velocity.X * projectile.velocity.X) + (projectile.velocity.Y * projectile.velocity.Y));
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
			Vector2 playerCenter = Main.player[projectile.owner].MountedCenter;
			Vector2 center = projectile.Center;
			Vector2 distToProj = playerCenter - projectile.Center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();
			while (distance > 30f && !float.IsNaN(distance)) {
				distToProj.Normalize();                 //get unit vector
				distToProj *= 24f;                      //speed = 24
				center += distToProj;                   //update draw position
				distToProj = playerCenter - center;    //update distance
				distance = distToProj.Length();
				Color drawColor = lightColor;

				//Draw chain
				spriteBatch.Draw(mod.GetTexture("Items/Equipment/MagnetHookChain"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, Main.chain30Texture.Width, Main.chain30Texture.Height), drawColor, projRotation,
					new Vector2(Main.chain30Texture.Width * 0.5f, Main.chain30Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
			}
			return true;
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
