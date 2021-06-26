using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using SpiritMod.Prim;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Accessory.UmbillicalEyeball
{
	public class UmbillicalEyeball : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbillical Eyeballs");
			Tooltip.SetDefault("Summons 3 eyeballs attached to you\nThese eyeballs do not take up minion slots");
		}

		public override void SetDefaults()
		{
			item.damage = 80;
			item.summon = true;
			item.knockBack = 1.5f;
			item.width = 24;
			item.height = 24;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<EyeballPlayer>().EyeballMinion = true;
	}

	public class EyeballPlayer : ModPlayer
	{
		public bool EyeballMinion = false;

		public override void ResetEffects() => EyeballMinion = false;

		public override void PostUpdateEquips()
		{
			if (EyeballMinion)
				if (player.ownedProjectileCounts[ModContent.ProjectileType<UmbillicalEyeballProj>()] < 3)
					Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<UmbillicalEyeballProj>(), (int)(80 * player.minionDamage), 1.5f, player.whoAmI, player.ownedProjectileCounts[ModContent.ProjectileType<UmbillicalEyeballProj>()], 0);
		}
	}

	public class UmbillicalEyeballProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbillical Eyeball");
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.minion = true;
			projectile.minionSlots = 0;
			projectile.penetrate = -1;
			projectile.timeLeft = 216000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		bool attacking = false;
		bool primsCreated = false;
		public Vector2 control1;
		public Vector2 control2;
		Vector2 posToBe = Vector2.Zero;
		float rotationCounter = 0f;
		float attackCounter = 0f;
		Vector2 rotationVector = Vector2.UnitX;
		int circleX = 0;
		int circleY = 0;
		float circleSpeed = 0.05f;

		public override void AI()
		{
			if (!primsCreated)
			{
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new UmbillicalPrimTrail(projectile));
			}
			if (circleX == 0)
			{
				circleSpeed = Main.rand.NextFloat(0.03f, 0.06f);
				circleX = Main.rand.Next(40, 80);
				circleY = Main.rand.Next(20, 40);
			}

			Player player = Main.player[projectile.owner];
			EyeballPlayer modOwner = player.GetModPlayer<EyeballPlayer>();
			attackCounter = (Main.GameUpdateCount / 60f * 6.28f) + projectile.ai[0];
			rotationCounter += circleSpeed;

			if (player.dead)
				modOwner.EyeballMinion = false;
			if (modOwner.EyeballMinion)
				projectile.timeLeft = 2;

			float maxRange = 160f;
			int range = 10;

			if (projectile.ai[1] == -1) //boilerplate, sorry
			{
				float lowestDist = float.MaxValue;
				for (int i = 0; i < 200; ++i)
				{
					NPC npc = Main.npc[i];
					if (npc.active && npc.CanBeChasedBy(projectile) && !npc.friendly && !npc.noGravity)
					{
						float dist = projectile.Distance(npc.Center);
						if (dist / 16 < range)
						{
							if (dist < lowestDist)
							{
								lowestDist = dist;

								projectile.ai[1] = npc.whoAmI;
								projectile.netUpdate = true;
							}
						}
					}
				}
			}
			projectile.rotation = rotationVector.ToRotation();
			var circle = new Vector2(circleX * (float)Math.Sin(rotationCounter), circleY * (float)Math.Cos(rotationCounter));
			float speed = 0.5f;

			if (!attacking)
			{
				projectile.friendly = false;
				posToBe = Vector2.UnitY * -1;
				projectile.ai[1] = -1;
				int offset = 80;
				double angle;

				if (player.direction == 1)
				{
					angle = 1.3;
					control1 = (Vector2.UnitY * -120).RotatedBy(6.28 - 1.4) + player.Center + (circle.RotatedBy(angle) / 2);
					control2 = (Vector2.UnitY * -80).RotatedBy(0.4) + player.Center + (circle.RotatedBy(angle) / 5);
				}
				else
				{
					angle = MathHelper.TwoPi - 1.3;
					control1 = (Vector2.UnitY * -120).RotatedBy(1.4) + player.Center + (circle.RotatedBy(angle) / 2);
					control2 = (Vector2.UnitY * -80).RotatedBy(6.28 - 0.4) + player.Center + (circle.RotatedBy(angle) / 5);
				}

				posToBe *= offset;
				posToBe += circle;
				posToBe = posToBe.RotatedBy(angle);
				posToBe.Y -= 30;
				posToBe += player.Center;

				if (attackCounter % 7 > 6.8)
					attacking = true;
			}
			else
			{
				projectile.friendly = true;
				NPC target = Main.npc.Where(n => n.CanBeChasedBy(projectile, false) && Vector2.Distance(n.Center, projectile.Center) < maxRange).OrderBy(n => Vector2.Distance(n.Center, projectile.Center)).FirstOrDefault();
				if (target != default)
				{
					posToBe = target.Center;
					speed = 2;
				}
				else
					attacking = false;
			}

			Vector2 direction = posToBe - projectile.position;
			if (direction.Length() > 1000)
				projectile.position = posToBe;
			else
			{
				speed *= (float)Math.Sqrt(direction.Length());
				direction.Normalize();
				direction *= speed;

				projectile.velocity = direction;
			}

			projectile.spriteDirection = -player.direction;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => attacking = false;
	}

	class UmbillicalPrimTrail : PrimTrail
	{
		double _length = 1;

		public UmbillicalPrimTrail(Projectile projectile)
		{
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
		}

		public override void SetDefaults()
		{
			AlphaValue = 1f;
			Width = 8;
			Cap = 21;
			Pixellated = false;
			Color = Color.Pink;
		}

		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			/*if (PointCount <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(Counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(Points.Count) * Width;
            DrawBasicTrail(c1, widthVar);*/
			if (PointCount <= 6) return;

			double UVX = 0;
			double UVXNext = 0;
			for (int i = 0; i < Points.Count; i++)
			{
				if (i != 0 && i != Points.Count - 1)
				{
					Vector2 dir = Points[i] - Points[i + 1];
					UVXNext += dir.Length() / (_length * 32);
					Vector2 normal = CurveNormal(Points, i);
					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 firstUp = Points[i] - normal * Width;
					Vector2 firstDown = Points[i] + normal * Width;
					Vector2 secondUp = Points[i + 1] - normalAhead * Width;
					Vector2 secondDown = Points[i + 1] + normalAhead * Width;

					AddVertex(firstDown, Color * AlphaValue, new Vector2((float)UVX, 1));
					AddVertex(firstUp, Color * AlphaValue, new Vector2((float)UVX, 0));
					AddVertex(secondDown, Color * AlphaValue, new Vector2((float)UVXNext, 1));

					AddVertex(secondUp, Color * AlphaValue, new Vector2((float)UVXNext, 0));
					AddVertex(secondDown, Color * AlphaValue, new Vector2((float)UVXNext, 1));
					AddVertex(firstUp, Color * AlphaValue, new Vector2((float)UVX, 0));
					UVX = UVXNext;
				}
			}
		}
		public override void SetShaders()
		{
			Effect effect = SpiritMod.EyeballShader;
			if (effect.HasParameter("tentacleTexture"))
				effect.Parameters["tentacleTexture"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/EyeballTentacle"));
			PrepareShader(effect, "MainPS", (float)_length, Color);
		}

		public override void OnUpdate()
		{
			PointCount = Points.Count() * 6;
			if (Destroyed || !Entity.active)
				OnDestroy();
			else
			{
				List<Vector2> points = new List<Vector2>();
				if (Entity is Projectile proj && proj.modProjectile is UmbillicalEyeballProj modproj)
				{
					Player player = Main.player[proj.owner];
					points.Add(player.Center);
					Color = Color.White.MultiplyRGBA(Lighting.GetColor((int)proj.Center.X / 16, (int)proj.Center.Y / 16));
					for (float i = 0; i <= 1; i += 0.05f)
						points.Add(Helpers.TraverseBezier(proj.Center, player.Center, modproj.control1, modproj.control2, i));
					points.Add(proj.Center);
				}
				Points = points;

			}
			_length = 0;
			for (int i = 0; i < Points.Count() - 2; i++)
			{
				Vector2 dir = Points[i] - Points[i + 1];
				_length += dir.Length();
			}
			_length /= 32.0;
		}

		public override void OnDestroy() => Dispose();
	}
}
