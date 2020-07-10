
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
using SpiritMod.Tide;

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
			npc.damage = 42;
			npc.defense = 14;
			npc.lifeMax = 2800;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit55;
			npc.DeathSound = SoundID.NPCDeath5;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.RlyehianBanner>();
        }
		int counter;
		int phase = 0; //0 = charging, 1 = special attack
		float angle = 0;
		float alphaCounter = 0;
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (phase != 0)
			{
				float sineAdd = (float)Math.Sin(alphaCounter) + 3;
				Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition), null, new Color((int)(16.9f * sineAdd), (int)(8.9f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.33f * (sineAdd + 1), SpriteEffects.None, 0f);
			}
		}
		public override void AI()
		{
			if (phase != 0)
			{
				alphaCounter+= 0.04f;
			}
			else if (alphaCounter > 0)
			{
				alphaCounter -= 0.08f;
			}
			if (phase == 0 && npc.life < npc.lifeMax / 3 && counter % 2 == 0)
			{
				counter++;
			}
			npc.TargetClosest();
			Player player = Main.player[npc.target];
            counter++;
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 173);
			npc.spriteDirection = npc.direction;
			if (counter % 400 == 100)
			{
				int distance = 500;
				bool teleported = false;
				while (!teleported)
				{
					int angleT = Main.rand.Next(360);
					double anglex = Math.Sin(angleT * (Math.PI / 180));
					double angley = Math.Cos(angleT * (Math.PI / 180));
					npc.position.X = player.Center.X + (int)(distance * anglex);
					npc.position.Y = player.Center.Y + (int)(distance * angley);
					if(Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16)].active()) {
						npc.alpha = 255;
					} else {
						teleported = true;
						npc.alpha = 0;
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
						DustHelper.DrawDiamond(npc.Center, 173, 12);
					}
				}
			}
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            npc.scale = num395 + 0.95f;
            if (!Main.raining)
            {
                Main.cloudAlpha += .005f;
                if (Main.cloudAlpha >= .5f)
                {
                    Main.cloudAlpha = .5f;
                }
            }
			if (counter % 400 == 200)
			{
				phase = Main.rand.Next(3) + 1;
				angle = Main.rand.NextFloat(0.785f);
			}

			#region phase 1
			if (counter % 400 == 316 && phase == 1)
            {
                Main.PlaySound(2, npc.Center, 109);
                for (angle = 0; angle < 6.29; angle+= 0.785f)
				{
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 90f;
					int laser = Terraria.Projectile.NewProjectile(npc.Center.X+ offset.X, npc.Center.Y + offset.Y, 0, 0, ModContent.ProjectileType<RyBolt>(), npc.damage/2, 0);
					Projectile p = Main.projectile[laser];
					Vector2 direction = Main.player[npc.target].Center - p.Center;
					direction.Normalize();
					direction *= 19;
					p.velocity = direction;
				}
				phase = 0;
			}
			if (phase == 1)
			{
				if (counter % 12 == 0 && counter % 400 < 300)
				{
                    Main.PlaySound(2, npc.Center, 8);
					Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 90f;
					DustHelper.DrawTriangle(npc.Center + offset, 173, 4);
					angle += 0.785f;
				}
			}
			#endregion
			#region phase 2
			if (phase == 2)
			{
				if (counter % 15 == 10)
				{
					int laser = Terraria.Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 10, ModContent.ProjectileType<RyTentacle>(), npc.damage/2, 0);
				}
				if (counter % 400 == 390)
				{
					phase = 0;
				}
			}
			#endregion
			#region phase 3
			if (phase == 3)
			{
				if (counter % 25 == 10)
				{
					int angleS = Main.rand.Next(360);
					double squidAnglex = Math.Sin(angleS * (Math.PI / 180));
					double squidAngley = 0 - Math.Abs(Math.Cos(angleS * (Math.PI / 180)));
					int squid = Projectile.NewProjectile(player.Center.X + (int)(500 * squidAnglex), player.Center.Y + (int)(500 * squidAngley), 0, 0, ModContent.ProjectileType<TentacleSquid>(), npc.damage/2, 0);
                    Projectile p = Main.projectile[squid];
					Vector2 direction = Main.player[npc.target].Center - p.Center;
                    Main.PlaySound(4, p.Center, 19);
                    direction.Normalize();
					direction *= 9f;
					p.velocity = direction;
					DustHelper.DrawTriangle(new Vector2(player.Center.X + (int)(500 * squidAnglex), player.Center.Y + (int)(500 * squidAngley)), 173, 3);
				}
				if (counter % 400 == 390)
				{
					phase = 0;
				}
			}
			#endregion
		}
		public override void FindFrame(int frameHeight)
		{
			if (phase == 0)
			{
				npc.frameCounter += 0.2f;
				npc.frameCounter %= 6;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
			else
			{
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
            string[] lootTable = { "TomeOfRylien", "TentacleChain"};
            {
                int loot = Main.rand.Next(lootTable.Length);
                {
                    npc.DropItem(mod.ItemType(lootTable[loot]));
                }
            }
			if (Main.rand.NextBool(10))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RlyehMask>());
            }
            if (Main.rand.NextBool(10))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Trophy10>());
            }
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			for(int i = 0; i < 10; i++)
				;
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tentacle"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TentacleHead"), 1f);
				if(TideWorld.TheTide) {
					TideWorld.TidePoints += 10;
				}
			}
		}
	}
}
