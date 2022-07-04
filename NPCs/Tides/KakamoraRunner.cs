using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.TideDrops;

namespace SpiritMod.NPCs.Tides
{
	public class KakamoraRunner : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamora");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 42;
			NPC.height = 38;
			NPC.damage = 24;
			NPC.defense = 12;
			AIType = NPCID.SnowFlinx;
			NPC.aiStyle = 3;
			NPC.lifeMax = 120;
			NPC.knockBackResist = .25f;
			NPC.value = 200f;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.KakamoraBanner>();
		}

		public override void AI()
		{
			if (NPC.wet)
			{
				NPC.noGravity = true;
				if (NPC.velocity.Y > -7)
					NPC.velocity.Y -= .085f;
				return;
			}
			else
				NPC.noGravity = false;

			NPC.spriteDirection = NPC.direction;
			var list = Main.npc.Where(x => x.Hitbox.Intersects(NPC.Hitbox));
			foreach (var npc2 in list)
			{
				if (npc2.type == ModContent.NPCType<LargeCrustecean>() && NPC.Center.Y > npc2.Center.Y && npc2.active)
				{
					NPC.velocity.X = npc2.direction * 7;
					NPC.velocity.Y = -2;
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Kakamora/KakamoraHit"), NPC.Center);
				}
				NPC.TargetClosest(true);
				Player player = Main.player[NPC.target];
				var list2 = Main.projectile.Where(x => x.Hitbox.Intersects(NPC.Hitbox));
				foreach (var proj in list2)
				{
					if (proj.type == ModContent.ProjectileType<ShamanBolt>() && proj.active && NPC.life < NPC.lifeMax - 30)
					{
						NPC.life += 30;
						NPC.HealEffect(30, true);
						proj.active = false;
					}
					else if (proj.type == ModContent.ProjectileType<ShamanBolt>() && proj.active && NPC.life > NPC.lifeMax - 30)
					{
						NPC.life += NPC.lifeMax - NPC.life;
						NPC.HealEffect(NPC.lifeMax - NPC.life, true);
						proj.active = false;
					}
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<CoconutGun>(50);
			npcLoot.AddCommon<TikiJavelin>(50);
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.collideY || NPC.wet)
			{
				NPC.frameCounter += 0.2f;
				NPC.frameCounter %= Main.npcFrameCount[NPC.type];
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = frame * frameHeight;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Kakamora/KakamoraDeath"), NPC.Center);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Kakamora_Gore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Kakamora_Gore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Kakamora_Gore3").Type, 1f);
			}
		}
	}
}