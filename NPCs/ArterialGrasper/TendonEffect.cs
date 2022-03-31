using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.NPCs.ArterialGrasper
{
	public class TendonEffect : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Flesh Tendon");

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.alpha = 0;
			projectile.extraUpdates = 1;
		}

		bool stuck = false;

		readonly int num1 = ModContent.NPCType<CrimsonTrapper>();
		public override void AI()
		{
			if (!Main.npc[(int)projectile.ai[1]].active || Main.npc[(int)projectile.ai[1]].type != num1)
			{
				projectile.Kill();
				return;
			}

			projectile.timeLeft++;

			if (!stuck)
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			else
				projectile.velocity = Vector2.Zero;

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			NPC parent = Main.npc[(int)projectile.ai[1]];
			if (Main.npc[(int)projectile.ai[1]].active && Main.npc[(int)projectile.ai[1]].type == num1)
			{
				Vector2 direction9 = Vector2.Normalize(parent.Center - projectile.Center);
				ProjectileExtras.DrawChain(projectile.whoAmI, parent.Center,
				"SpiritMod/NPCs/ArterialGrasper/" + Name + "_Chain");
			}
			return false;

		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI) => drawCacheProjsBehindNPCsAndTiles.Add(index);

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			stuck = true;
			if (oldVelocity.X != projectile.velocity.X) //if its an X axis collision
			{
				if (projectile.velocity.X > 0)
					projectile.rotation = 1.57f;
				else
					projectile.rotation = 4.71f;
			}
			if (oldVelocity.Y != projectile.velocity.Y) //if its a Y axis collision
			{
				if (projectile.velocity.Y > 0)
					projectile.rotation = 3.14f;
				else
					projectile.rotation = 0f;
			}
			return false;
		}
	}
}