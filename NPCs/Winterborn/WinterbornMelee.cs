using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.CryoliteSet;
using SpiritMod.Items.Consumable.Food;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs.DoT;
using SpiritMod.Mechanics.QuestSystem;

namespace SpiritMod.NPCs.Winterborn
{
	public class WinterbornMelee : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winterborn");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 46;
			NPC.damage = 30;
			NPC.defense = 11;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCDeath15;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 189f;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Frostburn] = true;
			NPC.buffImmune[ModContent.BuffType<CryoCrush>()] = true;
			NPC.knockBackResist = .07f;
			NPC.aiStyle = 3;
			AIType = NPCID.ArmoredViking;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.WinterbornBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => NPC.downedBoss3 && ((spawnInfo.SpawnTileY > Main.rockLayer && spawnInfo.Player.ZoneSnow) || (spawnInfo.Player.ZoneSnow && Main.raining && spawnInfo.Player.ZoneOverworldHeight)) ? 0.12f : 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UnusedWhiteBluePurple, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare_Blue, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
				}
			}
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Winterborn/WinterbornGore1").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Winterborn/WinterbornGore2").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Winterborn/WinterbornGore2").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Winterborn/WinterbornGore3").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Winterborn/WinterbornGore3").Type, 1f);

				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Flare_Blue, 0f, 0f, 100, default, .8f);
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.35f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, .43f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 3f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.GetTexture("NPCs/Winterborn/WinterbornMelee_Glow"));

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.25f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Chilled, 300);

			if (Main.rand.Next(10) == 0)
				target.AddBuff(BuffID.Frozen, 120);
		}

		public override void AI()
		{
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.079f, 0.132f, .2f);
			NPC.spriteDirection = NPC.direction;
		}

		public override void OnKill()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<CryoliteOre>(), 2 + Main.rand.Next(3, 7));

			if (Main.rand.NextBool(16))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Popsicle>());

			if (QuestManager.GetQuest<Mechanics.QuestSystem.Quests.IceDeityQuest>().IsActive && Main.rand.NextBool(5))
				Item.NewItem(NPC.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.IceDeityShard1>());
		}
	}
}