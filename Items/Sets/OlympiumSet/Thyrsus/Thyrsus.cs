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
			Item.damage = 50;
			Item.noMelee = true;
			Item.rare = ItemRarityID.LightRed;
			Item.width = 18;
			Item.height = 18;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 24;
			Item.knockBack = 8;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<ThyrsusProj>();
			Item.shootSpeed = 10f;
			Item.value = Item.sellPrice(0, 2, 0, 0);
		}
	}
	public class ThyrsusProj : ModProjectile
	{
		bool stuck = false;
		float shrinkCounter = 0.25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thyrsus");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.minion = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			AIType = ProjectileID.ThrowingKnife;
		}
		public override bool PreAI()
		{
			if (stuck)
			{
				Projectile.velocity = Vector2.Zero;
				if (Projectile.timeLeft < 40)
				{
					shrinkCounter += 0.1f;
					Projectile.scale = 0.75f + (float)(Math.Sin(shrinkCounter));
					if (Projectile.scale < 0.3f)
					{
						Projectile.active = false;
					}
					if (Projectile.scale > 1)
						Projectile.scale = ((Projectile.scale - 1) / 2f) + 1;
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
				Projectile.tileCollide = false;
				Projectile.timeLeft = 900;
				Vector2 direction = new Vector2(oldVelocity.X == Projectile.velocity.X ? 0 : 0 - Math.Sign(oldVelocity.X), oldVelocity.Y == Projectile.velocity.Y ? 0 : 0 - Math.Sign(oldVelocity.Y));
				direction.Normalize();
				for (int i = 0; i < 3; i++)
				{
					Projectile proj = Projectile.NewProjectileDirect(Projectile.Center, direction.RotatedBy(Main.rand.NextFloat(-1f, 1f)) * 5, ModContent.ProjectileType<ThyrsusProjTwo>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
					if (proj.ModProjectile is ThyrsusProjTwo modProj)
					{
						modProj.InitializeChain(Projectile.Center);
						modProj.initialVelocity = proj.velocity;
						modProj.sinMult = Main.rand.NextFloat(0.03f, 0.1f);
					}
				}
				Projectile.velocity = Vector2.Zero;
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

		private Projectile Parent => Main.projectile[(int)Projectile.ai[0]];

		private float Distance => (Parent.Center - Projectile.Center).Length();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vine");
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 0;
			Projectile.width = Projectile.height = 8;
			Projectile.timeLeft = 750;
			Projectile.ignoreWater = true;
		}
		public void InitializeChain(Vector2 position) => _chain = new Chain(8, chainSegments, position, new ChainPhysics(0.9f, 0.5f, 0f), true, true, 2);

		public override void AI()
		{
			if (!Parent.active)
			{
				Projectile.active = false;
				return;
			}
			else
				Projectile.timeLeft = 2;
			float ChainLength = 26 * chainSegments;
			NPC target = Main.npc.Where(n => n.CanBeChasedBy(Projectile, false) && Vector2.Distance(n.Center, Parent.Center) < ChainLength).OrderBy(n => Vector2.Distance(n.Center, Parent.Center)).FirstOrDefault();

			if (Main.netMode != NetmodeID.Server)
				_chain.Update(Parent.Center, Projectile.position);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (_chain == null)
				return false;

			_chain.Draw(spriteBatch, ModContent.Request<Texture2D>(Texture + "_chain"));
			return false;
		}
	}
}