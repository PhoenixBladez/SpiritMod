using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.PirateLobber
{
	public class PirateLobber : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate Lobber");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 48;
			npc.damage = 29;
			npc.defense = 16;
			npc.lifeMax = 140;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 0, 20, 0);
			npc.knockBackResist = 0.35f;

			npc.buffImmune[20] = true;
			npc.buffImmune[31] = false;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.PirateLobberBanner>();
		}

		int frame = 0;
		bool attack = false;

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Vector2.Distance(npc.Center, target.Center);

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
				npc.velocity.X = .008f * npc.direction;
				if (frame == 5 && npc.frameCounter == 0)
					Attack();
			}
			else
			{
				npc.aiStyle = 3;
				aiType = NPCID.AngryBones;
			}
		}

		private void ResetFrame()
		{
			npc.frameCounter = 0;
			frame = 0;
		}

		private void Attack()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				var vel = new Vector2(npc.direction * 5, 0);
				Projectile.NewProjectile(npc.Center - vel, vel, ModContent.ProjectileType<PirateLobberBarrel>(), NPCUtils.ToActualDamage(60, 1.3f), 5);
			}
			Main.PlaySound(SoundID.Item, npc.Center, 1);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frame.Width = 60;
			npc.frame.X = attack ? npc.frame.Width : 0;
			int numFrames = attack ? 8 : 6;
			int frameDuation = attack ? 7 : 4;
			npc.frameCounter++;
			if (npc.frameCounter >= frameDuation)
			{
				npc.frameCounter = 0;
				frame++;
			}
			frame %= numFrames;
			npc.frame.Y = frameHeight * frame;
		}

		public override void NPCLoot()
		{
			if (Main.rand.NextBool(8000)) npc.DropItem(ItemID.CoinGun);
			if (Main.rand.NextBool(4000)) npc.DropItem(ItemID.LuckyCoin);
			if (Main.rand.NextBool(2000)) npc.DropItem(ItemID.DiscountCard);
			if (Main.rand.NextBool(1500)) npc.DropItem(ItemID.PirateStaff);
			if (Main.rand.NextBool(200)) npc.DropItem(ItemID.Cutlass);
			if (Main.rand.NextBool(100)) npc.DropItem(ItemID.GoldRing);

			if (Main.rand.NextBool(333))
			{
				int[] GoldFurniture = new int[] { ItemID.GoldenBathtub, ItemID.GoldenBed, ItemID.GoldenBookcase, ItemID.GoldenCandelabra, ItemID.GoldenCandle, ItemID.GoldenChair, ItemID.GoldenChandelier,
					ItemID.GoldenChest, ItemID.GoldenClock, ItemID.GoldenDoor, ItemID.GoldenDresser, ItemID.GoldenLamp, ItemID.GoldenLantern, ItemID.GoldenPiano, ItemID.GoldenShower, ItemID.GoldenSink,
					ItemID.GoldenSofa, ItemID.GoldenTable, ItemID.GoldenToilet, ItemID.GoldenWorkbench };

				if (Main.rand.NextBool(GoldFurniture.Length + 1))
					npc.DropItem(ItemID.GoldenPlatform, Main.rand.Next(20, 40));
				else
					npc.DropItem(Main.rand.Next(GoldFurniture));
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int i = 0; i < 3; ++i)
					Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), Vector2.UnitX * hitDirection * Main.rand.NextFloat(0.9f, 1f), mod.GetGoreSlot("Gores/PirateLobber/PirateLobber" + i));
				Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), Vector2.UnitX * hitDirection * Main.rand.NextFloat(0.9f, 1f), mod.GetGoreSlot("Gores/PirateLobber/PirateLobber1"));
			}

			int dustCount = Main.rand.Next(2, 5);
			for (int i = 0; i < dustCount; ++i)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2 * hitDirection, Main.rand.NextFloat(-0.8f, 0), 0, default, Main.rand.NextFloat(0.9f, 1.4f));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.Pirates.Chance * 0.1f;
	}

	public class PirateLobberBarrel : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate Barrel");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
		}

		int direction = 0; //0 is left, 1 is right
		float rotation = 2;
		int jumpCounter = 6;

		public override void AI()
		{
			jumpCounter++;
			rotation *= 1.005f;
			projectile.velocity.Y += 0.4F;
			if (projectile.velocity.X > 0)
				direction = 1;
			if (projectile.velocity.X < 0)
				direction = 0;
			if (direction == 0)
				projectile.rotation -= rotation / 25f;
			else
				projectile.rotation += rotation / 25f;
			projectile.spriteDirection = projectile.direction;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y >= 10f)
				return true;

			if (oldVelocity.X != projectile.velocity.X)
			{
				if (jumpCounter > 5)
				{
					jumpCounter = 0;

					Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);

					if (projectile.velocity.X < 2f)
						projectile.timeLeft = 2;
				}
				else if (projectile.timeLeft > 2)
					projectile.timeLeft = 2;
			}
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, projectile.Center);

			for (int i = 0; i < 5; ++i)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BorealWood, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 0));
			for (int i = 0; i < 2; ++i)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Iron, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 0));

			int goreCount = Main.rand.Next(1, 4);
			for (int i = 0; i < goreCount; ++i)
				Gore.NewGore(projectile.position + new Vector2(Main.rand.Next(projectile.width), Main.rand.Next(projectile.height)), projectile.velocity, mod.GetGoreSlot("Gores/PirateLobber/Barrel" + Main.rand.Next(3)));
		}
	}
}
