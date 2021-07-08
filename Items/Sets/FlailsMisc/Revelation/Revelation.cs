using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.FlailsMisc.Revelation
{
	public class Revelation : BaseFlailItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Revelation");
			Tooltip.SetDefault("Update description later");
		}

		public override void SafeSetDefaults()
		{
			item.Size = new Vector2(34, 30);
			item.damage = 60;
			item.rare = ItemRarityID.Green;
			item.useTime = 30;
			item.useAnimation = 30;
			item.shoot = ModContent.ProjectileType<RevelationProj>();
			item.shootSpeed = 16;
			item.knockBack = 4;
		}
	}
	public class RevelationProj : BaseFlailProj
	{
		public RevelationProj() : base(new Vector2(0.7f, 1.3f), new Vector2(0.5f, 3f)) { }

		public override void SetStaticDefaults() => DisplayName.SetDefault("Revelation");

		public override void SpinExtras(Player player)
		{
			if (projectile.localAI[0] == 0)
			{
				SpiritMod.primitives.CreateTrail(new RevelationPrimTrailTwo(projectile, new Color(255, 0, 177), 14, 7, 0.75f));
				SpiritMod.primitives.CreateTrail(new RevelationPrimTrailTwo(projectile, Color.White, 6, 5, 0.8f));
			}
			if (++projectile.localAI[0] % 50 == 0)
				Projectile.NewProjectile(projectile.Center, Main.rand.NextVector2Circular(10, 10) + (Main.player[projectile.owner].velocity / 5), ModContent.ProjectileType<RevelationSoulWeak>(), projectile.damage, 0, projectile.owner);
			if (projectile.localAI[0] % 2 == 0)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PinkFlame);
		}

		public override void NotSpinningExtras(Player player)
		{
			if (++projectile.localAI[0] % 2 == 0)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PinkFlame);
		}
	}
	public class RevelationSoulWeak : ModProjectile
	{
		public virtual int Range => 300;

		public virtual int Duration => 90;

		public virtual int Speed => 10;

		protected Player Player => Main.player[projectile.owner];

		protected bool PrimsCreated = false;
		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.melee = true;
			projectile.Size = new Vector2(12, 12);
			projectile.tileCollide = false;
			projectile.timeLeft = Duration;
			projectile.penetrate = 1;
		}
		public override void AI()
		{
			if (!PrimsCreated)
			{
				PrimsCreated = true;
				SpiritMod.primitives.CreateTrail(new RevelationPrimTrailTwo(projectile, Color.HotPink, 11, 18, 1));
				SpiritMod.primitives.CreateTrail(new RevelationPrimTrail(projectile));
			}
			NPC npc = Projectiles.ProjectileExtras.FindNearestNPC(projectile.Center, Range, true);
			if (npc != null)
			{
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(npc.Center) * Speed, 0.1f);
			}
			else
			{
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Player.Center) * Speed, 0.02f);
			}
			projectile.rotation = projectile.velocity.ToRotation();
			if (++projectile.localAI[0] % 4 == 0)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PinkFlame);
		}
	}
}
