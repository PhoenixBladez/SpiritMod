using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlazingRattler
{
	public class BlazingRattler : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Rattler");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 58;
			npc.height = 46;
			npc.damage = 20;
			npc.defense = 16;
			npc.lifeMax = 240;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = Item.buyPrice(0, 0, 8, 0);
			npc.knockBackResist = .5f;
			npc.aiStyle = 3;
			aiType = 218;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.BlazingRattlerBanner>();
		}

		int hitCounter;

		public override void NPCLoot()
		{
			if (Main.rand.Next(153) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldenKey);
			if (Main.rand.Next(75) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Nazar);
			if (Main.rand.Next(100) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TallyCounter);
			if (Main.rand.Next(250) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BoneWand);
			if (Main.rand.Next(500) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Skull);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LivingFireBlock, Main.rand.Next(10, 25));
			if (Main.rand.Next(6) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Placeable.Furniture.SkullPile>());
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(hitCounter);
		public override void ReceiveExtraAI(BinaryReader reader) => hitCounter = reader.ReadInt32();

		public override void HitEffect(int hitDirection, double damage)
		{
			npc.scale -= .02f;

			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Dirt, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			hitCounter++;
			if (hitCounter >= 3)
			{
				hitCounter = 0;
				Vector2 dir = Vector2.Normalize(Main.player[npc.target].Center - npc.Center).RotatedByRandom(0.02f) * 4f;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Main.PlaySound(SoundID.Item20, npc.Center);
					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X, dir.Y, ProjectileID.BallofFire, Main.expertMode ? 5 : 12, 1, Main.myPlayer, 0, 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
				}
			}

			if (npc.life <= 0)
			{
				Main.PlaySound(SoundID.Item, npc.Center, 74);

				for (int i = 1; i < 8; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Rattler/Rattler" + i), 1f);

				Vector2 dir = new Vector2(Main.rand.NextFloat(4, 5)).RotatedByRandom(MathHelper.TwoPi);
				bool expertMode = Main.expertMode;
				for (int i = 0; i < 3; ++i)
				{
					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X, dir.Y, ProjectileID.BallofFire, expertMode ? 10 : 15, 1, Main.myPlayer, 0, 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
				}
			}
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			if (npc.scale <= .85f)
				npc.scale = .85f;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.downedPlantBoss)
				return spawnInfo.player.ZoneDungeon && !NPC.AnyNPCs(ModContent.NPCType<BlazingRattler>()) ? 0.0015f : 0f;
			return spawnInfo.player.ZoneDungeon && !NPC.AnyNPCs(ModContent.NPCType<BlazingRattler>()) ? 0.04f : 0f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/BlazingRattler/BlazingRattler_Glow"));

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}