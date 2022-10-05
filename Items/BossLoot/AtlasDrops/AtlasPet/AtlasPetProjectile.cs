using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.AtlasDrops.AtlasPet
{
	public class AtlasPetProjectile : ModProjectile
	{
		private const int HandOldPosSlot = 5;

		private Player Owner => Main.player[Projectile.owner];
		private NPC Target => Main.npc[(int)TargetNPC];
		private Vector2 HandPosition(Vector2 off) => Vector2.Lerp(Projectile.oldPos[HandOldPosSlot], Target.position + off, _handFactor);

		public ref float State => ref Projectile.ai[0];
		public ref float TargetNPC => ref Projectile.ai[1];

		private List<AtlasPetPart> _parts = new List<AtlasPetPart>();
		private float _handFactor = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lil' Scarab");
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.TrailCacheLength[Type] = 8;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Truffle);
			Projectile.aiStyle = 0;
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.light = 0;
			Projectile.tileCollide = false;

			AIType = 0;
		}

		public override void AI()
		{
			var modPlayer = Main.player[Projectile.owner].GetModPlayer<GlobalClasses.Players.PetPlayer>();

			if (Main.player[Projectile.owner].dead)
				modPlayer.scarabPet = false;

			if (modPlayer.scarabPet)
				Projectile.timeLeft = 2;

			if (State == 0)
				Initialize();
			else
			{
				Behaviour();
				UpdateParts();
			}
		}

		private void Behaviour()
		{
			bool dist = TargetNPC == -1 ? Projectile.DistanceSQ(Owner.Center) > 100 * 100 : Projectile.DistanceSQ(Target.Center) > 60 * 60;
			if (!Main.mouseLeft && dist)
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(TargetNPC != -1 ? Target.Center : Owner.Center) * 16, 0.015f);
			else
				Projectile.velocity *= TargetNPC == -1 ? 0.96f : 0.88f;

			if (TargetNPC == -1)
				FindTargetIfAny();
			else
				GrabTarget();
		}

		private void GrabTarget()
		{
			if (!Target.active || Target.life <= 0 || Target.DistanceSQ(Projectile.Center) > 700 * 700)
			{
				for (int i = 0; i < 10; ++i)
					Dust.NewDust(HandPosition(Vector2.Zero), 10, 10, Main.rand.NextBool() ? DustID.Shadowflame : DustID.Ebonwood);

				TargetNPC = -1;
				_handFactor = 0;
				return;
			}

			if (TargetNPC != Owner.lastCreatureHit && Owner.lastCreatureHit >= 0 && Main.npc[Owner.lastCreatureHit].CanBeChasedBy())
				TargetNPC = Owner.lastCreatureHit;

			_handFactor += 0.01f;

			if (_handFactor > 1f)
				_handFactor = 1f;
		}

		private void FindTargetIfAny()
		{
			if (Owner.lastCreatureHit >= 0 && Main.npc[Owner.lastCreatureHit].CanBeChasedBy())
			{
				TargetNPC = Owner.lastCreatureHit;
				return;
			}

			for (int i = 0; i < Main.maxNPCs; ++i)
			{
				NPC npc = Main.npc[i];

				if (npc.CanBeChasedBy() && npc.DistanceSQ(Projectile.Center) < 600 * 600 && (TargetNPC == -1 || Target.DistanceSQ(Projectile.Center) > npc.DistanceSQ(Projectile.Center)))
					TargetNPC = npc.whoAmI;
			}
		}

		private void UpdateParts()
		{
			foreach (var item in _parts)
			{
				item.Update();

				int slot = new int[4] { HandOldPosSlot, 0, 2, HandOldPosSlot }[item.column];
				item.position = Projectile.oldPos[slot];

				float adj = Projectile.oldPos[slot].X - Projectile.oldPos[slot + 1].X;
				item.effects = Math.Sign(float.IsNaN(adj) ? 0 : adj) == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

				if ((item.column == 0 || item.column == 3) && TargetNPC != -1)
				{
					item.position = HandPosition(item.column == 0 ? new Vector2(10, 0) : new Vector2(-10, 0));
					item.effects = Target.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				}
			}
		}

		private void Initialize()
		{
			for (int i = 0; i < 4; ++i)
			{
				int col = i;
				AtlasPetPart part = new AtlasPetPart()
				{ 
					column = col,
					position = Projectile.Center
				};

				_parts.Add(part);
			}

			TargetNPC = -1;
			State = 1;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			foreach (var item in _parts)
				item.Draw();
			return false;
		}
	}
}
