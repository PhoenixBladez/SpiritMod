using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
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
		private int Cycle => (int)(Projectile.ai[0] % cycletimer);
		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = cycletimer * 4;
			Projectile.height = 6;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 6;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
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
				points[0] = Projectile.Center;
				Projectile.extraUpdates = 10;
				float maxdist = 400;
				var validtargets = Main.npc.Where(x => x.CanBeChasedBy(this) && Collision.CanHit(x.Center, 0, 0, Projectile.Center, 0, 0) && x.active && x != null && x.Distance(Projectile.Center) < maxdist);
				if (validtargets.Any() && Projectile.ai[0] > 0) { //check if a target is in range, and if it's not the initial cycle
					NPC Finaltarget = null;
					foreach (NPC npc in validtargets) {
						if (npc.Distance(Projectile.Center) < maxdist) {
							maxdist = npc.Distance(Projectile.Center);
							Finaltarget = npc;
						}
					}
					if (Finaltarget != null) {
						Projectile.ai[1] = Projectile.DirectionTo(Finaltarget.Center).ToRotation();
						SetPoint(Projectile.DirectionTo(Finaltarget.Center));
					}
				}
				else
					SetPoint(Vector2.UnitX.RotatedBy(Projectile.ai[1]));

				for (int i = 0; i < 2; i++) 
					DustHelper.DrawElectricity(points[0], points[1], DustID.GoldCoin, 1f, 24);

				Projectile.Center = points[1];
				Projectile.netUpdate = true;
			}
			else if (Cycle >= (cycletimer / 3)) { //pause ai
				Projectile.extraUpdates = 0;
				for (int i = 0; i < 3; i++) {
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.GoldCoin);
					dust.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) / 3;
				}
			}
			Projectile.ai[0]++;
		}
		private void SetPoint(Vector2 direction)
		{
			float[] samplearray = new float[3];
			float maxdistance = Main.rand.NextFloat(200, 400);
			Collision.LaserScan(Projectile.Center, direction, 0, maxdistance, samplearray);
			maxdistance = 0;
			foreach(float sample in samplearray) 
				maxdistance += sample / samplearray.Length;

			if (maxdistance <= 16) {
				Projectile.Kill();
				return;
			}
			SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
			points[1] = Projectile.Center + (direction * maxdistance);
		}
		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Cycle == 1;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.Center.ToVector2() - targetHitbox.Size() / 2, targetHitbox.Size(), points[0], points[1]);

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(10) == 0)
				target.AddBuff(Mod.Find<ModBuff>("ElectrifiedV2").Type, 120, true);
		}

	}
}
