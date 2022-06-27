
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class VineChainProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vine Chain");
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 900;
			Projectile.DamageType = DamageClass.Melee;
		}
		//  bool comingHome = false;
		int hookednpc = 0;
		bool hooked = false;
		float returnSpeed = 7;
		public override bool PreAI()
		{
			if (Projectile.Hitbox.Intersects(Main.player[Projectile.owner].Hitbox) && Projectile.timeLeft < 870) {
				Projectile.active = false;
			}
			if (Projectile.timeLeft < 859) {
				Vector2 direction9 = Main.player[Projectile.owner].Center - Projectile.position;
				direction9.Normalize();
				Projectile.velocity = direction9 * returnSpeed;
				returnSpeed += 0.2f;
				Projectile.rotation = Projectile.velocity.ToRotation() - 1.57f;
			}
			else {
				Projectile.velocity -= new Vector2(Projectile.ai[0], Projectile.ai[1]) / 40f;
				Projectile.rotation = new Vector2(Projectile.ai[0], Projectile.ai[1]).ToRotation() + 1.57f;
			}
			Player player = Main.player[Projectile.owner];
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			if (!player.channel || hooked) {
				Projectile.timeLeft = 858;
				Projectile.friendly = false;
			}
			NPC npc = Main.npc[hookednpc];
			if (hooked && npc.active && player.channel) {
				int distance = (int)Math.Sqrt((npc.Center.X - player.Center.X) * (npc.Center.X - player.Center.X) + (npc.Center.Y - player.Center.Y) * (npc.Center.Y - player.Center.Y));
				if (distance > 100 && Projectile.Distance(npc.Center) < 50) {
					npc.velocity = Projectile.velocity;
					npc.netUpdate = true;
				}
				else {
					hooked = false;
				}
			}
			return false;

		}
		//projectile.ai[0]: X speed initial
		//projectile.ai[1]: y speed initial

		public override bool PreDraw(ref Color lightColor)
		{
			ProjectileExtras.DrawChain(Projectile.whoAmI, Main.player[Projectile.owner].MountedCenter,
			"SpiritMod/Projectiles/Flail/VineChain_Chain");
			ProjectileExtras.DrawAroundOrigin(Projectile.whoAmI, lightColor);
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.boss && target.knockBackResist != 0) {
				hooked = true;
				hookednpc = target.whoAmI;
				target.position = Projectile.position - new Vector2((target.width / 2), (target.height / 2));
				target.netUpdate = true;
				//  target.velocity = projectile.velocity;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.timeLeft >= 859) {
				Projectile.timeLeft = 800;
				Projectile.friendly = false;
			}
			else if (hooked) {
				NPC npc = Main.npc[hookednpc];
				if (npc.noTileCollide) {
					Projectile.tileCollide = false;
				}
				else {
					hooked = false;
					Projectile.tileCollide = false;
				}
			}
			else {
				Projectile.tileCollide = false;
			}
			return false;
		}
	}
}
