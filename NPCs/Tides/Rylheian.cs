
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Weapon.Flail;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
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
			Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
		{
			npc.width = 66;
			npc.height = 88;
			npc.damage = 32;
			npc.defense = 14;
			npc.lifeMax = 1300;
			npc.knockBackResist = 0;
			npc.aiStyle = -1;
			npc.noGravity = true;
            npc.netAlways = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit55;
			npc.DeathSound = SoundID.NPCDeath5;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.RlyehianBanner>();
		}

		float alphaCounter = 0;
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (npc.ai[1] != 0) {
				float sineAdd = (float)Math.Sin(alphaCounter) + 3;
				Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition), null, new Color((int)(16.9f * sineAdd), (int)(8.9f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.33f * (sineAdd + 1), SpriteEffects.None, 0f);
			}
		}

		public override void AI()
		{
			if (npc.ai[1] != 0) {
				alphaCounter += 0.04f;
			}
			else if (alphaCounter > 0) {
				alphaCounter -= 0.08f;
			}
			if (npc.ai[1] == 0 && npc.life < npc.lifeMax / 3 && npc.ai[0] % 2 == 0) {
				npc.ai[0]++;
			}
			npc.TargetClosest();
			Player player = Main.player[npc.target];
			npc.ai[0]++;
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 173);
			npc.spriteDirection = npc.direction;
			if (npc.ai[0] % 400 == 100 && Main.netMode != NetmodeID.MultiplayerClient) {
				int distance = 500;
				bool teleported = false;
				while (!teleported) {
						npc.ai[3] = Main.rand.Next(360);
						double anglex = Math.Sin(npc.ai[3] * (Math.PI / 180));
						double angley = Math.Cos(npc.ai[3] * (Math.PI / 180));
						npc.position.X = player.Center.X + (int)(distance * anglex);
						npc.position.Y = player.Center.Y + (int)(distance * angley);
					if (Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16)].active() || Main.tile[(int)(npc.Center.X / 16), (int)(npc.Center.Y / 16)].active()) {
							npc.alpha = 255;
						}
						else {
							teleported = true;
							npc.alpha = 0;
						}
				}
				npc.netUpdate = true;
			}
			if (npc.ai[0] % 400 == 104) {
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
				DustHelper.DrawDiamond(npc.Center, 173, 12);
				npc.netUpdate = true;
			}
				float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.2f;
			npc.scale = num395 + 0.95f;
			if (!Main.raining) {
				Main.cloudAlpha += .005f;
				if (Main.cloudAlpha >= .5f) {
					Main.cloudAlpha = .5f;
				}
			}
			if (npc.ai[0] % 400 == 200) {
				npc.ai[1] = Main.rand.Next(3) + 1;
				npc.ai[2] = Main.rand.NextFloat(0.785f);
				npc.netUpdate = true;
			}

			#region Phase 1
			if (npc.ai[0] % 400 == 316 && npc.ai[1] == 1) {
				Main.PlaySound(2, npc.Center, 109);
				for (npc.ai[2] = 0; npc.ai[2] < 6.29; npc.ai[2] += 0.785f) {
					Vector2 offset = new Vector2((float)Math.Cos(npc.ai[2]), (float)Math.Sin(npc.ai[2])) * 90f;
					Vector2 direction = player.Center - (npc.Center + offset);
					direction.Normalize();
					direction *= 19;

					if (Main.netMode != NetmodeID.MultiplayerClient) {
						Projectile proj = Projectile.NewProjectileDirect(npc.Center + offset, direction, ModContent.ProjectileType<RyBolt>(), npc.damage / 2, 0, Main.myPlayer);
						proj.netUpdate = true;
					}
				}
				npc.ai[1] = 0;
                npc.netUpdate = true;
			}
			if (npc.ai[1] == 1) {
				if (npc.ai[0] % 12 == 0 && npc.ai[0] % 400 < 300) {
					Main.PlaySound(2, npc.Center, 8);
					Vector2 offset = new Vector2((float)Math.Cos(npc.ai[2]), (float)Math.Sin(npc.ai[2])) * 90f;
					DustHelper.DrawTriangle(npc.Center + offset, 173, 4);
					npc.ai[2] += 0.785f;
                    npc.netUpdate = true;
				}
			}
			#endregion
			#region phase 2
			if (npc.ai[1] == 2) {
				if (npc.ai[0] % 15 == 10 && Main.netMode != NetmodeID.MultiplayerClient) {
					int laser = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 10, ModContent.ProjectileType<RyTentacle>(), npc.damage / 2, 0);
					Main.projectile[laser].netUpdate = true;
					npc.netUpdate = true;
				}
				if (npc.ai[0] % 400 == 390) {
					npc.ai[1] = 0;
                    npc.netUpdate = true;
				}
			}
			#endregion
			#region phase 3
			if (npc.ai[1] == 3) {
				if (npc.ai[0] % 25 == 10) {
					if (Main.netMode != NetmodeID.MultiplayerClient) {
						npc.ai[3] = Main.rand.Next(360);
						double squidAnglex = Math.Sin(npc.ai[3] * (Math.PI / 180));
						double squidAngley = 0 - Math.Abs(Math.Cos(npc.ai[3] * (Math.PI / 180)));
						Vector2 direction = player.Center - new Vector2(player.Center.X + (int)(500 * squidAnglex), player.Center.Y + (int)(500 * squidAngley));
						direction.Normalize();
						direction *= 19;
						int squid = Projectile.NewProjectile(player.Center.X + (int)(500 * squidAnglex), player.Center.Y + (int)(500 * squidAngley), direction.X, direction.Y, ModContent.ProjectileType<TentacleSquid>(), npc.damage / 2, 0);
						Main.projectile[squid].netUpdate = true;
						npc.netUpdate = true;
					}
				}
				if(npc.ai[0] % 25 == 11) {
					DustHelper.DrawTriangle(new Vector2(player.Center.X + (int)(500 * Math.Sin(npc.ai[3] * (Math.PI / 180))), player.Center.Y + (int)(500 * (0 - Math.Abs(Math.Cos(npc.ai[3] * (Math.PI / 180)))))), 173, 3);
				}
				if (npc.ai[0] % 400 == 390) {
					npc.ai[1] = 0;
                    npc.netUpdate = true;
				}
			}
			#endregion
		}
		public override void FindFrame(int frameHeight)
		{
			if (npc.ai[1] == 0) {
				npc.frameCounter += 0.2f;
				npc.frameCounter %= 6;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
			else {
				npc.frameCounter += 0.15f;
				npc.frameCounter %= 3;
				int frame = (int)npc.frameCounter + 6;
				npc.frame.Y = frame * frameHeight;
			}
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}
		public override void NPCLoot()
		{
			string[] lootTable = { "TomeOfRylien", "TentacleChain" };
			{
				int loot = Main.rand.Next(lootTable.Length);
				{
					npc.DropItem(mod.ItemType(lootTable[loot]));
				}
			}
			if (Main.rand.NextBool(10)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RlyehMask>());
			}
			if (Main.rand.NextBool(10)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Trophy10>());
			}
		}
        public override bool PreNPCLoot()
        {
            Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tentacle"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TentacleHead"), 1f);
			}
		}
	}
}
