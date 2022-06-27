using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpiritMod.NPCs.Tides
{
	public class RyTentacle : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tentacle Spike");
			ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 16;
			Projectile.height = 30;
			Projectile.hide = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 999;
			Projectile.tileCollide = true;
			//	projectile.extraUpdates = 1;
		}
		bool activated = false;
		public override bool PreAI()
		{
			Projectile.velocity.X = 0;
			if (!activated) {
				Projectile.velocity.Y = 24;
			}
			else {
				Projectile.ai[1]++;
				//projectile.extraUpdates = 0;
				if (Projectile.ai[1] < 50) {
					Projectile.timeLeft = 60;
					for (float i = Projectile.position.Y + 30; i > Projectile.position.Y - 125; i -= 10) {
						Dust.NewDustPerfect(new Vector2(Projectile.Center.X, i), 173).noGravity = true;
					}
					Projectile.velocity = Vector2.Zero;
				}
				else if (Projectile.ai[1] < 60) {
					Projectile.hostile = true;
					Projectile.velocity.Y = -10;
				}
				else {
					Projectile.velocity.Y = 3;
					Projectile.alpha += 10;
				}
				if (Projectile.ai[1] == 50) {
					SoundEngine.PlaySound(SoundID.Item, Projectile.Center, 103);
				}
				if (Projectile.ai[1] == 55) {
					Projectile.alpha = 0;
				}
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y != Projectile.velocity.Y && !activated) {
				activated = true;
				Projectile.tileCollide = false;
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}
	}
}

