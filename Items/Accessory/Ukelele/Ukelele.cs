using Terraria;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Accessory.Ukelele
{
	public class Ukelele : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ukulele");
			Tooltip.SetDefault("Hitting enemies has a chance to create a chain of lightning\n'...and his music was electric.'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 12));
		}

		public override void SetDefaults()
		{
			item.width = 60;
			item.height = 58;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(new Vector2(item.Center.X, item.Center.Y), 0.075f, 0.231f, 0.255f);

			Rectangle frame = Main.itemTexture[item.type].Frame(1, 1, 0, 0);
			var origin = new Vector2(Main.itemTexture[item.type].Width / 2, Main.itemTexture[item.type].Height / 2);
			float cos = (float)(Math.Cos(Main.GlobalTime % 2.4 / 2.4 * MathHelper.TwoPi) / 1.0 + 0.5);
			Texture2D texture = Main.itemTexture[item.type];
			Color baseCol = new Color(sbyte.MaxValue - item.alpha, sbyte.MaxValue - item.alpha, sbyte.MaxValue - item.alpha, 0).MultiplyRGBA(Color.White);

			for (int i = 0; i < 5; ++i)
			{
				Color drawCol = item.GetAlpha(baseCol) * 1f;
				Vector2 gPos = item.Center + ((i / 5 * MathHelper.TwoPi) + rotation).ToRotationVector2() * 2f - Main.screenPosition - new Vector2(texture.Width, texture.Height) * item.scale / 2f + origin * item.scale + new Vector2(0.0f, -2f);
				Main.spriteBatch.Draw(mod.GetTexture("Items/Accessory/Ukelele/Ukelele_Glow"), gPos, frame, drawCol, rotation, origin, item.scale, SpriteEffects.None, 0.0f);
			}

			for (int i = 0; i < 4; ++i)
			{
				Vector2 gPos = item.Center + ((i / 4 * MathHelper.TwoPi) + rotation).ToRotationVector2() * (float)(2.0 * cos + 2.0) - Main.screenPosition - new Vector2(texture.Width, texture.Height) * item.scale / 2f + origin * item.scale + new Vector2(0.0f, -2f);
				Main.spriteBatch.Draw(mod.GetTexture("Items/Accessory/Ukelele/Ukelele_Glow"), gPos, frame, baseCol, rotation, origin, item.scale, SpriteEffects.None, 0.0f);
			}

			Vector2 glowPos = item.Center - Main.screenPosition - new Vector2(texture.Width, (texture.Height / 1)) * item.scale / 2f + origin * item.scale + new Vector2(0.0f, -2f);
			Main.spriteBatch.Draw(mod.GetTexture("Items/Accessory/Ukelele/Ukelele_Glow"), glowPos, frame, baseCol, rotation, origin, item.scale, SpriteEffects.None, 0.0f);
			return true;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<UkelelePlayer>().active = true;
	}

	public class UkelelePlayer : ModPlayer
    {
		public bool active = false;
		int overcharge = 0;

		public override void ResetEffects()
        {
            active = false;
			if (overcharge > 0)
			overcharge--;
        }

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
			if (active && proj.type != ModContent.ProjectileType<UkeleleProj>() && Main.rand.Next(4) == 0 && overcharge < 30)
			{
				Main.PlaySound(SoundID.Item, target.position, 12);
				DoLightningChain(target, damage);
			}
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
			if (active && Main.rand.Next(4) == 0 && overcharge < 30)
			{
				Main.PlaySound(SoundID.Item, target.position, 12);
				DoLightningChain(target, damage);
			}
		}

		private void DoLightningChain(NPC target, int damage)
		{
			overcharge += 15;
			Projectile.NewProjectile(target.position + new Vector2(target.width / 2, 0), Vector2.Zero, ModContent.ProjectileType<UkeleleProjTwo>(), 0, 0, player.whoAmI);
			int proj = Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<UkeleleProj>(), damage / 2, 0, player.whoAmI);
			if (Main.projectile[proj].modProjectile is UkeleleProj lightning)
			{
				lightning.currentEnemy = target;
				lightning.hit[5] = target;
			}
		}
	}

	public class UkeleleProj : ModProjectile
    {
        public NPC[] hit = new NPC[8];
		public NPC currentEnemy;
		int animCounter = 5;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ukelele");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 5;
			projectile.timeLeft = 300;
			projectile.aiStyle = -1;
			projectile.height = 68;
			projectile.width = 74;
			projectile.alpha = 255;
		}

		private int Mode {
			get => (int)projectile.ai[0];
			set => projectile.ai[0] = value;
		}

		private NPC Target {
			get => Main.npc[(int)projectile.ai[1]];
			set => projectile.ai[1] = value.whoAmI;
		}

		private void SetOrigin(Vector2 value)
		{
			projectile.localAI[0] = value.X;
			projectile.localAI[1] = value.Y;
		}

		public override void AI()
		{
			if (Mode == 0) {
				SetOrigin(projectile.position);
				Mode = 1;
			}
			else {
				if (Mode == 2) {
					projectile.extraUpdates = 0;
					projectile.numUpdates = 0;
				}
				if (projectile.timeLeft < 300) {
					animCounter--;
					if (animCounter > 0)
						projectile.Center = currentEnemy.Center; 
					if (animCounter == 1)
					{
						NPC target = TargetNext(currentEnemy);
						if (target != null && !target.friendly && !target.townNPC)
						{
							projectile.Center = target.Center;
							Trail(currentEnemy.position + new Vector2(currentEnemy.width / 2, 0), target.position + new Vector2(target.width / 2, 0));
							Target = target;
						}
						else
							projectile.Kill();
					}
				}
				SetOrigin(projectile.position);
			}
		}

		private void Trail(Vector2 from, Vector2 to)
		{
			float distance = Vector2.Distance(from, to);
			for (float w = 0; w < 1; w += 2 / distance) {
				Vector2 c1 = Vector2.Lerp(from, to, 0.5f) - new Vector2(0, distance / 3);
				Vector2 point = Helpers.TraverseBezier(from, to, c1, w);
				int d = Dust.NewDust(point, 7, 7, DustID.Electric, 0f, 0f, 0, default, .3f * projectile.penetrate);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity = Vector2.Zero;
				Main.dust[d].scale *= .7f;
			}
		}

		private bool CanTarget(NPC target)
		{
			foreach (var npc in hit)
				if (target == npc)
					return false;
			return true;
		}

		public NPC TargetNext(NPC current)
		{
			float range = 50 * 14;
			range *= range;
			NPC target = null;
			var center = projectile.Center;
			for (int i = 0; i < 200; ++i) {
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc != current && npc.CanBeChasedBy() && CanTarget(npc)) {
					float dist = Vector2.DistanceSquared(center, npc.Center);
					if (dist < range) {
						range = dist;
						target = npc;
					}
				}
			}
			return target;
		}

		public override bool? CanHitNPC(NPC target) => CanTarget(target) && target == Target;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.velocity = Vector2.Zero;
			hit[projectile.penetrate - 1] = target;
			currentEnemy = target;
			animCounter = 5;
			Projectile.NewProjectile(target.position + new Vector2(target.width / 2, 0), Vector2.Zero, ModContent.ProjectileType<UkeleleProjTwo>(), 0, 0, projectile.owner);
			projectile.netUpdate = true;
		}
	}

	public class UkeleleProjTwo : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ukelele");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 16;
			projectile.aiStyle = -1;
			projectile.height = 68;
			projectile.width = 74;
		}

		public override void AI()
		{
			projectile.frame = ((16 - projectile.timeLeft) / 4) - 1;
			Color color = Color.Cyan;
			Lighting.AddLight((int)((projectile.position.X + (projectile.width / 2)) / 16f), (int)((projectile.position.Y + (projectile.height / 2)) / 16f), color.R / 450f, color.G / 450f, color.B / 450f);
		}

		public override bool PreDraw(SpriteBatch sb, Color color)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / 4;
			sb.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Rectangle(0,projectile.frame * frameHeight,tex.Width, frameHeight), Color.White, projectile.rotation, new Vector2(tex.Width - (projectile.width / 2), frameHeight), projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
