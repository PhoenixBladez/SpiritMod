using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Dusking
{
	public class Shadowflamer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antumbral Skull");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = npc.height = 30;
			npc.lifeMax = 70;
			npc.damage = 45;
			npc.knockBackResist = 0;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.friendly = false;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override bool PreAI()
		{
			npc.TargetClosest(true);
			float speed = 8f;
			float acceleration = 0.13f;
			Vector2 vector2 = new Vector2(npc.position.X + npc.width * 0.5F, npc.position.Y + npc.height * 0.5F);
			float xDir = Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5F) - vector2.X;
			float yDir = Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5F) - vector2.Y;
			float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);

			float num10 = speed / length;
			xDir = xDir * num10;
			yDir = yDir * num10;
			if (npc.velocity.X < xDir) {
				npc.velocity.X = npc.velocity.X + acceleration;
				if (npc.velocity.X < 0 && xDir > 0)
					npc.velocity.X = npc.velocity.X + acceleration;
			}
			else if (npc.velocity.X > xDir) {
				npc.velocity.X = npc.velocity.X - acceleration;
				if (npc.velocity.X > 0 && xDir < 0)
					npc.velocity.X = npc.velocity.X - acceleration;
			}

			if (npc.velocity.Y < yDir) {
				npc.velocity.Y = npc.velocity.Y + acceleration;
				if (npc.velocity.Y < 0 && yDir > 0)
					npc.velocity.Y = npc.velocity.Y + acceleration;
			}
			else if (npc.velocity.Y > yDir) {
				npc.velocity.Y = npc.velocity.Y - acceleration;
				if (npc.velocity.Y > 0 && yDir < 0)
					npc.velocity.Y = npc.velocity.Y - acceleration;
			}
			Player player = Main.player[base.npc.target];
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 173);
			float num5 = base.npc.position.X + (float)(base.npc.width / 2) - player.position.X - (float)(player.width / 2);
			float num6 = base.npc.position.Y + (float)base.npc.height - 59f - player.position.Y - (float)(player.height / 2);
			float num7 = (float)Math.Atan2((double)num6, (double)num5) + 1.57f;
			if (num7 < 0f) {
				num7 += 6.283f;
			}
			else if ((double)num7 > 6.283) {
				num7 -= 6.283f;
			}
			float num8 = 0.1f;
			if (base.npc.rotation < num7) {
				if ((double)(num7 - base.npc.rotation) > 3.1415) {
					base.npc.rotation -= num8;
				}
				else {
					base.npc.rotation += num8;
				}
			}
			else if (base.npc.rotation > num7) {
				if ((double)(base.npc.rotation - num7) > 3.1415) {
					base.npc.rotation += num8;
				}
				else {
					base.npc.rotation -= num8;
				}
			}
			if (base.npc.rotation > num7 - num8 && base.npc.rotation < num7 + num8) {
				base.npc.rotation = num7;
			}
			if (base.npc.rotation < 0f) {
				base.npc.rotation += 6.283f;
			}
			else if ((double)base.npc.rotation > 6.283) {
				base.npc.rotation -= 6.283f;
			}
			if (base.npc.rotation > num7 - num8 && base.npc.rotation < num7 + num8) {
				base.npc.rotation = num7;
			}
			base.npc.spriteDirection = base.npc.direction;
			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Shadowflame>(), 150);
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.frameCounter >= 4) {
				npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
				npc.frameCounter = 0;
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}
