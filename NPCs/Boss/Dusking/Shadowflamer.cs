using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Boss.Dusking
{
	public class Shadowflamer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antumbral Skull");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = NPC.height = 30;
			NPC.lifeMax = 70;
			NPC.damage = 45;
			NPC.knockBackResist = 0;
			NPC.DeathSound = SoundID.NPCDeath6;

			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[BuffID.ShadowFlame] = true;

			NPC.friendly = false;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("End of dawns, bringer of nights, may the king of darkness rise to fight! May you cast the world to darkness and forever seal the light! O’ king hear my cry, for may victory be yours tonight!"),
			});
		}

		public override bool PreAI()
		{
			NPC.TargetClosest(true);
			float speed = 8f;
			float acceleration = 0.13f;
			Vector2 vector2 = new Vector2(NPC.position.X + NPC.width * 0.5F, NPC.position.Y + NPC.height * 0.5F);
			float xDir = Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5F) - vector2.X;
			float yDir = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5F) - vector2.Y;
			float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);

			float num10 = speed / length;
			xDir *= num10;
			yDir *= num10;
			if (NPC.velocity.X < xDir) {
				NPC.velocity.X = NPC.velocity.X + acceleration;
				if (NPC.velocity.X < 0 && xDir > 0)
					NPC.velocity.X = NPC.velocity.X + acceleration;
			}
			else if (NPC.velocity.X > xDir) {
				NPC.velocity.X = NPC.velocity.X - acceleration;
				if (NPC.velocity.X > 0 && xDir < 0)
					NPC.velocity.X = NPC.velocity.X - acceleration;
			}

			if (NPC.velocity.Y < yDir) {
				NPC.velocity.Y = NPC.velocity.Y + acceleration;
				if (NPC.velocity.Y < 0 && yDir > 0)
					NPC.velocity.Y = NPC.velocity.Y + acceleration;
			}
			else if (NPC.velocity.Y > yDir) {
				NPC.velocity.Y = NPC.velocity.Y - acceleration;
				if (NPC.velocity.Y > 0 && yDir < 0)
					NPC.velocity.Y = NPC.velocity.Y - acceleration;
			}
			Player player = Main.player[NPC.target];
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff);
			float num5 = NPC.position.X + (NPC.width / 2) - player.position.X - (player.width / 2);
			float num6 = NPC.position.Y + NPC.height - 59f - player.position.Y - (player.height / 2);
			float num7 = (float)Math.Atan2(num6, num5) + 1.57f;
			if (num7 < 0f)
				num7 += 6.283f;
			else if ((double)num7 > 6.283)
				num7 -= 6.283f;
			float num8 = 0.1f;
			if (NPC.rotation < num7) {
				if ((double)(num7 - NPC.rotation) > 3.1415) {
					NPC.rotation -= num8;
				}
				else {
					NPC.rotation += num8;
				}
			}
			else if (NPC.rotation > num7) {
				if ((NPC.rotation - num7) > 3.1415) {
					NPC.rotation += num8;
				}
				else {
					NPC.rotation -= num8;
				}
			}
			if (NPC.rotation > num7 - num8 && NPC.rotation < num7 + num8) {
				NPC.rotation = num7;
			}
			if (NPC.rotation < 0f) {
				NPC.rotation += 6.283f;
			}
			else if (NPC.rotation > 6.283) {
				NPC.rotation -= 6.283f;
			}
			if (NPC.rotation > num7 - num8 && NPC.rotation < num7 + num8) {
				NPC.rotation = num7;
			}
			NPC.spriteDirection = NPC.direction;
			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(ModContent.BuffType<Shadowflame>(), 150);
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= 4) {
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[NPC.type] * frameHeight);
				NPC.frameCounter = 0;
			}
		}
	}
}
