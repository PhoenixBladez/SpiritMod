using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Projectiles
{
	class EelOrb : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Electric Orb");

		readonly Vector2[] points = new Vector2[] { Vector2.Zero, Vector2.Zero };
		private static readonly int cycletimer = 50;
		private int Cycle => (int)(projectile.ai[0] % cycletimer);
		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = cycletimer * 4;
			projectile.height = 6;
			projectile.magic = true;
			projectile.width = 6;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			foreach(Vector2 point in points) {
				writer.WriteVector2(point);
			}
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			for(int i = 0; i < points.Count(); i++) {
				points[i] = reader.ReadVector2();
			}
		}
		public override void AI()
		{
			if(Cycle == 0) { //look for point
				points[0] = projectile.Center;
				projectile.extraUpdates = 10;
				float maxdist = 400;
				var validtargets = Main.npc.Where(x => x.CanBeChasedBy(this) && Collision.CanHit(x.Center, 0, 0, projectile.Center, 0, 0) && x.active && x != null && x.Distance(projectile.Center) < maxdist);
				if (validtargets.Any() && projectile.ai[0] > 0) { //check if a target is in range, and if it's not the initial cycle
					NPC Finaltarget = null;
					foreach (NPC npc in validtargets) {
						if (npc.Distance(projectile.Center) < maxdist) {
							maxdist = npc.Distance(projectile.Center);
							Finaltarget = npc;
						}
					}
					if (Finaltarget != null) {
						projectile.ai[1] = projectile.DirectionTo(Finaltarget.Center).ToRotation();
						SetPoint(projectile.DirectionTo(Finaltarget.Center));
					}
				}
				else
					SetPoint(Vector2.UnitX.RotatedBy(projectile.ai[1]));

				for (int i = 0; i < 2; i++) 
					DustHelper.DrawElectricity(points[0], points[1], DustID.GoldCoin, 1f, 24);

				projectile.Center = points[1];
				projectile.netUpdate = true;
			}
			else if (Cycle >= (cycletimer / 3)) { //pause ai
				projectile.extraUpdates = 0;
				for (int i = 0; i < 3; i++) {
					Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.GoldCoin);
					dust.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) / 3;
				}
			}
			projectile.ai[0]++;
		}
		private void SetPoint(Vector2 direction)
		{
			float[] samplearray = new float[3];
			float maxdistance = Main.rand.NextFloat(200, 400);
			Collision.LaserScan(projectile.Center, direction, 0, maxdistance, samplearray);
			maxdistance = 0;
			foreach(float sample in samplearray) 
				maxdistance += sample / samplearray.Length;

			if (maxdistance <= 16) {
				projectile.Kill();
				return;
			}
			Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 122, 0.7f);
			points[1] = projectile.Center + (direction * maxdistance);
		}
		public override bool CanDamage() => Cycle == 1;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.Center.ToVector2() - targetHitbox.Size() / 2, targetHitbox.Size(), points[0], points[1]);

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(10) == 0)
				target.AddBuff(mod.BuffType("ElectrifiedV2"), 120, true);
		}

	}
}
