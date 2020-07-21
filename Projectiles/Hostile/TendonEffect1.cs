using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class TendonEffect1 : ModProjectile
	{
		Vector2 direction9 = Vector2.Zero;
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flesh Tendon");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.hide = true;
			projectile.timeLeft = 100000;
			projectile.tileCollide = true;
			projectile.alpha = 0;
		}
		bool stuck = false;
		bool typeChain = false;
		public override void AI()
		{
			int num1 = ModContent.NPCType<CrimsonTrapper>();
			if (!Main.npc[(int)projectile.ai[1]].active) {
				projectile.timeLeft = 0;
				projectile.active = false;
			}
			if (!stuck) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			}
			if (stuck) {
				projectile.velocity = Vector2.Zero;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			{
				NPC parent = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<CrimsonTrapper>())];
				Vector2 direction9 = parent.Center - projectile.Center;
				direction9.Normalize();
				//	direction9 *= 6;
				ProjectileExtras.DrawChain(projectile.whoAmI, parent.Center,
				"SpiritMod/Projectiles/Hostile/TendonEffect1_Chain");
			}
			return false;

		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
			stuck = true;
			if (oldVelocity.X != projectile.velocity.X) //if its an X axis collision
			{
				if (projectile.velocity.X > 0) {
					projectile.rotation = 1.57f;
				}
				else {
					projectile.rotation = 4.71f;
				}
			}
			if (oldVelocity.Y != projectile.velocity.Y) //if its a Y axis collision
			{
				if (projectile.velocity.Y > 0) {
					projectile.rotation = 3.14f;
				}
				else {
					projectile.rotation = 0f;
				}
			}
		}
	}
}