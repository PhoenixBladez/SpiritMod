using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class PalladiumStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Rune Wall");

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 40;
			Projectile.height = 60;
			Projectile.penetrate = 10;
			Projectile.hide = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 660;
		}
		bool hitGround = false;
		int timer;
		int alphaCounter;
		int effectTimer;
		public override bool? CanHitNPC(NPC target) => hitGround;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			hitGround = true;
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (hitGround) {
				Main.spriteBatch.Draw(TextureAssets.Extra[60].Value, new Vector2((int)Projectile.position.X - (int)Main.screenPosition.X - 44, (int)Projectile.position.Y - (int)Main.screenPosition.Y - 22), null, new Color(252 + alphaCounter * 2, 152 + alphaCounter, 3, 0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
			=> drawCacheProjsBehindNPCsAndTiles.Add(index);
		public override void AI()
		{
			Lighting.AddLight(Projectile.position, 0.5f, .5f, .4f);
			timer++;
			if (timer <= 70) {
				alphaCounter += 2;
			}
			if (timer > 70) {
				alphaCounter -= 2;
			}
			if (timer >= 140) {
				timer = 0;
			}
			effectTimer++;
			if (effectTimer >= 50) {
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X + Main.rand.Next(-27, 17), Projectile.Center.Y + 22, Projectile.velocity.X + Main.rand.Next(-2, 2), Projectile.velocity.Y + Main.rand.Next(-2, -1), ModContent.ProjectileType<PalladiumRuneEffect>(), 0, 0, Projectile.owner, 0f, 0f);
				Main.projectile[p].scale = Main.rand.NextFloat(.4f, .8f);
				Main.projectile[p].frame = Main.rand.Next(0, 8);
				effectTimer = 0;
			}
			Player player = Main.LocalPlayer;
			int distance1 = (int)Vector2.Distance(Projectile.Center, player.Center);
			if (distance1 < 53) {
				if (player.statLife <= player.statLifeMax / 3) {
					player.AddBuff(BuffID.RapidHealing, 300);
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, -2f, 0, Color.White, 2f);
				Main.dust[num].noLight = true;
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X += ((Main.rand.Next(-40, 41) / 20) - 1.5f);
				dust.position.Y += ((Main.rand.Next(-40, 41) / 20) - 1.5f);
				if (Main.dust[num].position != Projectile.Center) {
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}
	}
}