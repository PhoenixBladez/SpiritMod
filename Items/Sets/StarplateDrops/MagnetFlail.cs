using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Prim;
using Terraria;
using SpiritMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.FlailsMisc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiritMod.Items.Sets.StarplateDrops
{
	public class MagnetFlail : BaseFlailItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Livewire");
			Tooltip.SetDefault("Plugs into tiles, changing the chain into a shocking livewire");

		}

		public override void SafeSetDefaults()
		{
			item.Size = new Vector2(34, 30);
			item.damage = 40;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 01, 10, 0);
			item.useTime = 30;
			item.useAnimation = 30;
			item.shoot = ModContent.ProjectileType<LivewireProj>();
			item.shootSpeed = 13;
			item.knockBack = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 18);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class LivewireProj : BaseFlailProj
	{
		private PlugTrail trail;

		private PlugTrail2 trail2;

		private NPC stickTarget;

		private Vector2 stuckPosition;

		private bool readyToStick = true;

		private bool stuck = false;

		private int stuckTimer = 0;

		private bool stuckToTiles = false;

		private bool oldTileCollide = false;

		private bool canHitTarget = false;
		public LivewireProj() : base(new Vector2(0.7f, 1), new Vector2(0.5f, 3f), 2, 50, 8) { }

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(8, 8);
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = -1;
		}

		public override void SetStaticDefaults() => DisplayName.SetDefault("Livewire");

		public override bool PreAI()
		{
			if (!released)
				return true;
			stuckTimer--;

			if (stuckTimer <= 0 && stuck)
			{
				trail.Destroyed = true;
				trail2.Destroyed = true;
				stuck = false;
				projectile.tileCollide = oldTileCollide;
			}

			if (stuck)
			{
				Vector2 vel = Vector2.UnitX.RotatedBy(Main.rand.NextFloat(6.28f));
				ParticleHandler.SpawnParticle(new ImpactLine(projectile.Center, vel, new Color(33, 211, 255), new Vector2(0.25f, 2f), 16));
				projectile.velocity = Vector2.Zero;
				projectile.tileCollide = false;
				if (stuckToTiles)
					projectile.Center = stuckPosition;
				else
				{
					if (!stickTarget.active)
						stuckTimer = 0;
					else
						projectile.Center = stickTarget.Center + stuckPosition;
				}

				return false;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (readyToStick && released)
			{
				trail2 = new PlugTrail2(projectile, Main.player[projectile.owner]);
				trail = new PlugTrail(projectile, Main.player[projectile.owner]);
				SpiritMod.primitives.CreateTrail(trail2);
				SpiritMod.primitives.CreateTrail(trail);
				oldTileCollide = projectile.tileCollide;
				readyToStick = false;
				stuck = true;
				stuckToTiles = false;
				stuckTimer = 100;
				stickTarget = target;
				stuckPosition = projectile.Center - target.Center;
				projectile.rotation = projectile.DirectionTo(target.Center).ToRotation() - 1.57f;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player player = Main.player[projectile.owner];
			float collisionPoint = 0f;
			if (stuck)
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center, projectile.Center, 12, ref collisionPoint);
			return base.Colliding(projHitbox, targetHitbox);
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (stuck)
			{
				if (stickTarget != target || canHitTarget)
					return base.CanHitNPC(target);
				return false;
			}
			else if (stickTarget != null && target == stickTarget)
				return false;

			return base.CanHitNPC(target);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (stuck)
			{
				damage = (int)(damage * 0.5f);
				if (stickTarget == target)
					canHitTarget = false;
				else
					canHitTarget = true;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!readyToStick)
			{
				if (stuck)
					return false;
				return base.OnTileCollide(oldVelocity);
			}

			trail2 = new PlugTrail2(projectile, Main.player[projectile.owner]);
			SpiritMod.primitives.CreateTrail(trail2);

			trail = new PlugTrail(projectile, Main.player[projectile.owner]);
			SpiritMod.primitives.CreateTrail(trail);

			oldTileCollide = projectile.tileCollide;
			readyToStick = false;
			stuck = true;
			stuckToTiles = true;
			stuckTimer = 100;
			stuckPosition = projectile.Center;
			struckTile = true;

			if (oldVelocity.X != projectile.velocity.X) //if its an X axis collision
			{
				if (projectile.velocity.X > 0)
					projectile.rotation = 1.57f;
				else
					projectile.rotation = 4.71f;
			}

			if (oldVelocity.Y != projectile.velocity.Y) //if its a Y axis collision
			{
				if (projectile.velocity.Y > 0)
					projectile.rotation = 0f;
				else
					projectile.rotation = 3.14f;
			}

			return base.OnTileCollide(oldVelocity);
		}

		public override bool PreDrawExtras(SpriteBatch spriteBatch)
		{
			if (stuck)
				return false;
			return base.PreDrawExtras(spriteBatch);
		}
	}
	class PlugTrail : PrimTrail
	{
		public PlugTrail(Projectile projectile, Player player)
		{
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;

			_target = player;
		}

		protected Player _target;
		public override void SetDefaults()
		{
			Pixellated = true;
			Width = 30;
			Cap = 40;
		}
		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			/*if (Points <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(Counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(Points.Count) * Width;
            DrawBasicTrail(c1, widthVar);*/
			if (PointCount <= 6) return;
			float widthVar;
			for (int i = 0; i < Points.Count; i++)
			{
				widthVar = Width;
				if (i == 0)
				{
					Color c1 = Counter % 33 > 20 && Counter % 33 < 32 ? Color.White : Color.Lerp(Color.Cyan, Color.White, Math.Min((widthVar - Width) / 5f, 1));
					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;
					AddVertex(Points[i], c1 * AlphaValue, new Vector2(0, 0.5f));
					AddVertex(secondUp, c1 * AlphaValue, new Vector2((float)(i + 1) / (float)Cap, 0));
					AddVertex(secondUp, c1 * AlphaValue, new Vector2((float)(i + 1) / (float)Cap, 1));
				}
				else
				{
					if (i != Points.Count - 1)
					{
						Color c = Counter % 33 > 20 && Counter % 33 < 32 ? Color.White : Color.Lerp(Color.Cyan, Color.White, Math.Min((widthVar - Width) / 5f, 1));
						Vector2 normal = CurveNormal(Points, i);
						Vector2 normalAhead = CurveNormal(Points, i + 1);
						Vector2 firstUp = Points[i] - normal * widthVar;
						Vector2 firstDown = Points[i] + normal * widthVar;
						Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
						Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

						AddVertex(firstDown, c * AlphaValue, new Vector2((float)(i / (float)Cap), 1));
						AddVertex(firstUp, c * AlphaValue, new Vector2((float)(i / (float)Cap), 0));
						AddVertex(secondDown, c * AlphaValue, new Vector2((float)(i + 1) / (float)Cap, 1));

						AddVertex(secondUp, c * AlphaValue, new Vector2((float)(i + 1) / (float)Cap, 0));
						AddVertex(secondDown, c * AlphaValue, new Vector2((float)(i + 1) / (float)Cap, 1));
						AddVertex(firstUp, c * AlphaValue, new Vector2((float)(i / (float)Cap), 0));
					}
					else
					{

					}
				}
			}
		}
		public override void SetShaders()
		{
			Vector2 lengthMeasure = Entity.Center - _target.Center;
			Effect effect = SpiritMod.TeslaShader;
			effect.Parameters["baseTexture"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/GlowTrail"));
			effect.Parameters["pnoise"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/noise"));
			effect.Parameters["vnoise"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/vnoise"));
			effect.Parameters["wnoise"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/wnoise"));
			effect.Parameters["repeats"].SetValue(lengthMeasure.Length() / 250f);
			PrepareShader(effect, "MainPS", Counter * 0.1f);
		}
		public override void OnUpdate()
		{
			Counter++;
			PointCount = Points.Count() * 6;

			if (Destroyed || _target.dead || !Entity.active)
			{
				OnDestroy();
			}
			else
			{
				Points.Clear();
				for (float i = 0; i < 1; i += 0.025f)
				{
					Points.Add(Vector2.Lerp(Entity.Center, _target.Center, i));
				}
			}
		}
		public override void OnDestroy()
		{
			Destroyed = true;
			Width *= 0.75f;
			if (Width < 0.05f)
			{
				Dispose();
			}
		}
	}
	class PlugTrail2 : PlugTrail
	{
		public PlugTrail2(Projectile projectile, Player player) : base(projectile, player)
		{

		}

		public override void SetDefaults()
		{
			Pixellated = true;
			Width = 3;
			Cap = 40;
		}
		public override void SetShaders()
		{
			Vector2 lengthMeasure = Entity.Center - _target.Center;
			Effect effect = SpiritMod.RepeatingTextureShader;
			effect.Parameters["baseTexture"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Items/Sets/StarplateDrops/LivewireProj_chain"));
			effect.Parameters["repeats"].SetValue(lengthMeasure.Length() / 12f);
			PrepareShader(effect, "MainPS", Counter * 0.1f);
		}
	}
}