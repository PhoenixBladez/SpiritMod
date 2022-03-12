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
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			item.width = item.height = 46;
			item.damage = 14;
			item.rare = ItemRarityID.Blue;
			item.mana = 16;
			item.value = Item.sellPrice(0, 1, 80, 0);
			item.knockBack = 0;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<LocustBig>();
			item.UseSound = SoundID.Item44;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			Projectile.NewProjectile(position, Main.rand.NextVector2Circular(3, 3), type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, mod.GetTexture(Texture.Remove(0, mod.Name.Length + 1) + "_glow"), rotation, scale);
	}

	[AutoloadMinionBuff("Locusts", "Bringer of a plague")]
	internal class LocustBig : BaseMinion
	{
		public LocustBig() : base(600, 1800, new Vector2(30, 30)) { }

		private static readonly int traillength = 5;
		public override void AbstractSetStaticDefaults()
		{
			DisplayName.SetDefault("Locust");
			Main.projFrames[projectile.type] = 3;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = traillength;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void AbstractSetDefaults() => projectile.localNPCHitCooldown = 20;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Main.PlaySound(SoundID.Dig, projectile.Center);
			Collision.HitTiles(projectile.Center, projectile.velocity, projectile.width, projectile.height);
			projectile.Bounce(oldVelocity, 0.5f);
			return false;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
			return true;
		}

		public override void IdleMovement(Player player)
		{
			projectile.tileCollide = false;
			Vector2 targetCenter = player.MountedCenter - new Vector2(25 * IndexOfType * player.direction, 100);

			float acc = (projectile.Distance(targetCenter) > 300) ? 0.2f : 0.1f;
			float maxspeed = (projectile.Distance(targetCenter) > 300) ? (projectile.Distance(targetCenter) / 50) : 6f;
			if (projectile.Distance(targetCenter) > 50)
				projectile.velocity += new Vector2((projectile.Center.X < targetCenter.X) ? acc : -acc, (projectile.Center.Y < targetCenter.Y) ? acc : -acc);

			if (projectile.velocity.Length() > maxspeed)
				projectile.velocity = Vector2.Normalize(projectile.velocity) * maxspeed;

			if (projectile.Distance(targetCenter) > 3000)
				projectile.Center = targetCenter;

			projectile.ai[0] = 0;
		}

		public override void TargettingBehavior(Player player, NPC target)
		{
			projectile.tileCollide = true;
			projectile.ai[0]++;
			if (projectile.ai[0] < 40)
			{ //get close to the target, but not too close
				float vel = MathHelper.Clamp(Math.Abs(projectile.Distance(target.Center) - 100) / 12, 6, 20);
				projectile.velocity = (projectile.Distance(target.Center) > 120) ? LerpVel(projectile.DirectionTo(target.Center) * vel) :
					(projectile.Distance(target.Center) < 80) ? LerpVel(projectile.DirectionFrom(target.Center) * vel / 2) : LerpVel(Vector2.Zero);
			}
			else if (projectile.ai[0] < 60) //wind up before dash
				projectile.velocity = LerpVel(projectile.DirectionFrom(target.Center) * 10, 0.1f);

			if (projectile.ai[0] == 60)
			{ //dash
				float vel = MathHelper.Clamp(projectile.Distance(target.Center) / 12, 10, 20);
				projectile.velocity = projectile.DirectionTo(target.Center) * vel;
				projectile.ai[1] = Main.rand.NextBool() ? -1 : 1;
				for (int i = 0; i < 6; i++)
					Dust.NewDustPerfect(projectile.Center + Main.rand.NextVector2Circular(10, 10), ModContent.DustType<SandDust>(), -projectile.velocity.RotatedByRandom(MathHelper.Pi / 8));

				projectile.netUpdate = true;
			}

			if (projectile.ai[0] > 80 && projectile.ai[0] < 100)
			{ //after a delay, circle backwards
				projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(4) * projectile.ai[1]);
				projectile.velocity *= 0.98f;
			}

			if (projectile.ai[0] == 100)
			{ //reset
				projectile.ai[0] = 0;
				projectile.netUpdate = true;
			}
		}

		public override bool DoAutoFrameUpdate(ref int framespersecond, ref int startframe, ref int endframe)
		{
			framespersecond = (int)MathHelper.Clamp(projectile.velocity.Length(), 10, 20);
			return true;
		}

		private Vector2 LerpVel(Vector2 desiredvel, float lerpstrength = 0.05f) => Vector2.Lerp(projectile.velocity, desiredvel, lerpstrength);

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			LocustNPC lnpc = target.GetGlobalNPC<LocustNPC>();
			lnpc.locustinfo = new LocustNPC.LocustInfo((int)(projectile.damage * 0.65f), Main.rand.Next(40, 80), projectile.owner);

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, target.whoAmI);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float rotation = projectile.rotation;
			SpriteEffects effects = SpriteEffects.None;
			if (Math.Abs(rotation) > MathHelper.PiOver2)
			{
				rotation -= MathHelper.Pi;
				effects = SpriteEffects.FlipHorizontally;
			}
			if (projectile.ai[0] > 60)
			{
				projectile.QuickDrawTrail(spriteBatch, 0.5f, rotation, effects);
				projectile.QuickDrawGlowTrail(spriteBatch, 0.5f, rotation: rotation, spriteEffects: effects);
			}
			projectile.QuickDraw(spriteBatch, rotation, effects);
			projectile.QuickDrawGlow(spriteBatch, rotation: rotation, spriteEffects : effects);
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
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
			Main.projFrames[projectile.type] = 2;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = traillength;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 10;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 90;
			projectile.scale = 0.1f;
		}

		public override void AI()
		{
			if (projectile.timeLeft > 80)
				projectile.tileCollide = false;
            else
				projectile.tileCollide = true;

			projectile.scale = MathHelper.Lerp(projectile.scale, 1, 0.1f);
			projectile.width = projectile.height = 10;

			Player player = Main.player[projectile.owner];

			NPC target = null;
			float maxdist = 600;
			NPC miniontarget = projectile.OwnerMinionAttackTargetNPC;
			if (miniontarget != null && miniontarget.CanBeChasedBy(this) && CanHit(projectile.Center, miniontarget.Center) && CanHit(player.Center, miniontarget.Center) && miniontarget.Distance(projectile.Center) <= maxdist)
				target = miniontarget;
			else
			{
				var validtargets = Main.npc.Where(x => x != null && x.CanBeChasedBy(this) && CanHit(projectile.Center, x.Center) && CanHit(player.Center, x.Center)
														 && x.Distance(projectile.Center) <= maxdist && x.Distance(player.Center) <= maxdist * 2);

				foreach (NPC npc in validtargets)
				{
					if (npc.Distance(projectile.Center) <= maxdist)
					{
						maxdist = npc.Distance(projectile.Center);
						target = npc;
					}
				}
			}

			if (target != null && ++projectile.ai[0] > 30)
			{
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * Math.Max(8, projectile.velocity.Length()), 0.08f);
				projectile.velocity *= 1.02f + (projectile.ai[0] / 3000);
			}

			UpdateFrame((int)MathHelper.Clamp(projectile.velocity.Length(), 8, 16));
			projectile.rotation = projectile.velocity.ToRotation();
		}

		private bool CanHit(Vector2 center1, Vector2 center2) => Collision.CanHit(center1, 0, 0, center2, 0, 0);

		public override bool CanDamage() => projectile.ai[0] > 30;

		private void UpdateFrame(int framespersecond)
		{
			projectile.frameCounter++;
			if (projectile.frameCounter > 60 / framespersecond)
			{
				projectile.frameCounter = 0;
				projectile.frame++;

				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.frame = 0;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Plantera_Green, Scale: Main.rand.NextFloat(0.7f, 1)).noGravity = true;

			Main.PlaySound(new LegacySoundStyle(SoundID.NPCKilled, 1).WithPitchVariance(0.2f).WithVolume(0.15f), projectile.Center);

			for (int j = 1; j <= 3; j++)
			{
				Gore gore = Gore.NewGoreDirect(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/LocustCrook/SmallLocustGore" + j.ToString()));
				gore.timeLeft = 20;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float rotation = projectile.rotation;
			SpriteEffects effects = SpriteEffects.None;
			if (Math.Abs(rotation) > MathHelper.PiOver2)
			{
				rotation -= MathHelper.Pi;
				effects = SpriteEffects.FlipHorizontally;
			}
			projectile.QuickDrawTrail(spriteBatch, 0.5f, rotation, effects);
			projectile.QuickDrawGlowTrail(spriteBatch, 0.5f, rotation: rotation, spriteEffects: effects);

			projectile.QuickDraw(spriteBatch, rotation, effects);
			projectile.QuickDrawGlow(spriteBatch, rotation: rotation, spriteEffects: effects); 
			return false;
		}
	}
}