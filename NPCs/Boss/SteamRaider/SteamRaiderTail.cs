using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class SteamRaiderTail : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Voyager");
		}

		public override void SetDefaults()
		{
			NPC.damage = 25;
			NPC.npcSlots = 10f;
			NPC.width = 38; 
			NPC.height = 16; 
			NPC.defense = 15;
			NPC.lifeMax = 6500; //250000
			NPC.aiStyle = 6; //new
			Main.npcFrameCount[NPC.type] = 1; //new
			AIType = -1; //new
			AnimationType = 10; //new
			NPC.knockBackResist = 0f;
			NPC.alpha = 255;
			NPC.behindTiles = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.netAlways = true;
			for (int k = 0; k < NPC.buffImmune.Length; k++) {
				NPC.buffImmune[k] = true;
			}
			Music = MusicID.Boss3;
			NPC.dontCountMe = true;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			bool expertMode = Main.expertMode;
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0f, 0.075f, 0.25f);
			int parent = NPC.FindFirstNPC(ModContent.NPCType<SteamRaiderHead>());
			if (parent < 0 || parent >= Main.npc.Length) {
				NPC.active = false;
				return;
			}
			if (!Main.npc[(int)NPC.ai[1]].active) {
				NPC.life = 0;
				NPC.HitEffect(0, 10.0);
				NPC.active = false;
			}
			if (Main.npc[parent].ai[2] != 1) {
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					NPC.localAI[0] += expertMode ? 2f : 1f;
					if (NPC.localAI[0] >= 200f) {
						NPC.localAI[0] = 0f;
						NPC.TargetClosest(true);
						if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height)) {

							SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 12);
							float num941 = 4f; //speed
							Vector2 vector104 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)(NPC.height / 2));
							float num942 = player.position.X + (float)player.width * 0.5f - vector104.X + (float)Main.rand.Next(-20, 21);
							float num943 = player.position.Y + (float)player.height * 0.5f - vector104.Y + (float)Main.rand.Next(-20, 21);
							float num944 = (float)Math.Sqrt((double)(num942 * num942 + num943 * num943));
							num944 = num941 / num944;
							num942 *= num944;
							num943 *= num944;
							//int num946 = 440;
							vector104.X += num942 * 5f;
							vector104.Y += num943 * 5f;
							int num947 = Projectile.NewProjectile(vector104.X, vector104.Y, num942, num943, ModContent.ProjectileType<GlitchLaser>(), NPCUtils.ToActualDamage(25, 1.5f), 0f, Main.myPlayer, 0f, 0f);
							Main.projectile[num947].timeLeft = 300;
							NPC.netUpdate = true;
						}
					}
				}
			}
			else {
				NPC.localAI[0] = 0;
			}
			if ((Main.npc[parent].life <= Main.npc[parent].lifeMax * 0.75f)) {
				SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 14);
				NPC.life = 0;
				NPC.HitEffect(0, 10.0);
				NPC.active = false;
				NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TailProbe>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
				SoundEngine.PlaySound(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 0);
				SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 4);
				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++) {
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.DungeonSpirit, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
			if (Main.npc[(int)NPC.ai[1]].alpha < 128) {
				if (NPC.alpha != 0) {
					for (int num934 = 0; num934 < 2; num934++) {
						int num935 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, 2f);
						Main.dust[num935].noGravity = true;
						Main.dust[num935].noLight = true;
					}
				}
				NPC.alpha -= 42;
				if (NPC.alpha < 0)
					NPC.alpha = 0;
			}
			if (NPC.ai[2] > 0) {
				NPC.realLife = (int)NPC.ai[2];
			}

			if (NPC.target < 0 || NPC.target == byte.MaxValue || Main.player[NPC.target].dead) {
				NPC.TargetClosest(true);
			}

			if (NPC.ai[1] < (double)Main.npc.Length) {
				// We're getting the center of this NPC.
				Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				// Then using that center, we calculate the direction towards the 'parent NPC' of this NPC.
				float dirX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - npcCenter.X;
				float dirY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - npcCenter.Y;
				// We then use Atan2 to get a correct rotation towards that parent NPC.
				NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
				// We also get the length of the direction vector.
				float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				// We calculate a new, correct distance.
				float dist = (length - (float)NPC.width) / length;
				float posX = dirX * dist;
				float posY = dirY * dist;

				// Reset the velocity of this NPC, because we don't want it to move on its own
				NPC.velocity = Vector2.Zero;
				// And set this NPCs position accordingly to that of this NPCs parent NPC.
				NPC.position.X = NPC.position.X + posX;
				NPC.position.Y = NPC.position.Y + posY;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, hitDirection, -1f, 0, default, 1f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{

			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.GetTexture("NPCs/Boss/SteamRaider/SteamRaiderTail_Glow"));

		}
		public override bool CheckActive()
		{
			return false;
		}

		public override bool PreKill()
		{
			return false;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.6f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.65f);
		}
	}
}