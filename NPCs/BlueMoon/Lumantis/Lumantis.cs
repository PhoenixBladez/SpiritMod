using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon.Lumantis
{
	public class Lumantis : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lumantis");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 40;
			npc.damage = 62;
			npc.defense = 20;
			npc.lifeMax = 560;
			npc.HitSound = SoundID.DD2_LightningBugHurt;
			npc.DeathSound = SoundID.NPCDeath34;
			npc.value = 760f;
			npc.knockBackResist = .2f;
			npc.aiStyle = 3;
			aiType = NPCID.WalkingAntlion;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.LumantisBanner>();
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return MyWorld.BlueMoon && NPC.CountNPCS(ModContent.NPCType<Lumantis>()) < 4 && spawnInfo.player.ZoneOverworldHeight ? .6f : 0f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 187, hitDirection, -1f, 1, default(Color), .81f);
				Dust.NewDust(npc.position, npc.width, npc.height, 205, hitDirection, -1f, 1, default(Color), .51f);
			}
			if (npc.life <= 0) {
				for (int k = 0; k < 11; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 187, hitDirection, -1f, 1, default(Color), .81f);
					Dust.NewDust(npc.position, npc.width, npc.height, 205, hitDirection, -1f, 1, default(Color), .71f);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Lumantis/Lumantis1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Lumantis/Lumantis2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Lumantis/Lumantis3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Lumantis/Lumantis4"), 1f);
			}
		}
		int timer;
		int frame;
		public override void AI()
		{
			Player player = Main.player[npc.target];
			timer++;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .196f * 3, .092f * 3, 0.214f * 3);
			++npc.ai[1];
			if (npc.ai[1] >= 600) {
				reflectPhase = true;
				npc.aiStyle = 0;
				if (player.position.X > npc.position.X) {
					npc.spriteDirection = 1;
				}
				else {
					npc.spriteDirection = -1;
				}
			}
			else {
				npc.aiStyle = 3;
				aiType = NPCID.WalkingAntlion;
				npc.spriteDirection = npc.direction;
				reflectPhase = false;
				npc.defense = 20;
				if (timer >= 4) {
					frame++;
					timer = 0;
				}
				if (frame >= 3) {
					frame = 0;
				}
			}
			if (npc.ai[1] >= 840) {
				npc.ai[1] = 0;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			if (!reflectPhase) {
				npc.frame.Y = frameHeight * frame;
			}
			else {
				npc.frame.Y = frameHeight * 4;
			}
		}
		bool reflectPhase;
		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{

			if (reflectPhase) {
				player.Hurt(PlayerDeathReason.LegacyEmpty(), item.damage, 0, true, false, false, -1);
				Main.PlaySound(SoundID.DD2_LightningBugZap, npc.position);
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if (reflectPhase && !projectile.minion && !Main.player[projectile.owner].channel) {
				projectile.hostile = true;
				projectile.friendly = false;
				Main.PlaySound(SoundID.DD2_LightningBugZap, npc.position);
				projectile.penetrate = 2;
				projectile.velocity.X = projectile.velocity.X * -1f;
			}
		}
		private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
		{
			float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
			Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

			int dust = Dust.NewDust(position - vec * distance, 0, 0, 205);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale *= .6f;
			Main.dust[dust].velocity = vel;
			Main.dust[dust].customData = follow;
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
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/BlueMoon/Lumantis/Lumantis_Glow"));
		}
		public override void NPCLoot()
		{
			if (Main.rand.NextBool(5))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MoonStone>());
			if (Main.rand.NextBool(100))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<StopWatch>());
			if (Main.rand.NextBool(10))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MoonJelly>());
		}
	}
}
