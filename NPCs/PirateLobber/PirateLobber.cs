using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.PirateLobber
{
	public class PirateLobber : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate Lobber");
			Main.npcFrameCount[NPC.type] = 8;
		}

		public override void SetDefaults()
		{
			NPC.width = 34;
			NPC.height = 48;
			NPC.damage = 29;
			NPC.defense = 16;
			NPC.lifeMax = 140;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(0, 0, 20, 0);
			NPC.knockBackResist = 0.35f;

			NPC.buffImmune[20] = true;
			NPC.buffImmune[31] = false;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.PirateLobberBanner>();
		}

		int frame = 0;
		bool attack = false;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];
			int distance = (int)Vector2.Distance(NPC.Center, target.Center);

			if (distance < 400)
			{
				if (!attack)
					ResetFrame();
				attack = true;
			}

			if (distance > 500)
			{
				if (attack)
					ResetFrame();
				attack = false;
			}

			if (attack)
			{
				NPC.velocity.X = .008f * NPC.direction;
				if (frame == 5 && NPC.frameCounter == 0)
					Attack();
			}
			else
			{
				NPC.aiStyle = 3;
				AIType = NPCID.AngryBones;
			}
		}

		private void ResetFrame()
		{
			NPC.frameCounter = 0;
			frame = 0;
		}

		private void Attack()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				var vel = new Vector2(NPC.direction * 5, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - vel, vel, ModContent.ProjectileType<PirateLobberBarrel>(), NPCUtils.ToActualDamage(60, 1.3f), 5);
			}
			SoundEngine.PlaySound(SoundID.Item1, NPC.Center);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frame.Width = 60;
			NPC.frame.X = attack ? NPC.frame.Width : 0;
			int numFrames = attack ? 8 : 6;
			int frameDuation = attack ? 7 : 4;
			NPC.frameCounter++;
			if (NPC.frameCounter >= frameDuation)
			{
				NPC.frameCounter = 0;
				frame++;
			}
			frame %= numFrames;
			NPC.frame.Y = frameHeight * frame;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ItemID.CoinGun, 8000);
			npcLoot.AddCommon(ItemID.LuckyCoin, 4000);
			npcLoot.AddCommon(ItemID.DiscountCard, 2000);
			npcLoot.AddCommon(ItemID.PirateStaff, 1500);
			npcLoot.AddCommon(ItemID.Cutlass, 200);
			npcLoot.AddCommon(ItemID.GoldRing, 200);

			int[] GoldFurniture = new int[] { ItemID.GoldenBathtub, ItemID.GoldenBed, ItemID.GoldenBookcase, ItemID.GoldenCandelabra, ItemID.GoldenCandle, ItemID.GoldenChair, ItemID.GoldenChandelier,
					ItemID.GoldenChest, ItemID.GoldenClock, ItemID.GoldenDoor, ItemID.GoldenDresser, ItemID.GoldenLamp, ItemID.GoldenLantern, ItemID.GoldenPiano, ItemID.GoldenShower, ItemID.GoldenSink,
					ItemID.GoldenSofa, ItemID.GoldenTable, ItemID.GoldenToilet, ItemID.GoldenWorkbench };
			npcLoot.AddOneFromOptions(333, GoldFurniture);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 3; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)), Vector2.UnitX * hitDirection * Main.rand.NextFloat(0.9f, 1f), Mod.Find<ModGore>("SpiritMod/Gores/PirateLobber/PirateLobber" + i).Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)), Vector2.UnitX * hitDirection * Main.rand.NextFloat(0.9f, 1f), Mod.Find<ModGore>("SpiritMod/Gores/PirateLobber/PirateLobber1").Type);
			}

			int dustCount = Main.rand.Next(2, 5);
			for (int i = 0; i < dustCount; ++i)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2 * hitDirection, Main.rand.NextFloat(-0.8f, 0), 0, default, Main.rand.NextFloat(0.9f, 1.4f));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.Pirates.Chance * 0.1f;
	}

	public class PirateLobberBarrel : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate Barrel");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
		}

		int direction = 0; //0 is left, 1 is right
		float rotation = 2;
		int jumpCounter = 6;

		public override void AI()
		{
			jumpCounter++;
			rotation *= 1.005f;
			Projectile.velocity.Y += 0.4F;
			if (Projectile.velocity.X > 0)
				direction = 1;
			if (Projectile.velocity.X < 0)
				direction = 0;
			if (direction == 0)
				Projectile.rotation -= rotation / 25f;
			else
				Projectile.rotation += rotation / 25f;
			Projectile.spriteDirection = Projectile.direction;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y >= 10f)
				return true;

			if (oldVelocity.X != Projectile.velocity.X)
			{
				if (jumpCounter > 5)
				{
					jumpCounter = 0;

					Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);

					if (Projectile.velocity.X < 2f)
						Projectile.timeLeft = 2;
				}
				else if (Projectile.timeLeft > 2)
					Projectile.timeLeft = 2;
			}
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

			for (int i = 0; i < 5; ++i)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BorealWood, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 0));
			for (int i = 0; i < 2; ++i)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Iron, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 0));

			int goreCount = Main.rand.Next(1, 4);
			for (int i = 0; i < goreCount; ++i)
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.position + new Vector2(Main.rand.Next(Projectile.width), Main.rand.Next(Projectile.height)), Projectile.velocity, Mod.Find<ModGore>("SpiritMod/Gores/PirateLobber/Barrel" + Main.rand.Next(3)).Type);
		}
	}
}
