
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Accessory;
using System;
using SpiritMod.Items.Armor.AstronautVanity;

namespace SpiritMod.NPCs.Gloop
{
	public class GloopGloop : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gloop");
			Main.npcFrameCount[NPC.type] = 3;
		}

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 48;
			NPC.damage = 28;
			NPC.defense = 8;
			NPC.lifeMax = 85;
			NPC.noGravity = true;
			NPC.value = 90f;
			NPC.noTileCollide = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.HitSound = SoundID.DD2_GoblinHurt;
			NPC.DeathSound = SoundID.NPCDeath22;
			NPC.noGravity = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.GloopBanner>();
        }
		int xoffset = 0;
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC.ai[0]++;
			if(player.position.X > NPC.position.X) {
				xoffset = 16;
			} else {
				xoffset = -16;
			}
			NPC.velocity.X *= 0.99f;
				if(NPC.ai[1] == 0) {
					if(NPC.velocity.Y < 2.5f) {
						NPC.velocity.Y += 0.1f;
					}
					if(player.position.Y < NPC.position.Y && NPC.ai[0] % 30 == 0) {
						NPC.ai[1] = 1;
						NPC.netUpdate = true;
						NPC.velocity.X = xoffset / 1.25f;
						NPC.velocity.Y = -6;
					}
				}
				if(NPC.ai[1] == 1) {
					NPC.velocity *= 0.97f;
					if(Math.Abs(NPC.velocity.X) < 0.125f) {
						NPC.ai[1] = 0;
						NPC.netUpdate = true;
					}
					NPC.rotation = NPC.velocity.ToRotation() + 1.57f;
				}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<GravityModulator>(400);
			npcLoot.AddOneFromOptions(ModContent.ItemType<AstronautLegs>(), ModContent.ItemType<AstronautHelm>(), ModContent.ItemType<AstronautBody>());
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Plantera_Green, hitDirection, -1f, 0, default, .61f);
			}
			if (NPC.life <= 0) {
				for (int k = 0; k < 20; k++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Plantera_Green, hitDirection, -1f, 0, default, .91f);
				}
			}
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 0) {
				target.AddBuff(BuffID.Poisoned, 180);
			}
		}
	}
}
