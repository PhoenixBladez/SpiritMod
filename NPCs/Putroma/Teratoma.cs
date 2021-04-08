using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;

namespace SpiritMod.NPCs.Putroma
{
	public class Teratoma : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putroma");
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 36;
			npc.damage = 20;
			npc.defense = 5;
			npc.lifeMax = 65;
			npc.HitSound = SoundID.NPCHit18;
			npc.DeathSound = SoundID.NPCDeath21;
			npc.value = 110f;
			npc.knockBackResist = 0.45f;
			npc.aiStyle = 3;
			aiType = NPCID.WalkingAntlion;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.PutromaBanner>();
		}
		public override void AI()
		{
			npc.rotation += .06f * npc.velocity.X;
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
			spriteBatch.Draw(mod.GetTexture("NPCs/Putroma/Teratoma_Eyes"), npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
				drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 22;
			int d1 = 184;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, Color.White, .34f);
			}
			if (Main.rand.Next(2) == 0) {
				Main.PlaySound(SoundID.NPCHit, npc.Center, 19);
				int tomaProj;
				tomaProj = Main.rand.Next(new int[] { mod.ProjectileType("Teratoma1"), mod.ProjectileType("Teratoma2"), mod.ProjectileType("Teratoma3") });
				bool expertMode = Main.expertMode;
				Main.PlaySound(SoundID.Item20, npc.Center);
				int damagenumber = expertMode ? 9 : 12;
				int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-4, 4), Main.rand.Next(-4, 0), tomaProj, damagenumber, 1, Main.myPlayer, 0, 0);
				Main.projectile[p].friendly = false;
				Main.projectile[p].hostile = true;
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma1"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma2"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma3"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma4"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma5"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma6"), Main.rand.NextFloat(.85f, 1.1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma7"), Main.rand.NextFloat(.85f, 1.1f));
				Main.PlaySound(SoundID.Zombie, npc.Center, 9);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneCorrupt && spawnInfo.player.ZoneOverworldHeight ? .2f : 0f;
		}
        public override void NPCLoot()
        {
            if (Main.rand.NextBool(3))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RottenChunk);
            }
            if (Main.rand.NextBool(5))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.WormTooth);
            }
            if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Meatballs>());
            }
        }
    }
}
