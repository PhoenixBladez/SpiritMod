using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.NPCs.BlueMoon.GlowToad
{
	public class GlowToad : ModNPC
	{
		//TODO:
		//Get animation
		//smoother head rotation
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glow Toad");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 50;
			npc.damage = 100;
			npc.defense = 50;
			npc.lifeMax = 1080;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 2000f;
			npc.knockBackResist = 0.5f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 187, hitDirection, -1f, 1, default(Color), .81f);
				Dust.NewDust(npc.position, npc.width, npc.height, 205, hitDirection, -1f, 1, default(Color), .51f);
			}
			if (npc.life <= 0) {
				for (int k = 0; k < 11; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 187, hitDirection, -1f, 1, default(Color), .81f);
					Dust.NewDust(npc.position, npc.width, npc.height, 205, hitDirection, -1f, 1, default(Color), .71f);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GlowToad/GlowToad1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GlowToad/GlowToad2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GlowToad/GlowToad3"), 1f);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return MyWorld.BlueMoon && NPC.CountNPCS(ModContent.NPCType<GlowToad>()) < 4 && spawnInfo.player.ZoneOverworldHeight ? .6f : 0f;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(tongueOut);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			tongueOut = reader.ReadBoolean();
		}
		bool tongueOut = false;
		bool mouthOpen = false;
		float headRotation;
		int tongueCooldown = 300;
		int tongueProj = -1;
		Vector2 offset;

		private void tongueStuff(Player player, Vector2 dir)
		{
			offset = npc.Center;
			offset.X += npc.direction * 15;
			offset.Y += dir.Y * 12;
			tongueCooldown--;
			if (tongueCooldown < 0 && npc.velocity.Y == 0)
			{
				tongueOut = true;
				tongueCooldown = 300;
			}
			mouthOpen = tongueOut || tongueCooldown < 60;
			if (tongueOut)
			{
				if (tongueProj == -1)
				{
					tongueProj = Projectile.NewProjectile(offset + dir * 5, dir * 30, ModContent.ProjectileType<GlowToadTongue>(), 100, 0, player.whoAmI, npc.whoAmI, offset.Y);
				}
				else if (!Main.projectile[tongueProj].active)
				{
					tongueProj = -1;
					tongueOut = false;
				}
			}
		}
		int direction;
		int jumpCounter = 0;
		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			Vector2 dir = player.Center - npc.Center;
			dir.Normalize();
			if (!npc.collideY) {
				npc.velocity.X *= 1.045f;
			}
			if (!tongueOut && tongueCooldown >= 60 && (npc.velocity.Y == 0 || npc.collideY))
			{
				//movement
				if (jumpCounter >= 45)
				{
					npc.velocity.Y = -7;
					npc.velocity.X = npc.direction * 10;
					jumpCounter = 0;
				}
				jumpCounter++;
			}
			if (!tongueOut)
			{
				direction = npc.direction;
				headRotation = dir.ToRotation();
				if (npc.direction == -1)
				{
					headRotation += 3.14f;
				}
			}
			else
			{
				npc.direction = direction;
			}

			tongueStuff(player, dir);
		}
		//int tongueDirection = 0;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);

			Texture2D headTexture = SpiritMod.instance.GetTexture("NPCs/BlueMoon/GlowToad/GlowToad_Head");
			Vector2 headOffset = new Vector2(npc.direction == -1 ? 25 : headTexture.Width - 25,20);
			spriteBatch.Draw(headTexture, npc.position - Main.screenPosition + headOffset, new Rectangle(0,mouthOpen ? 52 : 0, headTexture.Width, headTexture.Height / 2),
							 drawColor, headRotation, headOffset, npc.scale, effects, 0);

			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(SpiritMod.instance.GetTexture("NPCs/BlueMoon/GlowToad/GlowToad_Glow"), npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			Texture2D headTexture = SpiritMod.instance.GetTexture("NPCs/BlueMoon/GlowToad/GlowToad_HeadGlow");
			Vector2 headOffset = new Vector2(npc.direction == -1 ? 25 : headTexture.Width - 25,20);
			spriteBatch.Draw(headTexture, npc.position - Main.screenPosition + headOffset, new Rectangle(0,mouthOpen ? 52 : 0, headTexture.Width, headTexture.Height / 2),
							 Color.White, headRotation, headOffset, npc.scale, effects, 0);
		}
	}
	public class GlowToadTongue : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glow Tongue");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.damage = 0;
            projectile.timeLeft = 100;
        }
		int speed = -30;
		Vector2 origin = Vector2.Zero;
		Vector2 dir;
        public override void AI()
        {
            NPC parent = Main.npc[(int)projectile.ai[0]];
			if (speed > 0 && projectile.Hitbox.Intersects(parent.Hitbox))
				projectile.active = false;
			
			origin = parent.Center;
			origin.X += parent.direction * 15;
			origin.Y = projectile.ai[1];
			dir = origin - projectile.Center;
			dir.Normalize();
			projectile.rotation = dir.ToRotation() - 1.57f;
			projectile.velocity = dir * speed;
			speed+= 1;
        }
        public override void Kill(int timeLeft)
        {
           
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawChain(projectile.whoAmI, origin,
				"SpiritMod/NPCs/BlueMoon/GlowToad/GlowToadTongue_Chain", white:true);
				ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, Color.White);
			spriteBatch.Draw(ModContent.GetTexture("SpiritMod/NPCs/BlueMoon/GlowToad/GlowToadTongue_ChainEnd"), origin - Main.screenPosition, null, Color.White, dir.ToRotation() - 1.57f, new Vector2(7, 5), projectile.scale, SpriteEffects.None, 0f); 
			return false;
		}
    }
}
