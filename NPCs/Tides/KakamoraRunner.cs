using SpiritMod.Projectiles.Hostile;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Tides.Tide;
using SpiritMod.Items.Sets.TideDrops;

namespace SpiritMod.NPCs.Tides
{
	public class KakamoraRunner : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamora");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 42;
			npc.height = 38;
			npc.damage = 24;
			npc.defense = 12;
			aiType = NPCID.SnowFlinx;
			npc.aiStyle = 3;
			npc.lifeMax = 120;
			npc.knockBackResist = .25f;
			npc.value = 200f;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath1;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.KakamoraBanner>();
		}

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
			var list = Main.npc.Where(x => x.Hitbox.Intersects(npc.Hitbox));
			foreach (var npc2 in list) {
				if (npc2.type == ModContent.NPCType<LargeCrustecean>() && npc.Center.Y > npc2.Center.Y && npc2.active) {
					npc.velocity.X = npc2.direction * 7;
					npc.velocity.Y = -2;
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
				}
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				var list2 = Main.projectile.Where(x => x.Hitbox.Intersects(npc.Hitbox));
				foreach (var proj in list2) {
					if (proj.type == ModContent.ProjectileType<ShamanBolt>() && proj.active && npc.life < npc.lifeMax - 30) {
						npc.life += 30;
						npc.HealEffect(30, true);
						proj.active = false;
					}
					else if (proj.type == ModContent.ProjectileType<ShamanBolt>() && proj.active && npc.life > npc.lifeMax - 30) {
						npc.life += npc.lifeMax - npc.life;
						npc.HealEffect(npc.lifeMax - npc.life, true);
						proj.active = false;
					}
				}
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.NextBool(50)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CoconutGun>());
			}
			if (Main.rand.NextBool(50)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<TikiJavelin>());
			}
		}
		public override void FindFrame(int frameHeight)
		{
			if (npc.collideY || npc.wet) {
				npc.frameCounter += 0.2f;
				npc.frameCounter %= Main.npcFrameCount[npc.type];
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 207;
			int d1 = 207;
			for (int k = 0; k < 10; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}
			if (npc.life <= 0) {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore3"), 1f);
			}
		}
	}
}
