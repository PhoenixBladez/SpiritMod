using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;
using SpiritMod.VerletChains;

namespace SpiritMod.Items.Sets.OlympiumSet.Thyrsus
{

	//TODO: Make it so you cant throw one if theres one active
	//TODO: Make it so right click destroys the currently alive one
	public class Thyrsus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thyrsus");
			Tooltip.SetDefault("Throw at a surface to grow damaging vines");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.noMelee = true;
			item.rare = ItemRarityID.LightRed;
			item.width = 18;
			item.height = 18;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 24;
			item.knockBack = 8;
			item.magic = true;
			item.noMelee = true;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<ThyrsusProj>();
			item.shootSpeed = 10f;
			item.value = Item.sellPrice(0, 2, 0, 0);
		}
	}
	public class ThyrsusProj : ModProjectile
	{
		bool stuck = false;
		float shrinkCounter = 0.25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thyrsus");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 1;
			projectile.friendly = false;
			projectile.minion = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			aiType = ProjectileID.ThrowingKnife;
		}
		public override bool PreAI()
		{
			if (stuck)
			{
				projectile.velocity = Vector2.Zero;
				if (projectile.timeLeft < 40)
				{
					shrinkCounter += 0.1f;
					projectile.scale = 0.75f + (float)(Math.Sin(shrinkCounter));
					if (projectile.scale < 0.3f)
					{
						projectile.active = false;
					}
					if (projectile.scale > 1)
						projectile.scale = ((projectile.scale - 1) / 2f) + 1;
				}
				return false;
			}
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//TODO: Playsound here
			if (!stuck)
			{
				stuck = true;
				projectile.tileCollide = false;
				projectile.timeLeft = 900;
				Vector2 direction = new Vector2(oldVelocity.X == projectile.velocity.X ? 0 : 0 - Math.Sign(oldVelocity.X), oldVelocity.Y == projectile.velocity.Y ? 0 : 0 - Math.Sign(oldVelocity.Y));
				direction.Normalize();
				for (int i = 0; i < 3; i++)
				{
					Projectile proj = Projectile.NewProjectileDirect(projectile.Center, direction.RotatedBy(Main.rand.NextFloat(-1f, 1f)) * 5, ModContent.ProjectileType<ThyrsusProjTwo>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.whoAmI);
					if (proj.modProjectile is ThyrsusProjTwo modProj)
					{
						modProj.InitializeChain(projectile.Center);
						modProj.initialVelocity = proj.velocity;
						modProj.sinMult = Main.rand.NextFloat(0.03f, 0.1f);
					}
				}
				projectile.velocity = Vector2.Zero;
			}
			return false;
		}
	}
	public class ThyrsusProjTwo : ModProjectile
	{
		public Vector2 initialVelocity;
		private Chain _chain;

		public float sinMult;

		private int chainSegments = 10;

		private Projectile Parent => Main.projectile[(int)projectile.ai[0]];

		private float Distance => (Parent.Center - projectile.Center).Length();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vine");
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 0;
			projectile.width = projectile.height = 8;
			projectile.timeLeft = 750;
			projectile.ignoreWater = true;
		}
		public void InitializeChain(Vector2 position) => _chain = new Chain(ModContent.GetTexture(Texture + "_chain"), 8, chainSegments, position, new ChainPhysics(0.9f, 0.5f, 0f), true, true, 2);

		public override void AI()
		{
			if (!Parent.active)
			{
				projectile.active = false;
				return;
			}
			else
				projectile.timeLeft = 2;
			float ChainLength = 26 * chainSegments;
			NPC target = Main.npc.Where(n => n.CanBeChasedBy(projectile, false) && Vector2.Distance(n.Center, Parent.Center) < ChainLength).OrderBy(n => Vector2.Distance(n.Center, Parent.Center)).FirstOrDefault();

			if (Main.netMode != NetmodeID.Server)
				_chain.Update(Parent.Center, projectile.position);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (_chain == null)
				return false;

			_chain.Draw(spriteBatch, out float endrot, out Vector2 endpos);
			return false;
		}
	}
}