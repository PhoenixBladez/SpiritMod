using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Dusts;
using SpiritMod.Projectiles.BaseProj;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ScarabeusDrops.LocustCrook
{
	public class LocustCrook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Locust Crook");
			Tooltip.SetDefault("Conjures a friendly locust that dashes into enemies\nLocusts infest hit enemies, causing baby locusts to hatch from them");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 46;
			Item.damage = 14;
			Item.rare = ItemRarityID.Blue;
			Item.mana = 16;
			Item.value = Item.sellPrice(0, 1, 80, 0);
			Item.knockBack = 0;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<LocustBig>();
			Item.UseSound = SoundID.Item44;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			position = Main.MouseWorld;
			Projectile.NewProjectile(position, Main.rand.NextVector2Circular(3, 3), type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, Mod.GetTexture(Texture.Remove(0, Mod.Name.Length + 1) + "_glow"), rotation, scale);
	}

	[AutoloadMinionBuff("Locusts", "Bringer of a plague")]
	internal class LocustBig : BaseMinion
	{
		public LocustBig() : base(600, 1800, new Vector2(30, 30)) { }

		private static readonly int traillength = 5;
		public override void AbstractSetStaticDefaults()
		{
			DisplayName.SetDefault("Locust");
			Main.projFrames[Projectile.type] = 3;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = traillength;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void AbstractSetDefaults() => Projectile.localNPCHitCooldown = 20;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
			Projectile.Bounce(oldVelocity, 0.5f);
			return false;
		}

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			return true;
		}

		public override void IdleMovement(Player player)
		{
			Projectile.tileCollide = false;
			Vector2 targetCenter = player.MountedCenter - new Vector2(25 * IndexOfType * player.direction, 100);

			float acc = (Projectile.Distance(targetCenter) > 300) ? 0.2f : 0.1f;
			float maxspeed = (Projectile.Distance(targetCenter) > 300) ? (Projectile.Distance(targetCenter) / 50) : 6f;
			if (Projectile.Distance(targetCenter) > 50)
				Projectile.velocity += new Vector2((Projectile.Center.X < targetCenter.X) ? acc : -acc, (Projectile.Center.Y < targetCenter.Y) ? acc : -acc);

			if (Projectile.velocity.Length() > maxspeed)
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxspeed;

			if (Projectile.Distance(targetCenter) > 3000)
				Projectile.Center = targetCenter;

			Projectile.ai[0] = 0;
		}

		public override void TargettingBehavior(Player player, NPC target)
		{
			Projectile.tileCollide = true;
			Projectile.ai[0]++;
			if (Projectile.ai[0] < 40)
			{ //get close to the target, but not too close
				float vel = MathHelper.Clamp(Math.Abs(Projectile.Distance(target.Center) - 100) / 12, 6, 20);
				Projectile.velocity = (Projectile.Distance(target.Center) > 120) ? LerpVel(Projectile.DirectionTo(target.Center) * vel) :
					(Projectile.Distance(target.Center) < 80) ? LerpVel(Projectile.DirectionFrom(target.Center) * vel / 2) : LerpVel(Vector2.Zero);
			}
			else if (Projectile.ai[0] < 60) //wind up before dash
				Projectile.velocity = LerpVel(Projectile.DirectionFrom(target.Center) * 10, 0.1f);

			if (Projectile.ai[0] == 60)
			{ //dash
				float vel = MathHelper.Clamp(Projectile.Distance(target.Center) / 12, 10, 20);
				Projectile.velocity = Projectile.DirectionTo(target.Center) * vel;
				Projectile.ai[1] = Main.rand.NextBool() ? -1 : 1;
				for (int i = 0; i < 6; i++)
					Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(10, 10), ModContent.DustType<SandDust>(), -Projectile.velocity.RotatedByRandom(MathHelper.Pi / 8));

				Projectile.netUpdate = true;
			}

			if (Projectile.ai[0] > 80 && Projectile.ai[0] < 100)
			{ //after a delay, circle backwards
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(4) * Projectile.ai[1]);
				Projectile.velocity *= 0.98f;
			}

			if (Projectile.ai[0] == 100)
			{ //reset
				Projectile.ai[0] = 0;
				Projectile.netUpdate = true;
			}
		}

		public override bool DoAutoFrameUpdate(ref int framespersecond, ref int startframe, ref int endframe)
		{
			framespersecond = (int)MathHelper.Clamp(Projectile.velocity.Length(), 10, 20);
			return true;
		}

		private Vector2 LerpVel(Vector2 desiredvel, float lerpstrength = 0.05f) => Vector2.Lerp(Projectile.velocity, desiredvel, lerpstrength);

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			LocustNPC lnpc = target.GetGlobalNPC<LocustNPC>();
			lnpc.locustinfo = new LocustNPC.LocustInfo((int)(Projectile.damage * 0.65f), Main.rand.Next(40, 80), Projectile.owner);

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, target.whoAmI);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float rotation = Projectile.rotation;
			SpriteEffects effects = SpriteEffects.None;
			if (Math.Abs(rotation) > MathHelper.PiOver2)
			{
				rotation -= MathHelper.Pi;
				effects = SpriteEffects.FlipHorizontally;
			}
			if (Projectile.ai[0] > 60)
			{
				Projectile.QuickDrawTrail(spriteBatch, 0.5f, rotation, effects);
				Projectile.QuickDrawGlowTrail(spriteBatch, 0.5f, rotation: rotation, spriteEffects: effects);
			}
			Projectile.QuickDraw(spriteBatch, rotation, effects);
			Projectile.QuickDrawGlow(spriteBatch, rotation: rotation, spriteEffects : effects);
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = true;
			return true;
		}
	}

	internal class LocustNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public override bool CloneNewInstances => true;

		public struct LocustInfo
		{
			public int LocustDamage { get; set; }
			public int LocustTime { get; set; }
			public int LocustPlayerindex { get; set; }
			public LocustInfo(int LocustDamage, int LocustTime, int LocustPlayerindex)
			{
				this.LocustDamage = LocustDamage;
				this.LocustTime = LocustTime;
				this.LocustPlayerindex = LocustPlayerindex;
			}
		}

		public LocustInfo locustinfo = new LocustInfo(0, 0, 0);

		public override void PostAI(NPC npc)
		{
			if (locustinfo.LocustTime > 0)
			{
				locustinfo.LocustTime--;

				if (Main.rand.NextBool(3))
					Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<SandDust>(), Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f), Scale: Main.rand.NextFloat(0.8f, 1.3f));

				if (locustinfo.LocustTime % 15 == 0 && Main.rand.NextBool(2) && Main.player[locustinfo.LocustPlayerindex].ownedProjectileCounts[ModContent.ProjectileType<LocustSmall>()] < 12)
				{
					Projectile proj = Projectile.NewProjectileDirect(npc.Center, Main.rand.NextVector2CircularEdge(6, 6), ModContent.ProjectileType<LocustSmall>(), locustinfo.LocustDamage, 1f, locustinfo.LocustPlayerindex);
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
				}
			}
		}
	}

	internal class LocustSmall : ModProjectile
	{
		private static readonly int traillength = 3;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baby Locust");
			Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = traillength;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 10;
			Projectile.minion = true;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 90;
			Projectile.scale = 0.1f;
		}

		public override void AI()
		{
			if (Projectile.timeLeft > 80)
				Projectile.tileCollide = false;
            else
				Projectile.tileCollide = true;

			Projectile.scale = MathHelper.Lerp(Projectile.scale, 1, 0.1f);
			Projectile.width = Projectile.height = 10;

			Player player = Main.player[Projectile.owner];

			NPC target = null;
			float maxdist = 600;
			NPC miniontarget = Projectile.OwnerMinionAttackTargetNPC;
			if (miniontarget != null && miniontarget.CanBeChasedBy(this) && CanHit(Projectile.Center, miniontarget.Center) && CanHit(player.Center, miniontarget.Center) && miniontarget.Distance(Projectile.Center) <= maxdist)
				target = miniontarget;
			else
			{
				var validtargets = Main.npc.Where(x => x != null && x.CanBeChasedBy(this) && CanHit(Projectile.Center, x.Center) && CanHit(player.Center, x.Center)
														 && x.Distance(Projectile.Center) <= maxdist && x.Distance(player.Center) <= maxdist * 2);

				foreach (NPC npc in validtargets)
				{
					if (npc.Distance(Projectile.Center) <= maxdist)
					{
						maxdist = npc.Distance(Projectile.Center);
						target = npc;
					}
				}
			}

			if (target != null && ++Projectile.ai[0] > 30)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * Math.Max(8, Projectile.velocity.Length()), 0.08f);
				Projectile.velocity *= 1.02f + (Projectile.ai[0] / 3000);
			}

			UpdateFrame((int)MathHelper.Clamp(Projectile.velocity.Length(), 8, 16));
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		private bool CanHit(Vector2 center1, Vector2 center2) => Collision.CanHit(center1, 0, 0, center2, 0, 0);

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Projectile.ai[0] > 30;

		private void UpdateFrame(int framespersecond)
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 60 / framespersecond)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;

				if (Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Plantera_Green, Scale: Main.rand.NextFloat(0.7f, 1)).noGravity = true;

			SoundEngine.PlaySound(new LegacySoundStyle(SoundID.NPCKilled, 1).WithPitchVariance(0.2f).WithVolume(0.15f), Projectile.Center);

			for (int j = 1; j <= 3; j++)
			{
				Gore gore = Gore.NewGoreDirect(Projectile.Center, Projectile.velocity, Mod.Find<ModGore>("Gores/LocustCrook/SmallLocustGore" + j.ToString()).Type);
				gore.timeLeft = 20;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float rotation = Projectile.rotation;
			SpriteEffects effects = SpriteEffects.None;
			if (Math.Abs(rotation) > MathHelper.PiOver2)
			{
				rotation -= MathHelper.Pi;
				effects = SpriteEffects.FlipHorizontally;
			}
			Projectile.QuickDrawTrail(spriteBatch, 0.5f, rotation, effects);
			Projectile.QuickDrawGlowTrail(spriteBatch, 0.5f, rotation: rotation, spriteEffects: effects);

			Projectile.QuickDraw(spriteBatch, rotation, effects);
			Projectile.QuickDrawGlow(spriteBatch, rotation: rotation, spriteEffects: effects); 
			return false;
		}
	}
}