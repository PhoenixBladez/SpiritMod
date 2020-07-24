using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
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
			projectile.friendly = true;
			projectile.magic = true;
			projectile.width = 40;
			projectile.height = 60;
			projectile.penetrate = 10;
			projectile.hide = true;
			projectile.alpha = 255;
			projectile.timeLeft = 660;
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
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (hitGround) {
				Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_60"), new Vector2((int)projectile.position.X - (int)Main.screenPosition.X - 44, (int)projectile.position.Y - (int)Main.screenPosition.Y - 22), null, new Color(252 + alphaCounter * 2, 152 + alphaCounter, 3, 0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
			=> drawCacheProjsBehindNPCsAndTiles.Add(index);
		public override void AI()
		{
			Lighting.AddLight(projectile.position, 0.5f, .5f, .4f);
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
				int p = Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-27, 17), projectile.Center.Y + 22, projectile.velocity.X + Main.rand.Next(-2, 2), projectile.velocity.Y + Main.rand.Next(-2, -1), ModContent.ProjectileType<PalladiumRuneEffect>(), 0, 0, projectile.owner, 0f, 0f);
				Main.projectile[p].scale = Main.rand.NextFloat(.4f, .8f);
				Main.projectile[p].frame = Main.rand.Next(0, 8);
				effectTimer = 0;
			}
			Player player = Main.LocalPlayer;
			int distance1 = (int)Vector2.Distance(projectile.Center, player.Center);
			if (distance1 < 53) {
				if (player.statLife <= player.statLifeMax / 3) {
					player.AddBuff(BuffID.RapidHealing, 300);
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 158, 0f, -2f, 0, Color.White, 2f);
				Main.dust[num].noLight = true;
				Main.dust[num].noGravity = true;
				Dust expr_62_cp_0 = Main.dust[num];
				expr_62_cp_0.position.X += ((Main.rand.Next(-40, 41) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y += ((Main.rand.Next(-40, 41) / 20) - 1.5f);
				if (Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
	}
}