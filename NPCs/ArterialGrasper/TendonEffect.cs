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
		int TrapperID => ModContent.NPCType<CrimsonTrapper>();

		public override void SetStaticDefaults() => DisplayName.SetDefault("Flesh Tendon");

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.alpha = 0;
			Projectile.extraUpdates = 1;
		}

		bool stuck = false;

		public override void AI()
		{
			if (!Main.npc[(int)Projectile.ai[1]].active || Main.npc[(int)Projectile.ai[1]].type != TrapperID)
			{
				Projectile.Kill();
				return;
			}

			Projectile.timeLeft++;

			if (!stuck)
				Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

			else
				Projectile.velocity = Vector2.Zero;

		}
		public override bool PreDraw(ref Color lightColor)
		{
			NPC parent = Main.npc[(int)Projectile.ai[1]];
			if (Main.npc[(int)Projectile.ai[1]].active && Main.npc[(int)Projectile.ai[1]].type == TrapperID)
			{
				Vector2 direction9 = Vector2.Normalize(parent.Center - Projectile.Center);
				ProjectileExtras.DrawChain(Projectile.whoAmI, parent.Center,
				"SpiritMod/NPCs/ArterialGrasper/" + Name + "_Chain");
			}
			return false;

		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI) => drawCacheProjsBehindNPCsAndTiles.Add(index);

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			stuck = true;
			if (oldVelocity.X != Projectile.velocity.X) //if its an X axis collision
			{
				if (Projectile.velocity.X > 0)
					Projectile.rotation = 1.57f;
				else
					Projectile.rotation = 4.71f;
			}
			if (oldVelocity.Y != Projectile.velocity.Y) //if its a Y axis collision
			{
				if (Projectile.velocity.Y > 0)
					Projectile.rotation = 3.14f;
				else
					Projectile.rotation = 0f;
			}
			return false;
		}
	}
}