using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.NPCs.Dungeon;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Tide;

namespace SpiritMod.NPCs.Tides
{
	public class Crocomount : ModNPC
	{
		bool attack = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crocosaur");
			Main.npcFrameCount[npc.type] = 11;
		}

		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 70;
			npc.damage = 32;
			npc.defense = 14;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit6;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 500f;
			npc.knockBackResist = .1f;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.CrocosaurBanner>();
		}


		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 5;
			int d1 = 5;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.Green, 0.87f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, Color.Green, .54f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crocomount/CrocomountGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crocomount/CrocomountGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crocomount/CrocomountGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crocomount/CrocomountGore4"), 1f);
				if (TideWorld.TheTide && TideWorld.TidePoints < 99) {
					TideWorld.TidePoints += 1;
				}
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.NextBool(78)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CrocodrilloMountItem>());
			}
		}

		int frame = 0;
		int timer = 0;
		//  int shootTimer = 0;
		public override void AI()
		{
			if (npc.wet) {
				npc.noGravity = true;
				if (npc.velocity.Y > -7) {
					npc.velocity.Y -= .085f;
				}
				return;
			}
			else {
				npc.noGravity = false;
			}
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 50) {
				attack = true;
			}
			if (distance > 80) {
				attack = false;
			}
			if (attack) {
				npc.velocity.X = .008f * npc.direction;
				//shootTimer++;
				timer++;
				if (timer >= 5) {
					frame++;
					timer = 0;
				}
				if (frame > 10) {
					frame = 7;
				}
				if (frame < 7) {
					frame = 7;
				}
				if (target.position.X > npc.position.X) {
					npc.direction = 1;
				}
				else {
					npc.direction = -1;
				}
			}
			else {
				{
					//shootTimer = 0;
					npc.aiStyle = 26;
					aiType = NPCID.Unicorn;
				}
				timer++;
				if (timer >= 4) {
					frame++;
					timer = 0;
				}
				if (frame > 6) {
					frame = 0;
				}
			}
			/*if (shootTimer > 120)
            {
                shootTimer = 120;
            }
            if (shootTimer < 0)
            {
                shootTimer = 0;
            }*/
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (attack) {
				target.AddBuff(BuffID.Bleeding, 600);
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
	}
}
