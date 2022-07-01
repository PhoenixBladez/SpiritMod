
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Items.Sets.RlyehianDrops;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.Tides.Tide;
using System.IO;

namespace SpiritMod.NPCs.Tides
{
	[AutoloadBossHead]
	public class Rylheian : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'lyehian");
			Main.npcFrameCount[NPC.type] = 9;
		}

		public override void SetDefaults()
		{
			NPC.width = 66;
			NPC.height = 88;
			NPC.damage = 32;
			NPC.defense = 14;
			NPC.lifeMax = 1300;
			NPC.knockBackResist = 0;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
            NPC.netAlways = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit55;
			NPC.DeathSound = SoundID.NPCDeath5;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.RlyehianBanner>();
		}

		float alphaCounter = 0;
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.ai[1] != 0) {
				float sineAdd = (float)Math.Sin(alphaCounter) + 3;
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Extra[49].Value, (NPC.Center - Main.screenPosition), null, new Color((int)(16.9f * sineAdd), (int)(8.9f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.33f * (sineAdd + 1), SpriteEffects.None, 0f);
			}
		}

		public override void AI()
		{
			if (NPC.ai[1] != 0) {
				alphaCounter += 0.04f;
			}
			else if (alphaCounter > 0) {
				alphaCounter -= 0.08f;
			}
			if (NPC.ai[1] == 0 && NPC.life < NPC.lifeMax / 3 && NPC.ai[0] % 2 == 0) {
				NPC.ai[0]++;
			}
			NPC.TargetClosest();
			Player player = Main.player[NPC.target];
			NPC.ai[0]++;
			int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff);
			NPC.spriteDirection = NPC.direction;
			if (NPC.ai[0] % 400 == 100 && Main.netMode != NetmodeID.MultiplayerClient) {
				int distance = 500;
				bool teleported = false;
				while (!teleported) 
				{
					if (Main.netMode != NetmodeID.Server)
					{
						if (Main.rand.NextBool(2))
							SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/RlyehianCry2").WithVolume(0.85f).WithPitchVariance(0.4f), NPC.Center);
						else
							SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/RlyehianCry").WithVolume(0.85f).WithPitchVariance(0.4f), NPC.Center);
					}
					NPC.ai[3] = Main.rand.Next(360);
						double anglex = Math.Sin(NPC.ai[3] * (Math.PI / 180));
						double angley = Math.Cos(NPC.ai[3] * (Math.PI / 180));
						NPC.position.X = player.Center.X + (int)(distance * anglex);
						NPC.position.Y = player.Center.Y + (int)(distance * angley);
					if (Main.tile[(int)(NPC.position.X / 16), (int)(NPC.position.Y / 16)].HasTile || Main.tile[(int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16)].HasTile) {
							NPC.alpha = 255;
						}
						else {
							teleported = true;
							NPC.alpha = 0;
						}
				}
				NPC.netUpdate = true;
			}
			if (NPC.ai[0] % 400 == 104) {
				SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
				DustHelper.DrawDiamond(NPC.Center, 173, 12);
				NPC.netUpdate = true;
			}
				float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.2f;
			NPC.scale = num395 + 0.95f;
			if (!Main.raining) {
				Main.cloudAlpha += .005f;
				if (Main.cloudAlpha >= .5f) {
					Main.cloudAlpha = .5f;
				}
			}
			if (NPC.ai[0] % 400 == 200) {
				NPC.ai[1] = Main.rand.Next(3) + 1;
				NPC.ai[2] = Main.rand.NextFloat(0.785f);
				NPC.netUpdate = true;
			}

			#region Phase 1
			if (NPC.ai[0] % 400 == 316 && NPC.ai[1] == 1) {
				SoundEngine.PlaySound(SoundID.Item109, NPC.Center);
				for (NPC.ai[2] = 0; NPC.ai[2] < 6.29; NPC.ai[2] += 0.785f) {
					Vector2 offset = new Vector2((float)Math.Cos(NPC.ai[2]), (float)Math.Sin(NPC.ai[2])) * 90f;
					Vector2 direction = player.Center - (NPC.Center + offset);
					direction.Normalize();
					direction *= 19;

					if (Main.netMode != NetmodeID.MultiplayerClient) {
						Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + offset, direction, ModContent.ProjectileType<RyBolt>(), NPC.damage / 2, 0, Main.myPlayer);
						proj.netUpdate = true;
					}
				}
				NPC.ai[1] = 0;
                NPC.netUpdate = true;
			}
			if (NPC.ai[1] == 1) {
				if (NPC.ai[0] % 12 == 0 && NPC.ai[0] % 400 < 300) {
					SoundEngine.PlaySound(SoundID.Item, NPC.Center, 8);
					Vector2 offset = new Vector2((float)Math.Cos(NPC.ai[2]), (float)Math.Sin(NPC.ai[2])) * 90f;
					DustHelper.DrawTriangle(NPC.Center + offset, 173, 4);
					NPC.ai[2] += 0.785f;
                    NPC.netUpdate = true;
				}
			}
			#endregion
			#region phase 2
			if (NPC.ai[1] == 2) {
				if (NPC.ai[0] % 15 == 10 && Main.netMode != NetmodeID.MultiplayerClient) {
					int laser = Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X, player.Center.Y, 0, 10, ModContent.ProjectileType<RyTentacle>(), NPC.damage / 2, 0);
					Main.projectile[laser].netUpdate = true;
					NPC.netUpdate = true;
				}
				if (NPC.ai[0] % 400 == 390) {
					NPC.ai[1] = 0;
                    NPC.netUpdate = true;
				}
			}
			#endregion
			#region phase 3
			if (NPC.ai[1] == 3) {
				if (NPC.ai[0] % 25 == 10) {
					if (Main.netMode != NetmodeID.MultiplayerClient) {
						NPC.ai[3] = Main.rand.Next(360);
						SoundEngine.PlaySound(SoundID.Item, NPC.Center, 8);
						double squidAnglex = Math.Sin(NPC.ai[3] * (Math.PI / 180));
						double squidAngley = 0 - Math.Abs(Math.Cos(NPC.ai[3] * (Math.PI / 180)));
						Vector2 direction = player.Center - new Vector2(player.Center.X + (int)(500 * squidAnglex), player.Center.Y + (int)(500 * squidAngley));
						direction.Normalize();
						direction *= 19;
						int squid = Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + (int)(500 * squidAnglex), player.Center.Y + (int)(500 * squidAngley), direction.X, direction.Y, ModContent.ProjectileType<TentacleSquid>(), NPC.damage / 2, 0);
						Main.projectile[squid].netUpdate = true;
						NPC.netUpdate = true;
					}
				}
				if(NPC.ai[0] % 25 == 11) {
					DustHelper.DrawTriangle(new Vector2(player.Center.X + (int)(500 * Math.Sin(NPC.ai[3] * (Math.PI / 180))), player.Center.Y + (int)(500 * (0 - Math.Abs(Math.Cos(NPC.ai[3] * (Math.PI / 180)))))), 173, 3);
				}
				if (NPC.ai[0] % 400 == 390) {
					NPC.ai[1] = 0;
                    NPC.netUpdate = true;
				}
			}
			#endregion
		}
		public override void FindFrame(int frameHeight)
		{
			if (NPC.ai[1] == 0) {
				NPC.frameCounter += 0.2f;
				NPC.frameCounter %= 6;
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = frame * frameHeight;
			}
			else {
				NPC.frameCounter += 0.15f;
				NPC.frameCounter %= 3;
				int frame = (int)NPC.frameCounter + 6;
				NPC.frame.Y = frame * frameHeight;
			}
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<RlyehMask>(10);
			npcLoot.AddCommon<Trophy10>(10);
			npcLoot.AddOneFromOptions(1, ModContent.ItemType<TomeOfRylien>(), ModContent.ItemType<TentacleChain>());
		}

		public override bool PreKill()
        {
            SoundEngine.PlaySound(SoundLoader.customSoundType, NPC.position, Mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Tentacle").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/TentacleHead").Type, 1f);
			}
		}
	}
}
