using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Items.Sets.BriarDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;

namespace SpiritMod.NPCs.Reach
{
	public class Reachman : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feral Hunter");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 54;
			npc.damage = 22;
			npc.defense = 8;
			npc.lifeMax = 66;
			npc.HitSound = SoundID.NPCHit2;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 90f;
			npc.knockBackResist = .34f;
			npc.aiStyle = 3;

			//drawOffsetY = 4;
			aiType = NPCID.Zombie;
			banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.ReachmanBanner>();
        }

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.10f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse  && (SpawnCondition.GoblinArmy.Chance == 0))
				return spawnInfo.player.GetSpiritPlayer().ZoneReach ? 1.7f : 0f;
			return 0f;
		}

		public override void AI() => Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), 0.46f, 0.32f, .1f);

		public override void NPCLoot()
		{
			if (Main.rand.Next(20) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SanctifiedStabber>());
            if (Main.rand.NextBool(33))
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CaesarSalad>());
            if (!Main.dayTime)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<EnchantedLeaf>());
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(10) == 0 && Main.expertMode)
				target.AddBuff(148, 2000);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Reach/Reachman_Glow"));

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.GrassBlades, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, 7, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach2"), 1f);
			}
		}
	}
}
