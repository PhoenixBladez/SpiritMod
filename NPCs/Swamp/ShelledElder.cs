using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Flail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Swamp
{
	public class ShelledElder : ModNPC
	{
		bool attack = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shelled Elder");
			Main.npcFrameCount[npc.type] = 13;
		}

		public override void SetDefaults()
		{
			npc.width = 90;
			npc.height = 100;
			npc.damage = 30;
			npc.defense = 11;
			npc.lifeMax = 700;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 90f;
			npc.knockBackResist = .35f;
		}
		int frame = 0;
		int timer = 0;
		int shootTimer = 0;
		public override void HitEffect(int hitDirection, double damage)
		{
			Main.PlaySound(3, npc.Center, 7);
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 167, hitDirection, -1f, 0, Color.Green, .61f);
			}
			if (npc.life <= 0) {
				Main.PlaySound(SoundID.Zombie, npc.Center, 7);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ThornStalker/ThornStalker1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ThornStalker/ThornStalker2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ThornStalker/ThornStalker3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ThornStalker/ThornStalker4"), 1f);
			}
		}

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.024f, 0.088f, 0.026f);
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			shootTimer++;
			if (shootTimer % 150 == 100) {
				attack = true;
			}
			if (attack) {
				npc.velocity.Y = 6;
				npc.velocity.X = .008f * npc.direction;
				//shootTimer++;
				if (frame == 10 && timer == 0) {
					Main.PlaySound(SoundID.Item, npc.Center, 95);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						for (int i = 0; i < 2; i++) {
							Vector2 knifePos = new Vector2(npc.Center.X + Main.rand.Next(-50, 50), npc.Center.Y - Main.rand.Next(60));
							Vector2 direction = Main.player[npc.target].Center - knifePos;
							direction.Normalize();
							direction *= Main.rand.NextFloat(7, 10);
							int knife = Terraria.Projectile.NewProjectile(knifePos, direction, ModContent.ProjectileType<ThornKnife>(), npc.damage / 4, 0);
						}
					}
					timer++;
				}
				timer++;
				if (timer >= 6) {
					frame++;
					timer = 0;
				}
				if (frame > 12) {
					attack = false;
					frame = 12;
				}
				if (frame < 6) {
					frame = 6;
				}
				if (target.position.X > npc.position.X) {
					npc.direction = 1;
				}
				else {
					npc.direction = -1;
				}
			}
			else {
				//shootTimer = 0;
				npc.aiStyle = 3;
				aiType = NPCID.Zombie;
				timer++;
				if (timer >= 6) {
					frame++;
					timer = 0;
				}
				if (frame > 5) {
					frame = 1;
				}
			}
			if (!attack && !npc.collideY && npc.velocity.Y > 0) {
				frame = 0;
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
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(3) == 1) {
				int Bark = Main.rand.Next(2) + 1;
				for (int J = 0; J <= Bark; J++) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<AncientBark>());
				}
			}
			if (!Main.dayTime) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<EnchantedLeaf>());
			}
			if (Main.rand.Next(33) == 3) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<VineChain>());
			}
		}
	}
}
