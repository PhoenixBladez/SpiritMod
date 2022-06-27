using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlazingRattler
{
	public class BlazingRattler : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Rattler");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 58;
			NPC.height = 46;
			NPC.damage = 20;
			NPC.defense = 16;
			NPC.lifeMax = 240;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = Item.buyPrice(0, 0, 8, 0);
			NPC.knockBackResist = .5f;
			NPC.aiStyle = 3;
			AIType = 218;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BlazingRattlerBanner>();
		}

		int hitCounter;

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.GoldenKey, 153));
			npcLoot.Add(ItemDropRule.Common(ItemID.Nazar, 75));
			npcLoot.Add(ItemDropRule.Common(ItemID.TallyCounter, 100));
			npcLoot.Add(ItemDropRule.Common(ItemID.BoneWand, 250));
			npcLoot.Add(ItemDropRule.Common(ItemID.Skull, 500));
			npcLoot.Add(ItemDropRule.Common(ItemID.LivingFireBlock, 1, 10, 25));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.SkullPile>(), 6));
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(hitCounter);
		public override void ReceiveExtraAI(BinaryReader reader) => hitCounter = reader.ReadInt32();

		public override void HitEffect(int hitDirection, double damage)
		{
			NPC.scale -= .02f;

			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			hitCounter++;
			if (hitCounter >= 3)
			{
				hitCounter = 0;
				Vector2 dir = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center).RotatedByRandom(0.02f) * 4f;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
					int p = Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center.X, NPC.Center.Y, dir.X, dir.Y, ProjectileID.BallofFire, Main.expertMode ? 5 : 12, 1, Main.myPlayer, 0, 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
				}
			}

			if (NPC.life <= 0)
			{
				SoundEngine.PlaySound(SoundID.Item74, NPC.Center);

				for (int i = 1; i < 8; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Rattler/Rattler" + i).Type, 1f);

				Vector2 dir = new Vector2(Main.rand.NextFloat(4, 5)).RotatedByRandom(MathHelper.TwoPi);
				bool expertMode = Main.expertMode;
				for (int i = 0; i < 3; ++i)
				{
					int p = Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center.X, NPC.Center.Y, dir.X, dir.Y, ProjectileID.BallofFire, expertMode ? 10 : 15, 1, Main.myPlayer, 0, 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
				}
			}
		}

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			if (NPC.scale <= .85f)
				NPC.scale = .85f;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.downedPlantBoss)
				return spawnInfo.Player.ZoneDungeon && !NPC.AnyNPCs(ModContent.NPCType<BlazingRattler>()) ? 0.0015f : 0f;
			return spawnInfo.Player.ZoneDungeon && !NPC.AnyNPCs(ModContent.NPCType<BlazingRattler>()) ? 0.04f : 0f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, ModContent.Request<Texture2D>("NPCs/BlazingRattler/BlazingRattler_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
	}
}