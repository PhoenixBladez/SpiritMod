using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.StarjinxSet.Sagittarius;
using SpiritMod.Utilities;

namespace SpiritMod.Mechanics.Trails
{
	public enum TrailLayer
	{
		UnderProjectile,
		UnderCachedProjsBehindNPC
	}

	public class TrailManager
	{
		private readonly List<BaseTrail> _trails = new List<BaseTrail>();
		private readonly Effect _effect;
		private readonly BasicEffect _basicEffect;

		public TrailManager(Mod mod)
		{
			_trails = new List<BaseTrail>();
			_effect = mod.GetEffect("Effects/trailShaders");
			_basicEffect = new BasicEffect(Main.graphics.GraphicsDevice)
			{
				VertexColorEnabled = true
			};
		}

		public void TryTrailKill(Projectile projectile)
		{
			//todo: refactor this to be based on the itrailprojectile interface? wanted to have a dissolve speed parameter with a default value so it doesnt need to be specified but not possible in this version of C#
			if (projectile.type == ModContent.ProjectileType<SleepingStar>() || projectile.type == ModContent.ProjectileType<LeafProjReachChest>() || projectile.type == ModContent.ProjectileType<HallowedStaffProj>() || projectile.type == ModContent.ProjectileType<TrueHallowedStaffProj>() || projectile.type == ModContent.ProjectileType<PositiveArrow>() || projectile.type == ModContent.ProjectileType<NegativeArrow>() || projectile.type == ModContent.ProjectileType<PartyStarterBullet>() || projectile.type == ModContent.ProjectileType<SandWall>() || projectile.type == ModContent.ProjectileType<SandWall2>())
			{
				SpiritMod.TrailManager.TryEndTrail(projectile, Math.Max(15f, projectile.velocity.Length() * 3f));
			}
			if (projectile.type == ModContent.ProjectileType<OrichHoming>() || projectile.type == ModContent.ProjectileType<DarkAnima>())
			{
				SpiritMod.TrailManager.TryEndTrail(projectile, Math.Max(2f, projectile.velocity.Length() * 1f));
			}
			if (projectile.type == ModContent.ProjectileType<Starshock1>())
			{
				SpiritMod.TrailManager.TryEndTrail(projectile, Math.Max(1f, projectile.velocity.Length() * .6f));
			}

			if (projectile.type == ModContent.ProjectileType<SagittariusConstellationArrow>())
				SpiritMod.TrailManager.TryEndTrail(projectile, Math.Max(1f, projectile.velocity.Length() * 6));

			if (projectile.type == ModContent.ProjectileType<SagittariusArrow>())
				SpiritMod.TrailManager.TryEndTrail(projectile, Math.Max(1f, projectile.velocity.Length() * 10));

			//This is where it tries to kill a trail (assuming said projectile is linked to a trail)
			//here you can specify dissolve speed. however, I recommend you just keep using the same case
			//like so:
			/*
            switch (projectile.type)
            {
                case ProjectileID.WoodenArrowFriendly:
                case ProjectileID.WoodenArrowHostile:
                case ProjectileID.MmmmmYesProjectile:
                case ProjectileID.WowCool:
                case ProjectileID.Spaghetti:
                    GimmeCraftingBenchAchievement.TrailManager.TryEndTrail(projectile, Math.Max(15f, projectile.velocity.Length() * 3f));
                    break;
            } 
            */
			switch (projectile.type)
			{
				case ProjectileID.WoodenArrowFriendly:
					SpiritMod.TrailManager.TryEndTrail(projectile, Math.Max(15f, projectile.velocity.Length() * 3f));
					break;
			}
		}

		public void CreateTrail(Projectile projectile, ITrailColor trailType, ITrailCap trailCap, ITrailPosition trailPosition, float widthAtFront, float maxLength, ITrailShader shader = null, TrailLayer layer = TrailLayer.UnderProjectile, float dissolveSpeed = -1)
		{
			var newTrail = new Trail(projectile, trailType, trailCap, trailPosition, shader ?? new DefaultShader(), layer, widthAtFront, maxLength, dissolveSpeed);
			newTrail.BaseUpdate();
			_trails.Add(newTrail);
		}

		public void CreateCustomTrail(BaseTrail trail)
		{
			trail.BaseUpdate();
			_trails.Add(trail);
		}

		public void UpdateTrails()
		{
			for (int i = 0; i < _trails.Count; i++)
			{
				BaseTrail trail = _trails[i];

				trail.BaseUpdate();
				if (trail.Dead)
				{
					_trails.RemoveAt(i);
					i--;
				}
			}
		}

		public void ClearAllTrails() => _trails.Clear();

		public void DrawTrails(SpriteBatch spriteBatch, TrailLayer layer)
		{
			foreach (BaseTrail trail in _trails)
			{
				if (trail.Layer == layer)
					trail.Draw(_effect, _basicEffect, spriteBatch.GraphicsDevice);
			}
		}

		public void TryEndTrail(Projectile projectile, float dissolveSpeed)
		{
			for (int i = 0; i < _trails.Count; i++)
			{
				BaseTrail trail = _trails[i];

				if (trail.MyProjectile.whoAmI == projectile.whoAmI && trail is Trail t)
				{
					t.DissolveSpeed = dissolveSpeed;
					t.StartDissolve();
					return;
				}
			}
		}

		public static void ManualTrailSpawn(Projectile projectile)
		{
			if (projectile.modProjectile is IManualTrailProjectile)
			{
				if (Main.netMode == NetmodeID.SinglePlayer)
					(projectile.modProjectile as IManualTrailProjectile).DoTrailCreation(SpiritMod.TrailManager);

				else
					SpiritMod.WriteToPacket(SpiritMod.Instance.GetPacket(), (byte)MessageType.SpawnTrail, projectile.whoAmI).Send();
			}
		}
	}

	public abstract class BaseTrail
	{
		public bool Dead { get; set; } = false;
		public Projectile MyProjectile { get; set; }
		public TrailLayer Layer { get; set; }

		private readonly int _originalProjectileType;
		private bool _dissolving = false;

		public BaseTrail(Projectile projectile, TrailLayer layer)
		{
			MyProjectile = projectile;
			Layer = layer;
			_originalProjectileType = projectile.type;
		}

		public void BaseUpdate()
		{
			if ((!MyProjectile.active || MyProjectile.type != _originalProjectileType) && !_dissolving)
				StartDissolve();

			if (_dissolving)
				Dissolve();
			else
				Update();
		}

		public void StartDissolve()
		{
			OnStartDissolve();
			_dissolving = true;
		}

		/// <summary>
		/// Behavior for the trail every tick, only called before the trail begins dying
		/// </summary>
		public virtual void Update() { }

		/// <summary>
		/// Behavior for the trail after it starts its death, called every tick after the trail begins dying
		/// </summary>
		public virtual void Dissolve() { }

		/// <summary>
		/// Additional behavior for the trail upon starting its death
		/// </summary>
		public virtual void OnStartDissolve() { }

		/// <summary>
		/// How the trail is drawn to the screen
		/// </summary>
		/// <param name="effect"></param>
		/// <param name="effect2"></param>
		/// <param name="device"></param>
		public virtual void Draw(Effect effect, BasicEffect effect2, GraphicsDevice device)
		{

		}
	}

}