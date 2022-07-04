using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using Terraria.GameContent.ItemDropRules;

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
			NPC.width = 34;
			NPC.height = 36;
			NPC.damage = 20;
			NPC.defense = 5;
			NPC.lifeMax = 65;
			NPC.HitSound = SoundID.NPCHit18;
			NPC.DeathSound = SoundID.NPCDeath21;
			NPC.value = 110f;
			NPC.knockBackResist = 0.45f;
			NPC.aiStyle = 3;
			AIType = NPCID.WalkingAntlion;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.PutromaBanner>();
		}
		public override void AI()
		{
			NPC.rotation += .06f * NPC.velocity.X;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
				drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Putroma/Teratoma_Eyes").Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
				drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Pot, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ScourgeOfTheCorruptor, 2.5f * hitDirection, -2.5f, 0, Color.White, .34f);
			}

			if (Main.rand.Next(2) == 0) {
				SoundEngine.PlaySound(SoundID.NPCHit19, NPC.Center);
				int tomaProj;
				tomaProj = Main.rand.Next(new int[] { ModContent.ProjectileType<Teratoma1>(), ModContent.ProjectileType<Teratoma2>(), ModContent.ProjectileType<Teratoma3>() });
				SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
				int damagenumber = Main.expertMode ? 9 : 12;
				int p = Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center.X, NPC.Center.Y, Main.rand.Next(-4, 4), Main.rand.Next(-4, 0), tomaProj, damagenumber, 1, Main.myPlayer, 0, 0);
				Main.projectile[p].friendly = false;
				Main.projectile[p].hostile = true;
			}

			if (NPC.life <= 0) {
				for (int i = 1; i < 8; ++i)
					Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Teratoma/Teratoma" + i).Type, Main.rand.NextFloat(.85f, 1.1f));
				SoundEngine.PlaySound(SoundID.Zombie9, NPC.Center);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.Player.ZoneCorrupt && spawnInfo.Player.ZoneOverworldHeight ? .2f : 0f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.RottenChunk, 3));
			npcLoot.Add(ItemDropRule.Common(ItemID.WormTooth, 5));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Meatballs>(), 16));
		}
	}
}
