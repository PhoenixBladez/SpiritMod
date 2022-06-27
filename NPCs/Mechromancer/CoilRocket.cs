using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Mechromancer
{
	public class CoilRocket : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Rocket");
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;       //projectile width
			Projectile.height = 24;  //projectile height
			Projectile.friendly = false;      //make that the projectile will not damage you
			Projectile.hostile = true;        // 
			Projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
			Projectile.penetrate = 1;      //how many npc will penetrate
			Projectile.timeLeft = 300;   //how many time projectile projectile has before disepire
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;
		}
		int numHits;
		public override bool PreAI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list) {
				if (Projectile != proj && proj.friendly) {
					numHits++;
					if (numHits >= 3) {
						Projectile.Kill();
					}
				}
			}
			return true;
		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] == 16f) {
				Projectile.localAI[0] = 0f;
				for (int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Electric, 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = Projectile.Center + vector2;
					Main.dust[num8].velocity = Projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			float num = 1f - (float)Projectile.alpha / 255f;
			num *= Projectile.scale;

			float num1 = 5.25f;
			float num2 = 3f;
			float num3 = 16f;
			num1 = 9f;
			num2 = 5.5f;
			if (Projectile.timeLeft > 30 && Projectile.alpha > 0)
				Projectile.alpha -= 25;
			if (Projectile.timeLeft > 30 && Projectile.alpha < 128 && Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				Projectile.alpha = 128;
			if (Projectile.alpha < 0)
				Projectile.alpha = 0;

			if (++Projectile.frameCounter > 4) {
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 4)
					Projectile.frame = 0;
			}

			++Projectile.ai[1];
			double num5 = (double)Projectile.ai[1] / 180.0;

			int index1 = (int)Projectile.ai[0];
			if (index1 >= 0 && Main.player[index1].active && !Main.player[index1].dead) {
				if (Projectile.Distance(Main.player[index1].Center) <= num3)
					return;
				Vector2 unitY = Projectile.DirectionTo(Main.player[index1].Center);
				if (unitY.HasNaNs())
					unitY = Vector2.UnitY;
				Projectile.velocity = (Projectile.velocity * (num1 - 1f) + unitY * num2) / num1;
			}
			else {
				if (Projectile.timeLeft > 30)
					Projectile.timeLeft = 30;
				if (Projectile.ai[0] == -1f)
					return;
				Projectile.ai[0] = -1f;
				Projectile.netUpdate = true;
			}
			int num1222 = 5;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Torch, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num1222 * (float)k;
				Main.dust[index2].scale = .95f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 300);
			Projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 40; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, -2f, 0, default, 1.5f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != Projectile.Center) {
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}

	}
}
