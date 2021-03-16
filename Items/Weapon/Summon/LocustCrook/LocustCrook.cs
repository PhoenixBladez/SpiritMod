using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.LocustCrook
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
			item.value = Item.sellPrice(0, 2, 0, 0);
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
			player.AddBuff(ModContent.BuffType<LocustBuff>(), 3600);
			position = Main.MouseWorld;
			Projectile.NewProjectile(position, Main.rand.NextVector2Circular(3, 3), type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, mod.GetTexture(Texture.Remove(0, mod.Name.Length + 1) + "_glow"), rotation, scale);
	}

	class LocustPlayer : ModPlayer
	{
		public bool Locusts = false;
		public override void Initialize() => Locusts = false;
		public override void ResetEffects() => Locusts = false;
	}

	class LocustBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Locust");
			Description.SetDefault("Bringer of a plague");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			LocustPlayer Player = player.GetModPlayer<LocustPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<LocustBig>()] > 0) {
				Player.Locusts = true;
			}

			if (!Player.Locusts) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}

			player.buffTime[buffIndex] = 18000;
		}
	}

	class LocustBig : ModProjectile
	{
		static readonly int traillength = 5;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Locust");
			Main.projFrames[projectile.type] = 3;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = traillength;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 40;
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Main.PlaySound(SoundID.Dig, projectile.Center);
			Collision.HitTiles(projectile.Center, projectile.velocity, projectile.width, projectile.height);
			projectile.velocity = new Vector2(projectile.velocity.X != oldVelocity.X ? -oldVelocity.X / 2 : projectile.velocity.X,
									 projectile.velocity.Y != oldVelocity.Y ? -oldVelocity.Y / 2 : projectile.velocity.Y);
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			LocustPlayer modPlayer = player.GetModPlayer<LocustPlayer>();
			if (player.dead || !player.active)
				modPlayer.Locusts = false;

			if (modPlayer.Locusts)
				projectile.timeLeft = 2;

			NPC target = null;
			float maxdist = 600;
			NPC miniontarget = projectile.OwnerMinionAttackTargetNPC;
			if (miniontarget != null && miniontarget.CanBeChasedBy(this) && CanHit(projectile.Center, miniontarget.Center) && CanHit(player.Center, miniontarget.Center)
				&& miniontarget.Distance(projectile.Center) <= maxdist && miniontarget.Distance(player.Center) <= maxdist * 3) {
				target = miniontarget;
			}

			else {
				var validtargets = Main.npc.Where(x => x != null && x.CanBeChasedBy(this) && CanHit(projectile.Center, x.Center) && CanHit(player.Center, x.Center)
														 && x.Distance(projectile.Center) <= maxdist && x.Distance(player.Center) <= maxdist * 2);

				foreach (NPC npc in validtargets) {
					if (npc.Distance(projectile.Center) <= maxdist) {
						maxdist = npc.Distance(projectile.Center);
						target = npc;
					}
				}
			}

			if(target == null) {
				IdleMovement(player);
			}
			else {
				TargettingBehavior(player, target);
			}

			UpdateFrame((int)MathHelper.Clamp(projectile.velocity.Length(), 10, 20));
			projectile.rotation = projectile.velocity.ToRotation();
		}
		private bool CanHit(Vector2 center1, Vector2 center2) => Collision.CanHit(center1, 0, 0, center2, 0, 0);

		private void IdleMovement(Player player)
		{
			projectile.tileCollide = false;
			int index = Main.projectile.Where(x => x.active && x.owner == projectile.owner && x.type == projectile.type && x.whoAmI < projectile.whoAmI).Count();
			Vector2 targetCenter = player.MountedCenter - new Vector2(25 * index * player.direction, 100);

			float acc = (projectile.Distance(targetCenter) > 300) ? 0.2f : 0.1f;
			float maxspeed = (projectile.Distance(targetCenter) > 300) ? (projectile.Distance(targetCenter)/50) : 6f;
			if(projectile.Distance(targetCenter) > 50)
				projectile.velocity += new Vector2((projectile.Center.X < targetCenter.X) ? acc : -acc, (projectile.Center.Y < targetCenter.Y) ? acc : -acc);

			if (projectile.velocity.Length() > maxspeed)
				projectile.velocity = Vector2.Normalize(projectile.velocity) * maxspeed;

			if (projectile.Distance(targetCenter) > 3000)
				projectile.Center = targetCenter;

			projectile.ai[0] = 0;
		}

		public override bool? CanCutTiles() => false;

		private void TargettingBehavior(Player player, NPC target)
		{
			projectile.tileCollide = true;
			projectile.ai[0]++;
			if(projectile.ai[0] < 40) { //get close to the target, but not too close
				float vel = MathHelper.Clamp(Math.Abs(projectile.Distance(target.Center) - 100) / 12, 6, 20);
				projectile.velocity = (projectile.Distance(target.Center) > 120) ? LerpVel(projectile.DirectionTo(target.Center) * vel) :
					(projectile.Distance(target.Center) < 80) ? LerpVel(projectile.DirectionFrom(target.Center) * vel/2) : LerpVel(Vector2.Zero);
			}

			else if(projectile.ai[0] < 60) //wind up before dash
				projectile.velocity = LerpVel(projectile.DirectionFrom(target.Center) * 10, 0.1f);
			
			if(projectile.ai[0] == 60) { //dash
				float vel = MathHelper.Clamp(projectile.Distance(target.Center) / 12, 10, 20);
				projectile.velocity = projectile.DirectionTo(target.Center) * vel;
				projectile.ai[1] = Main.rand.NextBool() ? -1 : 1;
				for(int i = 0; i < 6; i++) 
					Dust.NewDustPerfect(projectile.Center + Main.rand.NextVector2Circular(10, 10), ModContent.DustType<SandDust>(), -projectile.velocity.RotatedByRandom(MathHelper.Pi / 8));

				projectile.netUpdate = true;
			}

			if (projectile.ai[0] > 80 && projectile.ai[0] < 100) { //after a delay, circle backwards
				projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(4) * projectile.ai[1]);
				projectile.velocity *= 0.98f;
			}

			if(projectile.ai[0] == 100) { //reset
				projectile.ai[0] = 0;
				projectile.netUpdate = true;
			}
		}

		private Vector2 LerpVel(Vector2 desiredvel, float lerpstrength = 0.05f) => Vector2.Lerp(projectile.velocity, desiredvel, lerpstrength);

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			LocustNPC lnpc = target.GetGlobalNPC<LocustNPC>();
			lnpc.locustinfo = new LocustNPC.LocustInfo((int)(projectile.damage * 0.65f), Main.rand.Next(40, 80), projectile.owner);
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, target.whoAmI);
		}

		private void UpdateFrame(int framespersecond)
		{
			projectile.frameCounter++;
			if(projectile.frameCounter > 60/framespersecond) {
				projectile.frameCounter = 0;
				projectile.frame++;

				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.frame = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Texture2D glowtex = mod.GetTexture(Texture.Remove(0, mod.Name.Length + 1) + "_glow");
			Rectangle frame = new Rectangle(0, projectile.frame * tex.Height / Main.projFrames[projectile.type], tex.Width, tex.Height / Main.projFrames[projectile.type]);
			float rotation = projectile.rotation;
			SpriteEffects effects = SpriteEffects.None;
			if(Math.Abs(rotation) > MathHelper.PiOver2) {
				rotation -= MathHelper.Pi;
				effects = SpriteEffects.FlipHorizontally;
			}
			if(projectile.ai[0] > 60) {
				for(int i = 0; i < traillength; i++) {
					SpriteEffects effectstrail = SpriteEffects.None;
					float oldrotation = projectile.oldRot[i];
					if (Math.Abs(oldrotation) > MathHelper.PiOver2) {
						oldrotation -= MathHelper.Pi;
						effectstrail = SpriteEffects.FlipHorizontally;
					}
					float opacity = 0.5f * (traillength - i) / (float)(traillength);
					spriteBatch.Draw(tex, projectile.oldPos[i] + projectile.Size/2 - Main.screenPosition, frame, projectile.GetAlpha(lightColor) * opacity, oldrotation, frame.Size() / 2, projectile.scale, effectstrail, 0);
					spriteBatch.Draw(glowtex, projectile.oldPos[i] + projectile.Size / 2 - Main.screenPosition, frame, projectile.GetAlpha(Color.White) * opacity, oldrotation, frame.Size() / 2, projectile.scale, effectstrail, 0);
				}
			}
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, frame, projectile.GetAlpha(lightColor), rotation, frame.Size() / 2, projectile.scale, effects, 0);
			spriteBatch.Draw(glowtex, projectile.Center - Main.screenPosition, frame, projectile.GetAlpha(Color.White), rotation, frame.Size() / 2, projectile.scale, effects, 0);
			return false;
		}
	}

	class LocustNPC : GlobalNPC
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
			if(locustinfo.LocustTime > 0) {
				locustinfo.LocustTime--;

				if (Main.rand.NextBool(3)) 
					Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<SandDust>(), Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f), Scale: Main.rand.NextFloat(0.8f, 1.3f));
				
				if(locustinfo.LocustTime % 15 == 0 && Main.rand.NextBool(2) && Main.player[locustinfo.LocustPlayerindex].ownedProjectileCounts[ModContent.ProjectileType<LocustSmall>()] < 12) {

					Projectile proj = Projectile.NewProjectileDirect(npc.Center, Main.rand.NextVector2CircularEdge(6, 6), ModContent.ProjectileType<LocustSmall>(), locustinfo.LocustDamage, 1f, locustinfo.LocustPlayerindex);
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
				}
			}
		}
	}

	class LocustSmall : ModProjectile
	{
		static readonly int traillength = 3;
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
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 90;
			projectile.scale = 0.1f;
		}

		public override void AI()
		{
			projectile.scale = MathHelper.Lerp(projectile.scale, 1, 0.1f);
			projectile.width = projectile.height = 10;

			Player player = Main.player[projectile.owner];

			NPC target = null;
			float maxdist = 600;
			NPC miniontarget = projectile.OwnerMinionAttackTargetNPC;
			if (miniontarget != null && miniontarget.CanBeChasedBy(this) && CanHit(projectile.Center, miniontarget.Center) && CanHit(player.Center, miniontarget.Center) && miniontarget.Distance(projectile.Center) <= maxdist)
				target = miniontarget;

			else {
				var validtargets = Main.npc.Where(x => x != null && x.CanBeChasedBy(this) && CanHit(projectile.Center, x.Center) && CanHit(player.Center, x.Center)
														 && x.Distance(projectile.Center) <= maxdist && x.Distance(player.Center) <= maxdist * 2);

				foreach (NPC npc in validtargets) {
					if (npc.Distance(projectile.Center) <= maxdist) {
						maxdist = npc.Distance(projectile.Center);
						target = npc;
					}
				}
			}

			if (target != null && ++projectile.ai[0] > 30) {
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
			if (projectile.frameCounter > 60 / framespersecond) {
				projectile.frameCounter = 0;
				projectile.frame++;

				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.frame = 0;
			}
		}

		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 8; i++) 
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 167, Scale: Main.rand.NextFloat(0.7f, 1)).noGravity = true;
			
			Main.PlaySound(new LegacySoundStyle(SoundID.NPCKilled, 1).WithPitchVariance(0.2f).WithVolume(0.15f), projectile.Center);

			for(int j = 1; j <= 3; j++) {
				Gore gore = Gore.NewGoreDirect(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/LocustCrook/SmallLocustGore" + j.ToString()));
				gore.timeLeft = 20;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Texture2D glowtex = mod.GetTexture(Texture.Remove(0, mod.Name.Length + 1) + "_glow");
			Rectangle frame = new Rectangle(0, projectile.frame * tex.Height / Main.projFrames[projectile.type], tex.Width, tex.Height / Main.projFrames[projectile.type]);
			float rotation = projectile.rotation;
			SpriteEffects effects = SpriteEffects.None;
			if (Math.Abs(rotation) > MathHelper.PiOver2) {
				rotation -= MathHelper.Pi;
				effects = SpriteEffects.FlipHorizontally;
			}
			for (int i = 0; i < traillength; i++) {
				SpriteEffects effectstrail = SpriteEffects.None;
				float oldrotation = projectile.oldRot[i];
				if (Math.Abs(oldrotation) > MathHelper.PiOver2) {
					oldrotation -= MathHelper.Pi;
					effectstrail = SpriteEffects.FlipHorizontally;
				}
				float opacity = 0.5f * (traillength - i) / (float)(traillength);
				spriteBatch.Draw(tex, projectile.oldPos[i] + projectile.Size / 2 - Main.screenPosition, frame, projectile.GetAlpha(lightColor) * opacity, oldrotation, frame.Size() / 2, projectile.scale, effectstrail, 0);
			}
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, frame, projectile.GetAlpha(lightColor), rotation, frame.Size() / 2, projectile.scale, effects, 0);
			spriteBatch.Draw(glowtex, projectile.Center - Main.screenPosition, frame, projectile.GetAlpha(Color.White * 0.5f), rotation, frame.Size() / 2, projectile.scale, effects, 0);
			return false;
		}
	}
}