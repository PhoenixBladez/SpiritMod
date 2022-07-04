using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.FleshHound
{
	public class FleshHound : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flesh Hound");
			Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 60;
			NPC.height = 36;
			NPC.damage = 28;
			NPC.defense = 7;
			NPC.lifeMax = 85;
			NPC.HitSound = SoundID.NPCHit6;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.value = 180f;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			NPC.knockBackResist = .2f;
			NPC.aiStyle = 3;
			AIType = NPCID.WalkingAntlion;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.SpawnTileY < Main.rockLayer && (Main.bloodMoon) && NPC.downedBoss1 ? 0.12f : 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 40; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection * 2.5f, -1f, 0, default, Main.rand.NextFloat(.45f, 1.15f));

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Hound1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Hound2").Type, 1f);

				for (int k = 0; k < 40; k++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection * 2.5f, -1f, 0, default, Main.rand.NextFloat(.45f, 1.15f));
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

			if (trailbehind)
			{
				Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
				for (int k = 0; k < NPC.oldPos.Length; k++)
				{
					Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
					Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += num34616;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		int timer;
		bool trailbehind = false;
		float num34616;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			timer++;

			if (timer == 400 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				SoundEngine.PlaySound(SoundID.Zombie7, NPC.Center);
				NPC.netUpdate = true;
			}

			if (timer == 400 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				num34616 = .55f;
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				direction.X = direction.X * Main.rand.Next(7, 9);
				direction.Y = direction.Y * Main.rand.Next(2, 4);
				NPC.velocity.X = direction.X;
				NPC.velocity.Y = direction.Y;
				NPC.velocity.Y *= 0.98f;
				NPC.velocity.X *= 0.995f;
				NPC.netUpdate = true;
				trailbehind = true;
				NPC.knockBackResist = 0f;
			}
			else
				num34616 = .25f;

			if (timer >= 551)
			{
				timer = 0;
				NPC.netUpdate = true;
				trailbehind = false;
				NPC.knockBackResist = .2f;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(ModContent.BuffType<BloodCorrupt>(), 180);
	}
}