using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.NPCs
{
	public class WickedEnt : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wicked Ent");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Zombie];
		}

		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 50;
			npc.damage = 19;
			npc.defense = 7;
			npc.lifeMax = 48;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 60f;
			npc.knockBackResist = .10f;
			npc.aiStyle = 3;
			aiType = NPCID.SnowFlinx;
			animationType = NPCID.Zombie;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 78, hitDirection, -1f, 1, default(Color), .61f);
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Ent1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach2"), 1f);
				int n = 4;
				int deviation = Main.rand.Next(0, 300);
				for (int i = 0; i < n; i++)
				{
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = new Vector2(npc.velocity.X, npc.velocity.Y).RotatedBy(rotation);
					perturbedSpeed.Normalize();
					perturbedSpeed.X *= 5.5f;
					perturbedSpeed.Y *= 5.5f;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, 572, (int)((npc.damage * .5)), 0);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface) && (SpawnCondition.GoblinArmy.Chance == 0 && !Main.dayTime))
			{
				return spawnInfo.player.GetModPlayer<MyPlayer>(mod).ZoneReach ? 1.7f : 0f;
			}
			return 0f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/WickedEnt_Glow"));
        }
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AncientBark"));
		}
	}
}
