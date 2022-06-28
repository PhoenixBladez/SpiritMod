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
using System.IO;

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
			Item.Size = new Vector2(34, 30);
			Item.damage = 40;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 01, 10, 0);
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.shoot = ModContent.ProjectileType<LivewireProj>();
			Item.shootSpeed = 13;
			Item.knockBack = 4;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 18);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
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
			Projectile.Size = new Vector2(8, 8);
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults() => DisplayName.SetDefault("Livewire");

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.heldProj = Projectile.whoAmI;

			if (!released)
				return true;
			stuckTimer--;

			if (stuckTimer <= 0 && stuck)
			{
				trail.Destroyed = true;
				trail2.Destroyed = true;
				stuck = false;
				Projectile.tileCollide = oldTileCollide;
			}

			if (stuck)
			{

				Vector2 vel = Vector2.UnitX.RotatedBy(Main.rand.NextFloat(6.28f));
				ParticleHandler.SpawnParticle(new ImpactLine(Projectile.Center, vel, new Color(33, 211, 255), new Vector2(0.25f, 1f), 16));

				float lerper = Main.rand.NextFloat();
				ParticleHandler.SpawnParticle(new GlowParticle(Vector2.Lerp(Projectile.Center, player.Center, lerper), Vector2.Zero, new Color(33, 151, 255), 0.1f, 30));
				Projectile.velocity = Vector2.Zero;
				Projectile.tileCollide = false;
				if (stuckToTiles)
					Projectile.Center = stuckPosition;
				else
				{
					if (!stickTarget.active)
						stuckTimer = 0;
					else
						Projectile.Center = stickTarget.Center + stuckPosition;
				}

				return false;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (readyToStick && released && target.life > 0)
			{
				trail2 = new PlugTrail2(Projectile, Main.player[Projectile.owner]);
				trail = new PlugTrail(Projectile, Main.player[Projectile.owner]);
				SpiritMod.primitives.CreateTrail(trail2);
				SpiritMod.primitives.CreateTrail(trail);
				oldTileCollide = Projectile.tileCollide;
				readyToStick = false;
				stuck = true;
				stuckToTiles = false;
				stuckTimer = 100;
				stickTarget = target;
				stuckPosition = Projectile.Center - target.Center;
				Projectile.rotation = Projectile.DirectionTo(target.Center).ToRotation() - 1.57f;
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player player = Main.player[Projectile.owner];
			float collisionPoint = 0f;
			if (stuck)
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center, Projectile.Center, 12, ref collisionPoint);
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
				{
					canHitTarget = false;
					for (int i = 0; i < 20; i++)
					{
						Vector2 vel = Main.rand.NextVector2Circular(35, 25);
						ParticleHandler.SpawnParticle(new GlowParticle(Projectile.Center, vel, new Color(33, 211, 255), 0.05f, 10));
					}
				}
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

			trail2 = new PlugTrail2(Projectile, Main.player[Projectile.owner]);
			SpiritMod.primitives.CreateTrail(trail2);

			trail = new PlugTrail(Projectile, Main.player[Projectile.owner]);
			SpiritMod.primitives.CreateTrail(trail);

			oldTileCollide = Projectile.tileCollide;
			readyToStick = false;
			stuck = true;
			stuckToTiles = true;
			stuckTimer = 100;
			stuckPosition = Projectile.Center;
			struckTile = true;

			if (oldVelocity.X != Projectile.velocity.X) //if its an X axis collision
			{
				if (Projectile.velocity.X > 0)
					Projectile.rotation = 1.57f;
				else
					Projectile.rotation = 4.71f;
			}

			if (oldVelocity.Y != Projectile.velocity.Y) //if its a Y axis collision
			{
				if (Projectile.velocity.Y > 0)
					Projectile.rotation = 0f;
				else
					Projectile.rotation = 3.14f;
			}

			return base.OnTileCollide(oldVelocity);
		}

		public override bool PreDrawExtras()
		{
			if (stuck)
				return false;
			return base.PreDrawExtras(Main.spriteBatch);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(oldTileCollide);
			writer.Write(readyToStick);
			writer.Write(stuck);
			writer.Write(stuckToTiles);
			writer.Write(stuckTimer);
			writer.WriteVector2(stuckPosition);
			if (stickTarget == default(NPC)) //Write a -1 instead if the npc isnt set
				writer.Write(-1);
			else
				writer.Write(stickTarget.whoAmI);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			oldTileCollide = reader.ReadBoolean();
			readyToStick = reader.ReadBoolean();
			stuck = reader.ReadBoolean();
			stuckToTiles = reader.ReadBoolean();
			stuckTimer = reader.ReadInt32();
			stuckPosition = reader.ReadVector2();
			int whoAmI = reader.ReadInt32(); //Read the whoami value sent
			if (whoAmI == -1) //If its a -1, sync that the npc hasn't been set yet
				stickTarget = default;
			else
				stickTarget = Main.npc[whoAmI];

			if (stuck)
			{
				if (trail2 == null)
				{
					trail2 = new PlugTrail2(Projectile, Main.player[Projectile.owner]);
					SpiritMod.primitives.CreateTrail(trail2);
				}
				if (trail == null)
				{
					trail = new PlugTrail(Projectile, Main.player[Projectile.owner]);
					SpiritMod.primitives.CreateTrail(trail);
				}
			}
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