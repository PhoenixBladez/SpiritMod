using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.MoonjellyEvent
{
	public class MoonjellyGiant : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tethervolt Jelly");
			Main.npcFrameCount[npc.type] = 8;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 70;
			npc.damage = 16;
			npc.defense = 10;
			npc.lifeMax = 65;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.value = 250f;
			npc.knockBackResist = 0f;
			npc.alpha = 100;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = -1;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.GiantJellyBanner>();
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.08f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override bool PreAI()
		{
			npc.TargetClosest(true);
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);

			Player target = Main.player[npc.target];

			npc.spriteDirection = npc.direction;

			Vector2 vector2_1 = target.Center - npc.Center + new Vector2(0, -100f);
			float distance = vector2_1.Length();

			Vector2 desiredVelocity = npc.velocity;
			if (distance < 20)
				desiredVelocity.Normalize();

			if (distance < 40.0)
				desiredVelocity = vector2_1 * (5f * 0.025f);
			else if (distance < 80.0)
				desiredVelocity = vector2_1 * (5f * 0.075f);
			else
				desiredVelocity = vector2_1 * 5f;

			npc.SimpleFlyMovement(desiredVelocity, 0.05f);
			npc.rotation = npc.velocity.X * 0.1f;

			if (npc.ai[0] == 0f)
			{
				for (int i = 0; i < 5; i++)
				{
					Vector2 vel = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * new Vector2(Main.rand.Next(3, 8), Main.rand.Next(3, 8));
					int p = Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-20, 20), npc.Center.Y + Main.rand.Next(-20, 20), vel.X, vel.Y, ModContent.ProjectileType<ElectricJellyfishOrbiter>(), NPCUtils.ToActualDamage(30, 1.5f), 0.0f, Main.myPlayer, 0.0f, npc.whoAmI);
					Main.projectile[p].scale = Main.rand.NextFloat(.6f, .95f);
					Main.projectile[p].ai[0] = npc.whoAmI;

					npc.ai[0] = 1f;
					npc.netUpdate = true;
				}
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/MoonjellyEvent/MoonjellyGiant_Glow"));

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 15; k++)
				Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.65f).noGravity = true;

			if (npc.life <= 0)
				for (int k = 0; k < 30; k++)
					Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(7), 0, default, 0.95f).noGravity = true;
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(2, 5));

			if (Main.rand.NextBool(2))
				npc.DropItem(mod.ItemType("MoonJelly"));

			if (Main.rand.NextBool(18))
				npc.DropItem(mod.ItemType("Moonlight_Sack"));
		}
	}
}