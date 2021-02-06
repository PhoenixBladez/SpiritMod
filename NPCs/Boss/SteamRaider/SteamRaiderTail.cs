using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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
			npc.damage = 25;
			npc.npcSlots = 10f;
			npc.width = 38; 
			npc.height = 16; 
			npc.defense = 15;
			npc.lifeMax = 6500; //250000
			npc.aiStyle = 6; //new
			Main.npcFrameCount[npc.type] = 1; //new
			aiType = -1; //new
			animationType = 10; //new
			npc.knockBackResist = 0f;
			npc.alpha = 255;
			npc.behindTiles = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.netAlways = true;
			for (int k = 0; k < npc.buffImmune.Length; k++) {
				npc.buffImmune[k] = true;
			}
			music = MusicID.Boss3;
			npc.dontCountMe = true;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

		public override void AI()
		{
			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0f, 0.075f, 0.25f);
			int parent = NPC.FindFirstNPC(ModContent.NPCType<SteamRaiderHead>());
			if (!Main.npc[(int)npc.ai[1]].active) {
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				npc.active = false;
			}
			if (Main.npc[parent].ai[2] != 1) {
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					npc.localAI[0] += expertMode ? 2f : 1f;
					if (npc.localAI[0] >= 200f) {
						npc.localAI[0] = 0f;
						npc.TargetClosest(true);
						if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)) {

							Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 12);
							float num941 = 4f; //speed
							Vector2 vector104 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)(npc.height / 2));
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
							npc.netUpdate = true;
						}
					}
				}
			}
			else {
				npc.localAI[0] = 0;
			}
			if ((Main.npc[parent].life <= 6500)) {
				Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 14);
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				npc.active = false;
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<TailProbe>(), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
				Main.PlaySound(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 0);
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 4);
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 226, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++) {
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 226, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 180, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
			if (Main.npc[(int)npc.ai[1]].alpha < 128) {
				if (npc.alpha != 0) {
					for (int num934 = 0; num934 < 2; num934++) {
						int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 226, 0f, 0f, 100, default(Color), 2f);
						Main.dust[num935].noGravity = true;
						Main.dust[num935].noLight = true;
					}
				}
				npc.alpha -= 42;
				if (npc.alpha < 0)
					npc.alpha = 0;
			}
			if (npc.ai[2] > 0) {
				npc.realLife = (int)npc.ai[2];
			}

			if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead) {
				npc.TargetClosest(true);
			}

			if (npc.ai[1] < (double)Main.npc.Length) {
				// We're getting the center of this NPC.
				Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				// Then using that center, we calculate the direction towards the 'parent NPC' of this NPC.
				float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
				float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
				// We then use Atan2 to get a correct rotation towards that parent NPC.
				npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
				// We also get the length of the direction vector.
				float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				// We calculate a new, correct distance.
				float dist = (length - (float)npc.width) / length;
				float posX = dirX * dist;
				float posY = dirY * dist;

				// Reset the velocity of this NPC, because we don't want it to move on its own
				npc.velocity = Vector2.Zero;
				// And set this NPCs position accordingly to that of this NPCs parent NPC.
				npc.position.X = npc.position.X + posX;
				npc.position.Y = npc.position.Y + posY;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 226, hitDirection, -1f, 0, default(Color), 1f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{

			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/SteamRaider/SteamRaiderTail_Glow"));

		}
		public override bool CheckActive()
		{
			return false;
		}

		public override bool PreNPCLoot()
		{
			return false;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.65f);
		}
	}
}