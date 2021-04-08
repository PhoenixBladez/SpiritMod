
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Accessory;
using System;

namespace SpiritMod.NPCs.Gloop
{
	public class GloopGloop : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gloop");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 48;
			npc.damage = 28;
			npc.defense = 8;
			npc.lifeMax = 85;
			npc.noGravity = true;
			npc.value = 90f;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.DD2_GoblinHurt;
			npc.DeathSound = SoundID.NPCDeath22;
			npc.noGravity = true;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.GloopBanner>();
        }
		int xoffset = 0;
		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.ai[0]++;
			if(player.position.X > npc.position.X) {
				xoffset = 16;
			} else {
				xoffset = -16;
			}
			npc.velocity.X *= 0.99f;
				if(npc.ai[1] == 0) {
					if(npc.velocity.Y < 2.5f) {
						npc.velocity.Y += 0.1f;
					}
					if(player.position.Y < npc.position.Y && npc.ai[0] % 30 == 0) {
						npc.ai[1] = 1;
						npc.netUpdate = true;
						npc.velocity.X = xoffset / 1.25f;
						npc.velocity.Y = -6;
					}
				}
				if(npc.ai[1] == 1) {
					npc.velocity *= 0.97f;
					if(Math.Abs(npc.velocity.X) < 0.125f) {
						npc.ai[1] = 0;
						npc.netUpdate = true;
					}
					npc.rotation = npc.velocity.ToRotation() + 1.57f;
				}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(1) == 400) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GravityModulator>());
			}
			string[] lootTable = { "AstronautLegs", "AstronautHelm", "AstronautBody" };
			if (Main.rand.Next(50) == 0) {
				int loot = Main.rand.Next(lootTable.Length);
				{
					npc.DropItem(mod.ItemType(lootTable[loot]));
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 167, hitDirection, -1f, 0, default(Color), .61f);
			}
			if (npc.life <= 0) {
				for (int k = 0; k < 20; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 167, hitDirection, -1f, 0, default(Color), .91f);
				}
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 0) {
				target.AddBuff(BuffID.Poisoned, 180);
			}
		}
	}
}
